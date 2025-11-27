using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTERFACES;
using Microsoft.EntityFrameworkCore;

namespace Strzelecki_Baranowski.DuckApp.DAO
{
    internal class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data source=ducks.db");
        }
        public virtual DbSet<Duck> Ducks { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }

    }

}
