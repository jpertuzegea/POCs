using Models.Configuration;
using Models.Configuration.SubModels;
using Newtonsoft.Json;
using POC_Queue.Enum;
using POC_Queue.Models;
using QueueMQ;
using QueueMQ.Helper;
using System;
using System.Linq;
using Utilities.Helper;

namespace POC_Queue
{
    class Program
    {
        private static AppSettings _appSettings; // esto no debe ser static
        private static ConnectionMQ _cmq; // esto no debe ser static
        private static int _retryNumber; // esto no debe ser static


        public static AppSettings AppSettings
        {
            get
            {
                if (_appSettings == null)
                {
                    _appSettings = new AppSettings();
                }
                return _appSettings;
            }
            set
            {
                _appSettings = value;
            }
        }


        static void Main(string[] args)
        {

            // --------------------------- ctor ---------------------------
            AppSettings = HelperConfiguration<AppSettings>.JSONConfigManagerGeneric();
            AppSettings.QueueAccess.Select(p =>
            {
                QueueHelper.Configuration = p;
                return true;
            }
            ).ToList();
            //_logger = logger;
            Program._cmq = AppSettings.QueueAccess.FirstOrDefault(c => c.Name == EnumQueue.Principal.ToString());
            _retryNumber = 3;
            //LogHelper.SingleInstance.SetConfiguration(AppSettings.EventViewer);
            QMLogHelper.SingleInstance.SetConfiguration(AppSettings.EventViewer);
            // --------------------------- ctor ---------------------------


            Persona Persona = new Persona();
            Persona.cedula = 123;
            Persona.Nombre = "Katherine ";
            Persona.Apellido = "Forero";
            Persona.Telefono = "312";
            Persona.ModelId = Guid.NewGuid().ToString();

           

            try
            {
                QueueHelper.instance.PutOnQueueJsonByQueue<Persona>("Principal", JsonConvert.SerializeObject(Persona), EnumQueue.PocJorgeQueue.ToString());// , (int)retryCount + 1);
            }
            catch (Exception Ex)
            {
                throw;
            }

        }
    }
}
