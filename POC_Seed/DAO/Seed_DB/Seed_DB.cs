using DAO.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Seed_DB
{

    /*
     Se debe modificar el program 
     Se debe inyectar la dependencia de Seed_DB
     */
    public class Seed_DB
    {
        public BD_Context BD;

        public Seed_DB()
        {
            BD = new BD_Context();
        }

        public async Task SeedAsync()
        {
            await BD.Database.EnsureCreatedAsync();
            await VerificarPeronas();
        }

        public async Task VerificarPeronas()
        {
            if (!BD.Personas.ToList().Any())
            { // Si no hay ningun elemento en la tabla Personas, lo crea

                BD.Personas.Add(new Personas() { Nombre = "Jorge David", Apellido = "Pertuz Egea" });
                await BD.SaveChangesAsync();
            }

        }
    }
}
