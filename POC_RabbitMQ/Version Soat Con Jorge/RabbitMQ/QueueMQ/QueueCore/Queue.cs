using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using QueueMQ.Configuration;
using QueueMQ.Model;
using QueueMQ.Helper;
using System.Threading; 
using Models.Process.Interfaces;
using Models.Configuration.SubModels;

namespace QueueMQ.QueueCore
{
    public sealed class Queue<T> : IQueue<T>, IDisposable where T : IInputModel, new()
    {
        private static Dictionary<string, Queue<T>> _instances;
        private static Dictionary<string, Queue<T>> instances
        {
            get
            {
                if (_instances == null)
                {
                    _instances = new Dictionary<string, Queue<T>>();
                }
                return _instances;
            }
            set
            {
                _instances = value;
            }
        }

        private static object syncRoot;

        private string hostName = "guest";

        private bool useSSL;

        private int port = 5672;

        private string virtualHost = "/";

        private string userName = "guest";

        private string password = "guest";

        private string sourceEvent = "Jorge.ConsumerQueue";

        private string queueName;

        //Ajuste de PrefetchCount en reconección
        private ushort prefetchCount;

        private Func<T, IDictionary<string, object>, IOutputModel> onMessageAction;

        private bool autoAck;

        private bool isSubscription;
        //Bandera para colas temporales.
        private bool exclusive;

        private static QueueManagerConfig config;

        private ConnectionMQ _CONFIGURATION;

        private string exchangeName;

        private string exchangeType;

        private bool durableExchange = true;

        private bool persistentMessages;

        private string routingKey;

        private Serializers serialization = Serializers.JSON;

        private ISerializer serializer;

#pragma warning disable CS0649 // El campo 'Queue<T>.receivedMessageHandler' nunca se asigna y siempre tendrá el valor predeterminado null
        private MethodInfo receivedMessageHandler;
#pragma warning restore CS0649 // El campo 'Queue<T>.receivedMessageHandler' nunca se asigna y siempre tendrá el valor predeterminado null

        private IConnection queueConnection;

        private IModel queueModel;

        //private List<Task<MessageHandler>> tasksConsumer = new List<Task<MessageHandler>>();

        //private QueueDeclareOk queueDeclared;

        static Queue()
        {
            Queue<T>.syncRoot = new object();
        }

        public void SetConfig(ConnectionMQ _CONNECTION, QueueConfiguration _QUEUECONF)
        {
            this.hostName = _CONNECTION.Host != null ? _CONNECTION.Host : hostName;
            this.useSSL = _CONNECTION.UseSSL;
            this.port = _CONNECTION.Port;
            this.virtualHost = _CONNECTION.vHost != null ? _CONNECTION.vHost : virtualHost;
            this.userName = _CONNECTION.User != null ? _CONNECTION.User : userName;
            this.password = _CONNECTION.Password != null ? _CONNECTION.Password : password;
            this.sourceEvent = _CONNECTION.SourceEvent != null ? _CONNECTION.SourceEvent : sourceEvent;
            string serialization = _CONNECTION.Serialization != null ? _CONNECTION.Serialization : "None";
            switch (serialization)
            {
                case "JSON":
                    this.serialization = Serializers.JSON;
                    break;
                case "Binary":
                    this.serialization = Serializers.Binary;
                    break;
                case "None":
                    this.serialization = Serializers.None;
                    break;
            }
        }


        private Queue(Serializers? serializer, string exchangeName = null, string exchangeType = null, bool durableExchange = true, bool persistentMessages = false, string routingKey = null, ushort? prefetchCount = null, Dictionary<string, object> arguments = null, ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null)
        {
            SetConfig(_CONNECTION, _QUEUECONF);
            this.queueName = _QUEUECONF.QueueName;
            if (prefetchCount.HasValue)
                this.prefetchCount = prefetchCount.Value;
            if (serializer.HasValue)
            {
                this.serialization = serializer.Value;
            }

            this.CreateSerializer();
            this.exchangeName = exchangeName;
            this.exchangeType = exchangeType;
            this.durableExchange = durableExchange;
            this.persistentMessages = persistentMessages;
            this.routingKey = routingKey;
            this.exclusive = this.queueName == string.Empty ? true : false;
            this.ConnectToQueue(prefetchCount, arguments);
        }

