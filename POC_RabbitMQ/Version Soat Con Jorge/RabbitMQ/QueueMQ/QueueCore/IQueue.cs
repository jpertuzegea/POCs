using Models.Process.Interfaces; 
using System;
using System.Collections.Generic;
using System.Text;


namespace QueueMQ.QueueCore
{
    public interface IQueue<T> where T : IInputModel, new()
    {
        void PutOnQueue(object item, int retryCount);
        object ReceiveFromQueue();
        void SubscribeToQueue();
        void SubscribeToQueue(Func<T, IDictionary<string, object>, IOutputModel> messageHandler, bool noAck = true);
        void AckMessage(ulong DeliveryTag);
    }
}
