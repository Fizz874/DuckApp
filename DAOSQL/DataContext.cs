using System.Text;
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
