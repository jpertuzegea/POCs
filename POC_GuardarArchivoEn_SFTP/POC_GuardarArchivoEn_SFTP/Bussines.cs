using Microsoft.AspNetCore.Http;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
                            if ((uint)Memory.Length > 32768)
                            {
                                client.BufferSize = (uint)Memory.Length; // El tamaño por defecto es 32768 bytes 
                            }

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


        // Metodo que guarda un archivo en servidor FTP que es diferente a SFTP, ya que son protocolos diferentes
        public bool FTPFileUpload()
        {
            string HostSFTP = "ftp://155.254.244.28/www.SIGECOR.somee.com";
            int PortSFTP = 21;
            string UserNameSFTP = "jpertuzegea";
            string PasswordSFTP = @"39590321JoRgE.";
            string DirectorySFTP = @"/Kathe/";

            string FileBase64 = @"U2Vydmlkb3IgQkQgRGVzYXJyb2xsbyAtLT4gMTAuMTAwLjEwMi4zNw0KVXN1YXJpbyAtLT4gZGF0YWJhc2VzDQpDbGF2ZSAtLT4gZGF0YWJhc2VzDQoNCg0KUG9ydGFsDQpodHRwOi8vMTAuMTAwLjEwMi4xMzE6ODA4My9Db2xzdWJzaWRpby9EZWZhdWx0LmFzcHg/dGFiaWQ9MzYNClVzdWFyaW86IGRubmhvc3QNCkNsYXZlOiBjb2xvbWJpYQ0KDQoNCmh0dHBzOi8vZXZlcnRlY2luY0BkZXYuYXp1cmUuY29tL2V2ZXJ0ZWNpbmMvUGljYXNzby9fZ2l0L1BpY2Fzc28=";
            string FileName = $"JorgePertuz_Prueba.txt";


            bool Respuesta = false;
            try
            {
                lock (ObjUpload)
                {
                    byte[] byteArray = Convert.FromBase64String(FileBase64);

                    var PathFull = HostSFTP + DirectorySFTP + FileName;
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(PathFull);

                    request.Method = WebRequestMethods.Ftp.UploadFile;

                    request.Credentials = new NetworkCredential(UserNameSFTP, PasswordSFTP);

                    Stream ftpStream = request.GetRequestStream();
                    ftpStream.Write(byteArray, 0, byteArray.Length);
                    ftpStream.Close();

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