        private Queue(ushort? prefetchCount = null, Dictionary<string, object> arguments = null, ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null)
        {
            SetConfig(_CONNECTION, _QUEUECONF);
            this.queueName = _QUEUECONF.QueueName;
            if (prefetchCount.HasValue)
                this.prefetchCount = prefetchCount.Value;
            this.exclusive = this.queueName == string.Empty ? true : false;
            this.ConnectToQueue(prefetchCount, arguments);
        }

        private void Cleanup()
        {
            try
            {
                if ((this.queueConnection == null ? false : this.queueConnection.IsOpen))
                {
                    Console.WriteLine("Cleanup");
                    this.queueConnection.ConnectionShutdown -= new EventHandler<ShutdownEventArgs>(this.Connection_ConnectionShutdown);
                    this.queueConnection.Close();
                    this.queueConnection.Dispose();
                    this.queueConnection = null;
                }
                if ((this.queueModel == null || (!this.queueModel.IsOpen) ? false : !this.queueModel.IsClosed))
                {
                    this.queueModel.Close();
                    this.queueModel.Dispose();
                    this.queueModel = null;
                }
            }
#pragma warning disable CS0168 // La variable 'Ex' se ha declarado pero nunca se usa
            catch (Exception Ex)
#pragma warning restore CS0168 // La variable 'Ex' se ha declarado pero nunca se usa
            {
                //throw Ex;
            }
        }

