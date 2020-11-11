using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.Monitoring.Models
{
    public class ServiceMonitoring
    {
        public string GetExhanges { get; set; }
        public string GetQueue { get; set; }
        public string GetBindings { get; set; }
    }

}
