using DAO.Entitys;
using DAO.Interfaces;
using DAO.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLL_Persona : DataAccess<Personas>// Todo el tema del CRUD se encuentra heradado de esta clase 
    {
        public async Task<bool> CrearPersona()
        { 
            Personas Persona = new Personas();
            Persona.Nombre = "Beatriz Elena";
            Persona.Apellido = "Pertuz Egea";

            return await CreateEntity(Persona);
        }
    }
}
