using System;
using System.IO;
using System.IO.Compression;

namespace POC_CompromirArchivosEnZip
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""); ;
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("POC Comprimir archivos en .Zip");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("");


            try
            {
                string Path = @"D:\Publicaciones\Corresponsales\Admin";
                string PathZip = @"D:\Publicaciones\Corresponsales\Admin.zip";

                ZipFile.CreateFromDirectory(Path, PathZip);// Crea un archivo .zip a partir de una carpeta
                //ZipFile.ExtractToDirectory(PathZip, Path); // descomprime un archivo .zip en una directorio dado
                //File.Delete(PathZip);// Elimina un archivo de un directorio

            }
            catch (Exception Error)
            {
                Console.WriteLine("Error al Comprimir:" + Error.Message);
            }

            Console.WriteLine(""); ;
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Proceso Compresion Finalizado");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine(""); ;

            Console.ReadKey();

        }
    }
}
