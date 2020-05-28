using System;

namespace POC_GuardarArchivoEnCarpetaCompartida
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Prueba Guardado de Archivo en Base64 en SFTP");

            Bussines Bussines = new Bussines();

            if (Bussines.SaveFileInFoldershared())
            {
                Console.WriteLine("\n Archivo Guardado Con Exito");
            }
            else
            {
                Console.WriteLine("\n Archivo NO Guardado");
            }

            Console.ReadKey();

        }
    }
}
