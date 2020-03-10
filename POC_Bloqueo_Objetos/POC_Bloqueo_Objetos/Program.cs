
using System;
using System.Diagnostics;
using System.Threading;

namespace POC_Bloqueo_Objetos
{
    class Program
    {

        /*
         * .Net Core 3.1 - VisualvStudio 2019
         * Esta prueba de concepto permite implementar el lock de un objeto estatico,
         * con el fin de eliminar los problemas presentados al intentar varios  acceder a un objeto que se encuentra en uso por otro hilo dentro del mismo proceso
         * permitiendo asi, el control de escritura o lectura de uno en uno.
         */

        private static Object MiObjetc = new object();// Se instancia un objeto estatico que es el que va a controlar los accesos al proceso a implementar


        static void Main(string[] args)
        {
            Console.WriteLine(" ------------------------------------------ ");
            Console.WriteLine("Inicio POC");
            Console.WriteLine(" ------------------------------------------ ");


            var stopWath = new Stopwatch();// Instancia Cronometro
            stopWath.Start(); // Inica Cronometro 

            lock (MiObjetc)
            {// aca se realiza el bloqueo del objeto, de tal forma que solo se pueda procesar de uno en uno
             // aca se realiza el proceso requerido garantizando asi el buen funcionamiento en paralelismos o escrituras simultaneas (dentro del mismo proceso)
                Thread.Sleep(1000);
                Console.WriteLine("Escritura en Bloqueo realizada ");
            }

            stopWath.Stop();// Finaliza Cronometro

            // var DuracionSeg = stopWath.ElapsedMilliseconds / 1000.0;// calcula tiempo rascurrido en segundos
            var DuracionSeg = stopWath.ElapsedMilliseconds;// / 1000.0;// calcula tiempo rascurrido en Mili-segundos

            Console.WriteLine("Fin POC");
            Console.WriteLine(" ------------------------------------------ ");
            Console.WriteLine($"Tiempo Transcurrido en Mili-Segundos: {DuracionSeg}");
            Console.WriteLine(" ------------------------------------------ ");


            Console.ReadKey();
        }
    }
}
