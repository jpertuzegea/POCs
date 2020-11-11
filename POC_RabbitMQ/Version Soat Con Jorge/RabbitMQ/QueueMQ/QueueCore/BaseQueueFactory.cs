using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.QueueCore
{
    using Models.Configuration.SubModels;
    using Models.Process.Interfaces;
    using System;
    using System.Collections.Generic; 

    public class BaseQueueFactory<T> : IQueueFactory<T> where T : IInputModel, new()
    {
        public BaseQueueFactory()
        {
        }

        public IQueue<T> CreateQueue(ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null)
        {
            return this.CreateQueue(null, null, null, null, _CONNECTION: _CONNECTION, _QUEUECONF: _QUEUECONF);
        }

        [Obsolete("This overload is deprecated. Please use CreateQueue(CreateQueueParameters createQueueParameters) overload")]
        public IQueue<T> CreateQueue(string serialization, ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null)
        {
            return this.CreateQueue(null, null, serialization, null, _CONNECTION: _CONNECTION, _QUEUECONF: _QUEUECONF);
        }

        [Obsolete("This overload is deprecated. Please use CreateQueue(CreateQueueParameters createQueueParameters) overload")]
        public IQueue<T> CreateQueue(string exchangeName, string routingKey, ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null)
        {
            ushort? nullable = null;
            IQueue<T> queue = this.CreateQueue(exchangeName, null, routingKey, nullable, null, _CONNECTION: _CONNECTION, _QUEUECONF: _QUEUECONF);
            return queue;
        }

        [Obsolete("This overload is deprecated. Please use CreateQueue(CreateQueueParameters createQueueParameters) overload")]
        public IQueue<T> CreateQueue(Dictionary<string, object> arguments, ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null)
        {
            ushort? nullable = null;
            IQueue<T> queue = this.CreateQueue(null, null, true, false, null, null, nullable, arguments, _CONNECTION: _CONNECTION, _QUEUECONF: _QUEUECONF);
            return queue;
        }

        [Obsolete("This overload is deprecated. Please use CreateQueue(CreateQueueParameters createQueueParameters) overload")]
        public IQueue<T> CreateQueue(string serialization, Dictionary<string, object> arguments, ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null)
        {
            ushort? nullable = null;
            IQueue<T> queue = this.CreateQueue(null, null, true, false, null, serialization, nullable, arguments, _CONNECTION: _CONNECTION, _QUEUECONF: _QUEUECONF);
            return queue;
        }

        [Obsolete("This overload is deprecated. Please use CreateQueue(CreateQueueParameters createQueueParameters) overload")]
        public IQueue<T> CreateQueue(string exchangeName = null, string routingKey = null, string serialization = null, ushort? prefetchCount = null, Dictionary<string, object> arguments = null, ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null)
        {
            IQueue<T> queue = this.CreateQueue(exchangeName, null, true, false, routingKey, serialization, prefetchCount, arguments, _CONNECTION: _CONNECTION, _QUEUECONF: _QUEUECONF);
            return queue;
        }

        private IQueue<T> CreateQueue(string exchangeName = null, string routingKey = null, string serialization = null, Dictionary<string, object> arguments = null, ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null)
        {
            Serializers? nullable = null;
            if (!string.IsNullOrEmpty(serialization))
            {
                nullable = new Serializers?((Serializers)System.Enum.Parse(typeof(Serializers), serialization));
            }
            ushort? nullable1 = null;
            Queue<T> instance = Queue<T>.GetInstance(nullable, exchangeName, null, true, false, routingKey, nullable1, arguments, _CONNECTION: _CONNECTION, _QUEUECONF: _QUEUECONF);
            return instance;
        }

        private IQueue<T> CreateQueue(string exchangeName = null, string exchangeType = null, bool durableExchange = true, bool persistentMessages = false, string routingKey = null, string serialization = null, ushort? prefetchCount = null, Dictionary<string, object> arguments = null, string hostNameMQ = "localhost", bool useSSLMQ = false, int portMQ = 5672, string virtualHostMQ = "/", string userNameMQ = "guest", string passwordMQ = "guest", string SerializationMQ = "None", string sourceEventMQ = "Jorge.ConsumerQueue", ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null)
        {
            Serializers? nullable = null;
            if (!string.IsNullOrEmpty(serialization))
            {
                nullable = new Serializers?((Serializers)System.Enum.Parse(typeof(Serializers), serialization));
            }
            Queue<T> instance = Queue<T>.GetInstance(nullable, exchangeName, exchangeType, durableExchange, persistentMessages, routingKey, prefetchCount, arguments, _CONNECTION: _CONNECTION, _QUEUECONF: _QUEUECONF);
            return instance;
        }

        public IQueue<T> CreateQueue(CreateQueueParameters createQueueParameters, ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null)
        {
            Serializers? nullable = null;
            if (!string.IsNullOrEmpty(createQueueParameters.Serialization))
            {
                nullable = new Serializers?((Serializers)System.Enum.Parse(typeof(Serializers), createQueueParameters.Serialization));
            }
            Queue<T> instance = Queue<T>.GetInstance(nullable, createQueueParameters.ExchangeName, createQueueParameters.ExchangeType, createQueueParameters.DurableExchange, createQueueParameters.PersistentMessages, createQueueParameters.RoutingKey, new ushort?(createQueueParameters.PrefetchCount), createQueueParameters.Arguments, _CONNECTION: _CONNECTION, _QUEUECONF: _QUEUECONF);
            return instance;
        }
    }
}
