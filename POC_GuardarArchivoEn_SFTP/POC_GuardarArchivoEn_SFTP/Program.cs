using System;

namespace POC_GuardarArchivoEn_SFTP
{
    class Program
    {
        static void Main(string[] args)
        {
            Bussines Bussines = new Bussines();
            Console.WriteLine("Prueba Guardado de Archivo en Base64 en SFTP");

            // *******************************************************
            // Method 1
            //Bussines.SFTPFileUploadIFormFile(File);
            // *******************************************************


            // *******************************************************
            // Method 2
            Bussines.FTPFileUpload();
            // *******************************************************


            // *******************************************************
            // Method 3
            if (Bussines.SFTPFileUploadBase64())
            {
                Console.WriteLine("\n Archivo GUardado Con Exito");
            }
            else
            {
                Console.WriteLine("\n Archivo NO Guardado");
            }
            // *******************************************************


            Console.ReadKey();
        }
    }
}
