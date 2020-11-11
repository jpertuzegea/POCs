using Models.Configuration.SubModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Configuration
{
    public class AppSettings : IAppSettings
    {
        public Dictionary<string, object> AppKeys { get; set; }
        //public LoggingConfiguration Logging { get; set; }
        //public DataBaseAccess DataBaseAccess { get; set; }
        public ConnectionMQ[] QueueAccess { get; set; }
        public EventViewerConfiguration EventViewer { get; set; }
    }
}
