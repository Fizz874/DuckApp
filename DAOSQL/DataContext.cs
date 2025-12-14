using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Strzelecki_Baranowski.DuckApp.INTERFACES;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Strzelecki_Baranowski.DuckApp.DAO
{
    internal class DataContext : DbContext
    {

        public virtual DbSet<Duck> Ducks { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlite("Data source=ducks.db");
            //if (!optionsBuilder.IsConfigured)
            //{
            //    string folderPath = AppDomain.CurrentDomain.BaseDirectory;

            //    string fullPath = System.IO.Path.Combine(folderPath, "ducks.db");

            //    optionsBuilder.UseSqlite($"Data Source={fullPath}");
            //}

            string dllPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            string? dllDirectory = System.IO.Path.GetDirectoryName(dllPath);

            string dbPath = System.IO.Path.Combine(dllDirectory ?? "", "ducks.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Duck>()
                .HasOne(d => d.Producer)
                .WithMany()
                .HasForeignKey(d => d.ProducerID)
                .OnDelete(DeleteBehavior.Cascade);
        }


        public void SeedData()
        {
            if (Producers.Any())
            {
                return; 
            }


            string dllPath = Assembly.GetExecutingAssembly().Location;
            string? dllDirectory = Path.GetDirectoryName(dllPath);
            string path = Path.Combine(dllDirectory ?? "", "seed.sql");


            if (!File.Exists(path)) return;

            string sql = File.ReadAllText(path, Encoding.UTF8);

            this.Database.ExecuteSqlRaw(sql);
        }


    }

}
