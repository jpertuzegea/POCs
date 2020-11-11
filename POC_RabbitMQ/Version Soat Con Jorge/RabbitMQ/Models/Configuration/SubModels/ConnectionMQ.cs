using Models.Configuration.SubModels.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic; 

namespace Models.Configuration.SubModels
{
    public class ConnectionMQ
    {
        public string Name;
        [JsonConverter(typeof(StringEnumConverter))]
        public QueueType Type;
        public string Host;
        public string vHost;
        public int Port;
        public string User;
        public string Password;
        public string Serialization;
        public bool UseSSL;
        public int MaxReconnectionAttempts;
        public int IntervalBetweenReconnectionAttempts;
        public string SourceEvent;
        public QueueConfiguration[] QueueConfigurations;
    }
}
