using Models.Configuration;
using Models.Configuration.SubModels;
using Newtonsoft.Json;
using POC_Queue.Enum;
using POC_Queue.Models;
using QueueMQ;
using QueueMQ.Helper;
using System;
using System.Diagnostics;
using System.Linq;
using Utilities.Helper;

namespace POC_Queue
{
    class Program
    {
        private static AppSettings _appSettings; // esto no debe ser static
        private static ConnectionMQ _cmq; // esto no debe ser static
        private static int _retryNumber; // esto no debe ser static

        public static Stopwatch stopWath = new Stopwatch();// Instancia Cronometro


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


            stopWath.Start(); // Inica Cronometro 

            int CantidadTotal = 63629;

            Persona Persona = new Persona();
            Persona.cedula = 123;
            Persona.Nombre = "Katherine ";
            Persona.Apellido = "Forero";
            Persona.Telefono = "312";
            

            try
            {

                for (int i = 0; i < CantidadTotal; i++)
                {
                    Persona.ModelId = Guid.NewGuid().ToString();
                    QueueHelper.instance.PutOnQueueJsonByQueue<Persona>("Principal", JsonConvert.SerializeObject(Persona), EnumQueue.PocJorgeQueuexx.ToString());// , (int)retryCount + 1);
                    Console.WriteLine($"Registro Encolado: { i }");
                }

                stopWath.Stop();// Finaliza Cronometro


                // ---------------------------- Cronometro ----------------------------
                Console.WriteLine();
                Console.WriteLine();               
                var DuracionSeg = stopWath.ElapsedMilliseconds;// / 1000.0;// calcula tiempo rascurrido en Mili-segundos
                Console.WriteLine($"Tiempo Transcurrido en Mili-Segundos: {DuracionSeg}");
                Console.WriteLine(" ------------------------------------------ ");
                // ---------------------------- Cronometro ----------------------------

            }
            catch (Exception Ex)
            {
                throw;
            }

        }
    }
}
