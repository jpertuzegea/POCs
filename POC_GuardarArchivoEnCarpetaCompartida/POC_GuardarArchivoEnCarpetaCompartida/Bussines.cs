using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace POC_GuardarArchivoEnCarpetaCompartida
{
    public class Bussines
    {
        public static object ObjSave = new object();

        // Metodo para guardar archivo en carpeta comnpartida 
        public bool SaveFileInFoldershared()
        {
            string FileBase64 = @"U2Vydmlkb3IgQkQgRGVzYXJyb2xsbyAtLT4gMTAuMTAwLjEwMi4zNw0KVXN1YXJpbyAtLT4gZGF0YWJhc2VzDQpDbGF2ZSAtLT4gZGF0YWJhc2VzDQoNCg0KUG9ydGFsDQpodHRwOi8vMTAuMTAwLjEwMi4xMzE6ODA4My9Db2xzdWJzaWRpby9EZWZhdWx0LmFzcHg/dGFiaWQ9MzYNClVzdWFyaW86IGRubmhvc3QNCkNsYXZlOiBjb2xvbWJpYQ0KDQoNCmh0dHBzOi8vZXZlcnRlY2luY0BkZXYuYXp1cmUuY29tL2V2ZXJ0ZWNpbmMvUGljYXNzby9fZ2l0L1BpY2Fzc28=";
            string FileName = "JorgePertuzEgea.txt";

            try
            {
                lock (ObjSave)
                {
                    string targetPath = @"//192.168.99.135/HeadsCompartida/JoRgUeMunita/";
                    byte[] FileByteArray = Convert.FromBase64String(FileBase64);
                    File.WriteAllBytes(targetPath + FileName, FileByteArray);
                    return true;
                }
            }
            catch (Exception Error)
            {
                return false;
            }

        }
    }
}
