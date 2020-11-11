 
using System;
using System.Collections.Generic;
using System.Text;
using Models.Configuration.SubModels;
using Models.Process.Interfaces;

namespace QueueMQ.QueueCore
{
    public interface IQueueFactory<T> where T : IInputModel, new()
    {
        IQueue<T> CreateQueue(ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null);

        [Obsolete("This overload is deprecated. Please use CreateQueue(CreateQueueParameters createQueueParameters) overload")]
        IQueue<T> CreateQueue(Dictionary<string, object> arguments, ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null);

        [Obsolete("This overload is deprecated. Please use CreateQueue(CreateQueueParameters createQueueParameters) overload")]
        IQueue<T> CreateQueue(string serialization, ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null);

        [Obsolete("This overload is deprecated. Please use CreateQueue(CreateQueueParameters createQueueParameters) overload")]
        IQueue<T> CreateQueue(string serialization, Dictionary<string, object> arguments, ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null);

        IQueue<T> CreateQueue(CreateQueueParameters createQueueParameters, ConnectionMQ _CONNECTION = null, QueueConfiguration _QUEUECONF = null);
    }
}
