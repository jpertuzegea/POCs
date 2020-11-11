using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.Configuration
{
    public struct QueuesStruct
    {
        public string QueueName;
        public int PreFetchCount;
        public int PreFetchCountFail;
        public int TimeRetry;
    }
    public struct ContingencyStruct
    {
    }

    public class QueueManagerConfig
    {
        public string hostName { get; set; }
        public string virtualHost { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string exchange { get; set; }
        public int port { get; set; }
        public string serialization { get; set; }
        public bool useSSL;
        public int MaxRabbitMQReconnectionAttempts { get; set; }
        public int IntervalBetweenReconnectionAttempts { get; set; }
        public string sourceEvent { get; set; }
        public QueuesStruct[] Queues;
        public ContingencyStruct Contingency;
    }

}


