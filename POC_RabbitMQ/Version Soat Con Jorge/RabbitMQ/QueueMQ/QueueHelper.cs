

namespace QueueMQ
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using QueueMQ.QueueCore;
    using QueueMQ.QueueConsumer;
    using System.Linq;
    using QueueMQ.Configuration;
    using QueueMQ.Helper;
    using Models.Configuration.SubModels;
    using Models.Configuration.SubModels.Enum;
    using Models.Result;
    using Models.Process.Interfaces;
    using Models.Result.Enum;
    using Models.Logging;

    public class QueueHelper : IQueueHelper
    {
        public const int _PREFETCH_COUNT_DEFAULT = 10;
        public const int _PREFETCH_COUNT_FAIL_DEFAULT = 10;
        public const int _TIMERETRY_DEFAULT = 10;
        public static QueueHelper _instance;
        public static Dictionary<string, ConnectionMQ> _Configuration;
        public static ConnectionMQ Configuration
        {
            get
            {
                throw new InvalidOperationException(RecursosQueueManager.InvalidConfiguration);
            }
            set
            {
                if (_Configuration == null)
                {
                    _Configuration = new Dictionary<string, ConnectionMQ>();
                }
                if (value.Type == QueueType.RabbitMQ)
                {
                    _Configuration.Add(value.Name, value);
                }
                else
                {
                    throw new InvalidOperationException(RecursosQueueManager.InvalidQueueType);
                }
            }
        }
        public IResult<bool, ErrorModel> SetConfiguration(ConnectionMQ Configuration)
        {
            QueueHelper.Configuration = Configuration;
            return new ResultValue<bool, ErrorModel>(true);
        }
        public static QueueHelper instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new QueueHelper();
                }
                return _instance;
            }
        }
        private void PutOnQueue<T>(CreateQueueParameters createQueueParameters, object item, string Serialization = "", string EventSource = "", ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null, int retryCount = 0) where T : IInputModel, new()
        {
            try
            {
                createQueueParameters.Serialization = Serialization;
                IQueue<T> Queue = QueueInstance<T>.GetQueueInstance(createQueueParameters, _CONNECTION, _QUEUECONF);
                Queue.PutOnQueue(item, retryCount);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(EventSource, ex.Message, EventLogEntryType.Error);
                throw ex;
            }
        }
        public IResult<T, ErrorModel> PutOnQueueJsonByQueue<T>(string ConnectionName, object item, string keyQueue, string RoutingKey = "", int retryCount = 0) where T : IInputModel, new()
        {
            string queueName = keyQueue;
            RoutingKey = RoutingKey == "" ? keyQueue : RoutingKey;
            QueuesStruct QueueConf;
            QueueConfiguration ConnectionDetails = QueueConfiguration.Default;
            //bool returnValue = false;
            try
            {
                if (_Configuration.ContainsKey(ConnectionName))
                {
                    try
                    {
                        ConnectionDetails = _Configuration[ConnectionName].QueueConfigurations.Where(q => q.QueueName == queueName).First();
                    }
                    catch (Exception Ex)
                    {
                        ConnectionDetails.QueueName = queueName;
                        QMLogHelper.SingleInstance.Register(new RegisterEntry(
                                "PutOnQueueJsonByQueue " + (typeof(T).FullName),
                                "Sistran.QueueManager.dll",
                                String.Empty,
                                String.Empty,
                                Ex
                            ), EventLogEntryType.Warning);
                    }

                }
                else
                {
                    ConnectionDetails.QueueName = queueName;
                    QMLogHelper.SingleInstance.Register(new RegisterEntry(
                            "PutOnQueueJsonByQueue " + (typeof(T).FullName),
                            "Sistran.QueueManager.dll",
                            "No existe el nombre de la conexión",
                            String.Empty
                        ), EventLogEntryType.Error);
                    return new ResultError<T, ErrorModel>(ErrorModel.CreateErrorModel(
                        "No existe el nombre de la conexión",
                        ErrorType.TechnicalFault
                        ));
                }

                QueueConf = new QueuesStruct()
                {
                    QueueName = queueName,
                    PreFetchCount = ConnectionDetails.PreFetchCount,
                    PreFetchCountFail = ConnectionDetails.PreFetchCountFail,
                    TimeRetry = ConnectionDetails.TimeRetry
                };
                //returnValue = true;

            }
            catch (Exception Ex)
            {
                QueueConf = new QueuesStruct()
                {
                    QueueName = queueName,
                    PreFetchCount = _PREFETCH_COUNT_DEFAULT,
                    PreFetchCountFail = _PREFETCH_COUNT_FAIL_DEFAULT,
                    TimeRetry = _TIMERETRY_DEFAULT,
                };
                EventLog.WriteEntry(Configuration.SourceEvent, Ex.Message, EventLogEntryType.Warning);
                //returnValue = false;
            }
            Dictionary<string, object> args = new Dictionary<string, object>
            {
                { ArgumentsNames.MessageDeduplication,true}
            };
            CreateQueueParameters createQueueParameters = new CreateQueueParameters
            {
                QueueName = queueName,
                RoutingKey = RoutingKey,
                PrefetchCount = ushort.Parse(QueueConf.PreFetchCount.ToString()),
                PersistentMessages = true,
                Arguments = args
            };
            try
            {
                PutOnQueue<T>(createQueueParameters, item, _Configuration[ConnectionName].Serialization, _Configuration[ConnectionName].SourceEvent, _Configuration[ConnectionName], ConnectionDetails, retryCount);
                return new ResultValue<T, ErrorModel>(new T());
            }
            catch (Exception Ex)
            {
                return new ResultError<T, ErrorModel>(ErrorModel.CreateErrorModel(RecursosQueueManager.PutOnQueueError, ErrorType.TechnicalFault, Ex));
            }
        }

        public IResult<bool, ErrorModel> PutOnQueueJsonByExchange<T>(string ConnectionName, object item, string keyExchangeName, string ExchangeType = "fanout", string RoutingKey = "", string Serialization = "", string EventSource = "") where T : IInputModel, new()
        {
            try
            {
                ConnectionMQ ConnectionDetails;
                if (_Configuration.ContainsKey(ConnectionName))
                {
                    try
                    {
                        ConnectionDetails = _Configuration[ConnectionName];
                    }
                    catch (Exception Ex)
                    {
                        QMLogHelper.SingleInstance.Register(new RegisterEntry(
                                "PutOnQueueJsonByExchange " + (typeof(T).FullName),
                                "Sistran.QueueManager.dll",
                                String.Empty,
                                String.Empty,
                                Ex
                            ), EventLogEntryType.Warning);
                    }

                }
                else
                {
                    QMLogHelper.SingleInstance.Register(new RegisterEntry(
                            "PutOnQueueJsonByExchange " + (typeof(T).FullName),
                            "Sistran.QueueManager.dll",
                            "No existe el nombre de la conexión",
                            String.Empty
                        ), EventLogEntryType.Error);
                    return new ResultError<bool, ErrorModel>(ErrorModel.CreateErrorModel(
                        "No existe el nombre de la conexión",
                        ErrorType.TechnicalFault
                        ));
                }
                string exchangeName = keyExchangeName;
                if (string.IsNullOrEmpty(exchangeName))
                {
                    exchangeName = keyExchangeName;
                }
                Dictionary<string, object> args = new Dictionary<string, object>
                {
                    { ArgumentsNames.DeadLetterExchange, exchangeName },
                    { ArgumentsNames.MaxLength, 500 },
                    { ArgumentsNames.MessageDeduplication,true}

                };
                CreateQueueParameters createQueueParameters = new CreateQueueParameters
                {
                    QueueName = string.Empty,
                    ExchangeName = exchangeName,
                    ExchangeType = ExchangeType,
                    RoutingKey = RoutingKey,
                    PersistentMessages = true,
                    Arguments = args
                };
                PutOnQueue<T>(createQueueParameters, item, Configuration.Serialization, Configuration.SourceEvent);
                return new ResultValue<bool, ErrorModel>(true);
            }
            catch (Exception Ex)
            {
                EventLog.WriteEntry(Configuration.SourceEvent, Ex.Message, EventLogEntryType.Error);
                return new ResultError<bool, ErrorModel>(ErrorModel.CreateErrorModel(
                    Ex.Message,
                    ErrorType.TechnicalFault,
                    Ex
                    ));
            }
        }
    }
}
