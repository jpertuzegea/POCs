using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.Monitoring.Models
{
    public class ExchangeMonitoring
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string VHost { get; set; }
    }
}
