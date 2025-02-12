using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using POC_BorradoMasivo.Entity;

namespace POC_BorradoMasivo
{
    public class ContextDB : DbContext
    {

        public ContextDB(DbContextOptions<ContextDB> options, IConfiguration configuration) : base(options)
        {

        }

        public ContextDB()
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
               "Server=localhost,1433;Database=PRUEBA;User Id=sa;Password=saphety123.;TrustServerCertificate=True",
                opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(2).TotalSeconds)
                );
        }
        public DbSet<Departament> Departament { get; set; }


    }
}
