using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.Monitoring.Models
{
    public class UrlServiceMonitoring
    {
        public string RabbitURL { get; set; }
        public string Proxy { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