        private void Connect()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = this.hostName,
                VirtualHost = this.virtualHost,
                UserName = this.userName,
                Password = this.password,
                Port = this.port
            };
            connectionFactory.RequestedHeartbeat = 60;
            connectionFactory.Ssl.Enabled = false;
            connectionFactory.AutomaticRecoveryEnabled = true;
            connectionFactory.NetworkRecoveryInterval = new TimeSpan(0, 0, 10);
            connectionFactory.RequestedHeartbeat = 60;
            this.queueConnection = connectionFactory.CreateConnection();
            //this.queueConnection.AutoClose = false;
            this.queueConnection.ConnectionShutdown += new EventHandler<ShutdownEventArgs>(this.Connection_ConnectionShutdown);
            this.queueModel = this.queueConnection.CreateModel();
            this.queueModel.BasicQos(0, this.prefetchCount, false);
            if (!string.IsNullOrEmpty(this.exchangeName))
            {
                this.queueModel.ExchangeDeclare(this.exchangeName, (!string.IsNullOrEmpty(this.exchangeType) ? this.exchangeType : "direct"), this.durableExchange);
            }
        }

        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            this.Cleanup();
            this.Reconnect();
        }

        private void ConnectToQueue(ushort? prefetchCount = null, Dictionary<string, object> args = null)
        {
            this.Connect();
            if (prefetchCount.HasValue)
            {
                this.queueModel.BasicQos(0, prefetchCount.Value, false);
            }
            //Ajuste para manejo de colas exclusivas a conexión
            this.queueModel.QueueDeclare(this.queueName, true, this.exclusive, false, args);
            if (!string.IsNullOrEmpty(this.exchangeName))
            {
                this.queueModel.QueueBind(this.queueName, this.exchangeName, this.routingKey ?? string.Empty);
            }
        }

        private void CreateSerializer()
        {
            switch (this.serialization)
            {
                case Serializers.JSON:
                    {
                        this.serializer = new JsonSerializer();
                        break;
                    }
                case Serializers.Binary:
                    {
                        this.serializer = new BinarySerializer();
                        break;
                    }
                case Serializers.None:
                    {
                        this.serializer = new NoneSerializer();
                        break;
                    }
                default:
                    {
                        QMLogHelper.SingleInstance.Register(new QueueManagerLogEntry("Creando serializador", "QueueManager.dll", "InvalidSerializationMethod", "", null), System.Diagnostics.EventLogEntryType.Error);
                        break;
                    }
            }
        }

        public void AckMessage(ulong DeliveryTag)
        {
            queueModel.BasicAck(DeliveryTag, false);
        }

        public void NackMessage(ulong DeliveryTag)
        {
            queueModel.BasicNack(DeliveryTag, false, true);
        }

        private void CreateSubscription()
        {
            EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(this.queueModel);
            eventingBasicConsumer.Received += new EventHandler<BasicDeliverEventArgs>((object ch, BasicDeliverEventArgs ea) => {
                byte[] body = ea.Body;
                //tasksConsumer.Where(pTask => pTask.IsCompleted && pTask.Result.Complete).ToArray().AsParallel().WithDegreeOfParallelism(8).Select((pTask) => { tasksConsumer.Remove(pTask); pTask.Dispose(); return 0; }).ToArray();
                object Message = serializer.Deserialize(ea.Body);
                T DeserializedMessage = default(T);
                try
                {
                    DeserializedMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Message.ToString());
                }
                catch (Exception Ex)
                {
                    QMLogHelper.SingleInstance.Register(new QueueManagerLogEntry("Procesando mensage", "QueueManager.dll", "Invalid Input Object", Ex.Message, Ex), System.Diagnostics.EventLogEntryType.Error);
                }
                if (DeserializedMessage != null)
                {
                    Task<MessageHandler<T>> Controller = new MessageHandler<T>(
                        onMessageAction, //Delegado de control de mensaje
                        AckMessage, //Función para hacer ACK
                        NackMessage, //Función para reencolar
                        autoAck, //Indica si se están haciendo AutoACK
                        DeserializedMessage,//De tipo Input Message
                        ea.RoutingKey, //Llave de enrutamiento
                        ea.DeliveryTag, //Identificador del mensaje
                        ea.BasicProperties //Propiedades
                    ).RunMessageHandler();
                    //tasksConsumer.Add(Controller);
                    Controller.Start();
                }
                else if (!autoAck)
                {
                    AckMessage(ea.DeliveryTag);
                }
            });
            this.queueModel.BasicConsume(this.queueName, this.autoAck, eventingBasicConsumer);
        }

        public void Dispose()
        {
            this.queueConnection.ConnectionShutdown -= new EventHandler<ShutdownEventArgs>(this.Connection_ConnectionShutdown);
            this.Cleanup();
        }
        //Validar llamados
        internal static Queue<T> GetInstance(Serializers? serializer, string exchangeName = null, string exchangeType = null, bool durableExchange = true, bool persistentMessages = false, string routingKey = null, ushort? prefetchCount = null, Dictionary<string, object> arguments = null, ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null)
        {
            Queue<T> queue;
            string str = _QUEUECONF.QueueName;
            if (_QUEUECONF.Exchange)
            {
                str = string.Concat(str, string.Format("||{0}", _QUEUECONF.QueueName));
                if (!string.IsNullOrEmpty(routingKey))
                {
                    str = string.Concat(str, string.Format("||{0}", routingKey));
                }
            }
            if ((!Queue<T>.instances.ContainsKey(str) ? true : Queue<T>.instances[str] == null))
            {
                lock (Queue<T>.syncRoot)
                {
                    if ((!Queue<T>.instances.ContainsKey(str) ? true : Queue<T>.instances[str] == null))
                    {
                        Queue<T> queue1 = new Queue<T>(serializer, exchangeName, exchangeType, durableExchange, persistentMessages, routingKey, prefetchCount, arguments, _CONNECTION, _QUEUECONF);
                        Queue<T>.instances.Add(str, queue1);
                        queue = queue1;
                        return queue;
                    }
                }
            }
            Queue<T> item = Queue<T>.instances[str];
            lock (Queue<T>.syncRoot)
            {
                item.Cleanup();
                item.Reconnect();
            }
            queue = item;
            return queue;
        }

        internal static Queue<T> GetInstance(string queueName, ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null)
        {
            Queue<T> queue;
            if ((!Queue<T>.instances.ContainsKey(_QUEUECONF.QueueName) ? true : Queue<T>.instances[_QUEUECONF.QueueName] == null))
            {
                Queue<T> queue1 = null;
                lock (Queue<T>.syncRoot)
                {
                    if ((!Queue<T>.instances.ContainsKey(_QUEUECONF.QueueName) ? true : Queue<T>.instances[_QUEUECONF.QueueName] == null))
                    {
                        queue1 = new Queue<T>(null, null, _CONNECTION: _CONNECTION, _QUEUECONF: _QUEUECONF);
                        Queue<T>.instances.Add(queueName, queue1);
                        queue = queue1;
                        return queue;
                    }
                }
            }
            Queue<T> item = Queue<T>.instances[_QUEUECONF.QueueName];
            lock (Queue<T>.syncRoot)
            {
                item.Cleanup();
                item.Reconnect();
            }
            queue = item;
            return queue;
        }

        public void PutOnQueue(object item, int retryCount)
        {
            this.WriteOnDurableRabbitMQ(item, true, retryCount);
        }
        public object ReceiveFromQueue()
        {
            object obj;
            BasicGetResult basicGetResult = this.queueModel.BasicGet(this.queueName, true);
            if (basicGetResult == null)
            {
                obj = null;
            }
            else
            {
                IBasicProperties basicProperties = basicGetResult.BasicProperties;
                byte[] body = basicGetResult.Body;
                obj = this.serializer.Deserialize(body);
            }
            return obj;
        }

        private void Reconnect()
        {
            bool flag;
            int num = 0;
            ManualResetEventSlim manualResetEventSlim = new ManualResetEventSlim(false);
            int num1 = Queue<T>.config.MaxRabbitMQReconnectionAttempts == 0 ? -1 : 0;
            int num2 = Queue<T>.config.IntervalBetweenReconnectionAttempts == 0 ? 5000 : 0;
            while (true)
            {
                if (manualResetEventSlim.Wait(num2))
                {
                    flag = false;
                }
                else
                {
                    flag = (num1 < 0 ? true : num <= num1);
                }
                if (!flag)
                {
                    break;
                }
                try
                {
                    num++;
                    this.Connect();
                    if (this.isSubscription)
                    {
                        this.CreateSubscription();
                    }
                    break;
                }
#pragma warning disable CS0168 // La variable 'Ex' se ha declarado pero nunca se usa
                catch (Exception Ex)
#pragma warning restore CS0168 // La variable 'Ex' se ha declarado pero nunca se usa
                {
                    if (num == num1)
                    {
                        throw;
                    }
                    //throw Ex;
                }
            }
        }

        public void SubscribeToQueue()
        {
            if (this.receivedMessageHandler == null)
            {
                throw new InvalidOperationException("The received message handler is not set.");
            }
            EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(this.queueModel);
            eventingBasicConsumer.Received += new EventHandler<BasicDeliverEventArgs>((object ch, BasicDeliverEventArgs ea) => {
                byte[] body = ea.Body;
                var variable = new { Body = this.serializer.Deserialize(body), RoutingKey = ea.RoutingKey };
                this.receivedMessageHandler.Invoke(null, new object[] { variable });
            });
            this.queueModel.BasicConsume(this.queueName, this.autoAck, eventingBasicConsumer);
        }

        public void SubscribeToQueue(Func<T, IDictionary<string, object>, IOutputModel> messageHandler, bool noAck = true)
        {
            this.onMessageAction = messageHandler;
            this.autoAck = noAck;
            this.CreateSubscription();
            this.isSubscription = true;
        }

        private void WriteOnDurableRabbitMQ(object item, bool persistent = true, int retryCount = 0)
        {
            BasicProperties properties = new BasicProperties()
            {
                DeliveryMode = (persistent ? Convert.ToByte(2) : Convert.ToByte(1)),
                Persistent = persistent
            };
            string GUID = Guid.NewGuid().ToString().Replace("-", "");
            properties.Headers = new Dictionary<string, object>
            {
                { "x-deduplication-header", GUID },
                { "retryCount", retryCount }
            };
            byte[] numArray = this.serializer.Serialize(item);
            this.queueModel.BasicPublish(this.exchangeName ?? string.Empty, (!string.IsNullOrEmpty(this.routingKey) ? this.routingKey : this.queueName), properties, numArray);
        }
    }

}
