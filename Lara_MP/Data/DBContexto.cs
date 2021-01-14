using Lara_MP.Model;
using Lara_MP.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lara_MP.Data
{
    class DBContexto : DbContext
    {
        public DbSet<Pessoa> MyProperty { get; set; }

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
    }
}
