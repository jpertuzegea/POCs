using Models.Process.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace POC_Queue.Models
{
    public class Persona : IInputModel, IOutputModel
    {
        public int cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; } 
        public string ModelId { get; set; }
    }
}
