using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.Monitoring.Models
{
    public class BindingMonitoring
    {
        public string Source { get; set; }
        public string VHost { get; set; }
        public string Destination { get; set; }
        public string Destination_Type { get; set; }
        public string Routing_Key { get; set; }
        public string Properties_Key { get; set; }
    }
}
