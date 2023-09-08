using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileDemo.Models
{
    public class AppDBContext : DbContext
    {
        private readonly DbContextOptions _options;

        public AppDBContext() { }

        public AppDBContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Attachment> attachments { get; set; }

        public DbSet<Student> students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=User;Initial Catalog=TestDB02;Integrated Security=True");
            }
        }
    }
    
    
}
