using Microsoft.AspNetCore.Http;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace POC_GuardarArchivoEn_SFTP
{
    public class Bussines
    {

        public static object ObjUpload = new object();

        string HostSFTP = "192.168.60.100";
        int PortSFTP = 22;
        string UserNameSFTP = "dbeft";
        string PasswordSFTP = @"843795213@db";
        string DirectorySFTP = @"/EFT/Cargues/ExitoClienteFTP/";
        string FileBase64 = @"U2Vydmlkb3IgQkQgRGVzYXJyb2xsbyAtLT4gMTAuMTAwLjEwMi4zNw0KVXN1YXJpbyAtLT4gZGF0YWJhc2VzDQpDbGF2ZSAtLT4gZGF0YWJhc2VzDQoNCg0KUG9ydGFsDQpodHRwOi8vMTAuMTAwLjEwMi4xMzE6ODA4My9Db2xzdWJzaWRpby9EZWZhdWx0LmFzcHg/dGFiaWQ9MzYNClVzdWFyaW86IGRubmhvc3QNCkNsYXZlOiBjb2xvbWJpYQ0KDQoNCmh0dHBzOi8vZXZlcnRlY2luY0BkZXYuYXp1cmUuY29tL2V2ZXJ0ZWNpbmMvUGljYXNzby9fZ2l0L1BpY2Fzc28=";
        string FileName = $"JorgePertuz_Prueba_{DateTime.Now.Date}.txt";

        // Metodo para guardar archivo en servidor (s)FTP desde un archivo en Base64
        public bool SFTPFileUploadBase64()
        {
            bool Respuesta = false;
            try
            {
                lock (ObjUpload)
                {
                    byte[] File = Convert.FromBase64String(FileBase64);

                    var client = new SftpClient(HostSFTP, PortSFTP, UserNameSFTP, PasswordSFTP);
                    client.Connect();

                    if (client.IsConnected)
                    {
                        using (var Memory = new MemoryStream(File))
                        {
                            client.BufferSize = (uint)Memory.Length;
                            client.UploadFile(Memory, DirectorySFTP + FileName, false); // el ultimo parametro es para sobre escribir el arhivo si existe 
                            Respuesta = true;
                        }
                    }
                    else
                    {
                        Respuesta = false;
                    }
                }
                return Respuesta;
            }
            catch (Exception Error)
            {
                Console.WriteLine("\nError --> " + Error.ToString());
                return false;
            } 
        }


        // Metodo para guardar archivo en servidor (s)FTP desde un IFormFile desde el controlador 
        //  con relacion al ejemplo anterior solo cabia esta linea using (var Memory = File.OpenReadStream())
        public bool SFTPFileUploadIFormFile(IFormFile File)
        {
            bool Respuesta = false;
            try
            {
                lock (ObjUpload)
                { 
                    var client = new SftpClient(HostSFTP, PortSFTP, UserNameSFTP, PasswordSFTP);
                    client.Connect();

                    if (client.IsConnected)
                    {
                        using (var Memory = File.OpenReadStream())
                        {
                            client.BufferSize = (uint)Memory.Length;
                            client.UploadFile(Memory, DirectorySFTP + FileName, false); // el ultimo parametro es para sobre escribir el arhivo si existe 
                            Respuesta = true;
                        }
                    }
                    else
                    {
                        Respuesta = false;
                    }
                }
                return Respuesta;
            }
            catch (Exception Error)
            {
                Console.WriteLine("\nError --> " + Error.ToString());
                return false;
            } 
        }


    }
}
