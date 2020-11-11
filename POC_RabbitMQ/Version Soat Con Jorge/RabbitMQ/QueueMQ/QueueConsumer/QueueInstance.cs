using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.QueueConsumer
{
    using System.Collections.Generic; 
    using QueueMQ.QueueCore; 
    using Models.Configuration.SubModels;
    using Models.Process.Interfaces;

    public class QueueInstance<T> where T : IInputModel, new()
    {
        private static readonly object _lockIQueue = new object();
        private static IQueue<T> queueInstance = null;
        private static readonly Dictionary<string, IQueue<T>> dictionary = new Dictionary<string, IQueue<T>>();
        private QueueConfiguration _QUEUECONFIG;
        private ConnectionMQ _CONNECTIONCONFIG;

        public static IQueue<T> GetQueueInstance(ConnectionMQ _CONNECTION, QueueConfiguration _QUEUECONF)
        {
            lock (_lockIQueue)
            {
                CreateQueueParameters createQueueParameters = new CreateQueueParameters
                {
                    QueueName = _QUEUECONF.QueueName,
                    RoutingKey = _QUEUECONF.QueueName,
                    PrefetchCount = (ushort)_QUEUECONF.PreFetchCount,
                    PersistentMessages = true
                };
                queueInstance = GetQueueInstance(createQueueParameters, _CONNECTION, _QUEUECONF);
            }
            return queueInstance;
        }

        public static IQueue<T> GetQueueInstance(CreateQueueParameters createQueueParameters, ConnectionMQ _CONNECTION, QueueConfiguration _QUEUECONF)
        {
            lock (_lockIQueue)
            {
                if (queueInstance == null || !dictionary.ContainsKey(createQueueParameters.QueueName))
                {

                    queueInstance = new BaseQueueFactory<T>().CreateQueue(createQueueParameters, _CONNECTION: _CONNECTION, _QUEUECONF: _QUEUECONF);
                    dictionary.Add(createQueueParameters.QueueName, queueInstance);
                }
                else
                {
                    queueInstance = dictionary[createQueueParameters.QueueName];
                }
            }
            return queueInstance;
        }
    }
}
