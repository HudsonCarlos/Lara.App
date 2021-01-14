using Lara.Data.Mapping;
using Lara.Model;
using Lara.Util;
using Microsoft.EntityFrameworkCore;
using System;

namespace Lara.Data
{
    class DBContexto : DbContext
    {
        public DbSet<Pessoa> BpmPessoa { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                optionsBuilder.UseSqlite("Data Source=Teste.db");
                base.OnConfiguring(optionsBuilder);
            }
            catch (Exception ex)
            {
                Constantes.LogLara.EnfileirarLogErro("Erro em OnConfiguring", ex);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new Pessoa_Map());
        }
    }
}