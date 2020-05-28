using System;

namespace POC_GuardarArchivoEn_SFTP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Prueba Guardado de Archivo en Base64 en SFTP");

            Bussines Bussines = new Bussines();

            if (Bussines.SFTPFileUpload())
            {
                Console.WriteLine("\n Archivo GUardado Con Exito");
            }
            else {
                Console.WriteLine("\n Archivo NO Guardado"); 
            }
            Console.ReadKey();
        }
    }
}
