using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace POC_Ejecutar_Funciones_En_Paralelo
{
    class Program
    {
        /*
       * Creado po: Jorge David Pertuz Egea 10 Marzo 2020
       * .Net Core 3.1 - VisualvStudio 2019
       * 
       * Esta prueba de concepto permite implementar la ejecucion de varios metodos en forma paralela,
       * y luego esperar el resultado cuando todas se hayan ejecutado (el tiempo total es el que demore la mas lenta),
       * sin importar los parametros de entrada ni el tipo de variable de retorno.
       * 
       * Tambien se puede ejecutar otro proceso una vez ejecutado el hilo principal (dependencia de procesos),
       * utilizando la instruccion .ContinueWith( () => { Mi otro proceso } 
       */

        public static Stopwatch stopWath = new Stopwatch();// Instancia Cronometro



        static void Main(string[] args)
        {
            Console.WriteLine(" ------------------------------------------ ");
            Console.WriteLine("Inicio POC");
            Console.WriteLine(" ------------------------------------------ \n ");

            EjecutarPruebaSincrona();
            EjecutarPruebaAsincrona();

            Console.WriteLine("\n ------------------------------------------ ");
            Console.WriteLine("Fin POC");
            Console.WriteLine(" ------------------------------------------ ");

            /*
            ****************************************
             OTRAS FORMAS DE PARALELISMO
            ****************************************
            _____________________________________________________________
              MiLista.AsParallel()// probado con exito !!!
                .WithDegreeOfParallelism(10) // Establecemos el número de hilos que ejecutarán las tareas en paralelo
                .ForAll(x => // Ejecuta el bloque de codigo siguiente para cada uno de los items del la lista 
                {  
                // codigo a ejecutar
                });
            _____________________________________________________________

            _____________________________________________________________
            // Lista de IDs a guardar
            List<int> ids = new List<int> { 1, 2, 3, 4, 5 };
            // Crear una lista de tareas
            List<Task> tasks = new List<Task>();
            // Crear una tarea para cada ID
            foreach (var id in ids)
            {
                tasks.Add(Task.Run(() => Guardar(id)));
            }
            // Esperar que todas las tareas terminen
            await Task.WhenAll(tasks);
            _____________________________________________________________
             */
        }


        private static void EjecutarPruebaSincrona()
        {
            Console.WriteLine("Ejecucion Prueba Sincrona");

            stopWath.Start(); // Inica Cronometro 

            var metodo1 = Metodo1(1);
            var metodo2 = Metodo2("Jorge Pertuz");
            var metodo3 = Metodo3();

            Console.WriteLine("Se Ejecuto Metodo Sincrono " + metodo1);
            Console.WriteLine("Se Ejecuto Metodo Sincrono " + metodo2);
            Console.WriteLine("Se Ejecuto Metodo Sincrono " + metodo3);

            var DuracionSeg = stopWath.ElapsedMilliseconds;// / 1000.0;// calcula tiempo rascurrido en Mili-segundos
            Console.WriteLine($"Tiempo Transcurrido en Mili-Segundos: {DuracionSeg}");
            stopWath.Stop();// Finaliza Cronometro
            Console.WriteLine(" ------------------------------------------ ");
        }
        // Metodos Sincronos
        public static int Metodo1(int num)
        {
            Thread.Sleep(1000);
            return 1;
        }
        public static string Metodo2(string data)
        {
            Thread.Sleep(1000);
            return "2";
        }
        public static long Metodo3()
        {
            Thread.Sleep(1000);
            // Console.WriteLine("Se Ejecuto el metodo #3");
            return 3;
        }






        private static void EjecutarPruebaAsincrona()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Ejecucion Prueba Asincrona");

            stopWath.Restart(); // Re-Inica Cronometro  
            var Tarea1 = MetodoAsincrono1(1);
            var Tarea2 = MetodoAsincrono2("Jorge Pertuz");
            var Tarea3 = MetodoAsincrono3();

            Task.WhenAll(Tarea1, Tarea2);// .ContinueWith( () => { Mi otro proceso } // para ejecutar dependencia de proceos 

            Console.WriteLine("Se Ejecuto Metodo Asincrono " + Tarea1.Result);
            Console.WriteLine("Se Ejecuto Metodo Asincrono " + Tarea2.Result);
            Console.WriteLine("Se Ejecuto Metodo Asincrono " + Tarea3.Result);

            stopWath.Stop();// Finaliza Cronometro

            var DuracionSeg = stopWath.ElapsedMilliseconds;// / 1000.0;// calcula tiempo rascurrido en Mili-segundos
            Console.WriteLine($"Tiempo Transcurrido en Mili-Segundos: {DuracionSeg}");
            Console.WriteLine(" ------------------------------------------ ");
        }

        // Metodos Asincronos 
        public static async Task<int> MetodoAsincrono1(int num)
        {
            return await Task.Run(() =>
             {
                 Thread.Sleep(1000);
                 return 1;
             }); // .ContinueWith( () => { Mi otro proceso } // para ejecutar dependencia de proceos 
        }
        public static async Task<string> MetodoAsincrono2(string data)
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(1000);
                return "2";
            }); // .ContinueWith( () => { Mi otro proceso } // para ejecutar dependencia de proceos 
        }
        public static async Task<long> MetodoAsincrono3()
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(1000);
                return 3;
            }); // .ContinueWith( () => { Mi otro proceso } // para ejecutar dependencia de proceos 
        }

    }
}
