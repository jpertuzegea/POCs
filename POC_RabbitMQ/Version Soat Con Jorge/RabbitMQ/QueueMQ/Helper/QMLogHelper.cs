 

namespace QueueMQ.Helper
{

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using QueueMQ.Enum;
     using Models.Configuration.SubModels;
    using Models.Configuration.SubModels.Enum;

    public class QMLogHelper //: ILogHelper
    {
        private const string _PREFIX = "";
        private static readonly object ob = new object();
        private const string _MACHINENAME = ".";
        private static QMLogHelper _SingleInstance;
        private static EventViewerConfiguration _Config;

        public static EventViewerConfiguration Config
        {
            get
            {
                if (_Config == null)
                {
                    throw new InvalidOperationException("No se ha asignado la configuración del controlador de Log");
                }
                return _Config;
            }
            set
            {
                _Config = value;
            }
        }

        public void SetConfiguration(EventViewerConfiguration Config)
        {
            QMLogHelper.Config = Config;
        }

        public static QMLogHelper SingleInstance
        {
            get
            {
                if (_SingleInstance == null)
                {
                    lock (ob)
                    {
                        if (_SingleInstance == null)
                        {
                            _SingleInstance = new QMLogHelper();
                        }
                    }
                }
                return _SingleInstance;
            }
        }

        private void WriteInEventViewer(string messageLog, string SourceName, EventLogEntryType eventType, int eventID)
        {
            string logName = SourceName;
            try
            {
                if (!EventLog.Exists(SourceName))
                {
                    EventSourceCreationData objOrigenEvento = new EventSourceCreationData(SourceName, logName);
                }

                EventLog objEvento = new EventLog(SourceName, _MACHINENAME, logName);

                objEvento.WriteEntry(messageLog, eventType, eventID);
            }
            catch (Exception Ex)
            {

                // Jorge Pertuz Egea
                //string query = (new SQLiteCommandTextBuilder()).InsertIntoValues("errorLog", new Dictionary<string, object>() {
                //    { "logName",logName },
                //    { "sourceName",SourceName},
                //    { "severity",eventType.ToString()},
                //    { "description",$"Exception:{Ex.Message}\n InnerMessage:{(Ex.InnerException!=null?Ex.InnerException.Message:String.Empty)}"},
                //    { "date",DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff")}
                //}).ToString();
                

                //var result = SQLiteController.instance.executeInsert(query, new string[] { "errorLog" }, false);
                
            }
        }

        public void Register(Exception ex, EventLogEntryType SeverityLevel, QMLogName LogName = QMLogName.QMRabbitMQ, int eventID = 0)
        {
            int eventIDCalculate = eventID;
            switch (SeverityLevel)
            {
                case EventLogEntryType.FailureAudit:
                case EventLogEntryType.Information:
                case EventLogEntryType.SuccessAudit:
                    if (Config.Configuration == EventViewerLevel.ALL)
                    {
                        SingleInstance.WriteInEventViewer(ex.Message + "\n" + (ex.InnerException != null ? ex.InnerException.Message : String.Empty), LogName.ToString(), SeverityLevel, eventIDCalculate);
                    }
                    break;
                case EventLogEntryType.Warning:
                case EventLogEntryType.Error:
                    SingleInstance.WriteInEventViewer(ex.Message + "\n" + (ex.InnerException != null ? ex.InnerException.Message : String.Empty), LogName.ToString(), SeverityLevel, eventIDCalculate);
                    break;
                default:
                    break;

            }

        }
    }

}
