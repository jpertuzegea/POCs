

namespace QueueMQ.Helper
{
    using System.Diagnostics;
    using System.Reflection;
    using QueueMQ.Model;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class QMJSONConfigManagerGeneric<T>
    {
        private T config;
        public QMJSONConfigManagerGeneric(string ConfigName = "config.json")
        {
            if (System.IO.File.Exists(ConfigName))
            {
                config = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(ConfigName));
            }
            else
            {
                string temporalPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + ConfigName.TrimStart('\\');
                if (System.IO.File.Exists(temporalPath))
                {
                    config = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(temporalPath));
                }
                else
                {
                    QueueManagerLogEntry qmex = new QueueManagerLogEntry("Cargando archivo de configuración", "Sistran.Utilities.dll", "Erorr accediendo a archivo de configuración", temporalPath);
                    QMLogHelper.SingleInstance.Register(qmex, EventLogEntryType.Error);
                    throw qmex;
                    //config = default(T);
                }
            }
        }
        public T GetConfig()
        {
            return config;
        }
    }

}
