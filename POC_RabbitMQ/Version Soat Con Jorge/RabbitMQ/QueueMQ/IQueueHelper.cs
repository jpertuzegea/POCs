using Models.Process.Interfaces;
using Models.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ
{ 

    public interface IQueueHelper
    {
        IResult<T, ErrorModel> PutOnQueueJsonByQueue<T>(string ConnectionName, object item, string keyQueue, string RoutingKey = "", int retryCount = 0) where T : IInputModel, new();
        IResult<bool, ErrorModel> PutOnQueueJsonByExchange<T>(string ConnectionName, object item, string keyExchangeName, string ExchangeType = "fanout", string RoutingKey = "", string Serialization = "", string EventSource = "") where T : IInputModel, new();
    }
}
