using DAO.Entitys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public class BD_Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
            optionsBuilder.UseSqlServer(
              "Server=localhost;Database=POC_Seed_DB;Trusted_Connection=True;MultipleActiveResultSets=true",
              opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(2).TotalSeconds)
              );
             
        } 

        public DbSet<Personas> Personas { get; set; }

    }
}
