using Microsoft.EntityFrameworkCore;
using MVC03.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MVC03.DAL.Data.Contexts
{
    public class CompanyDbContext : DbContext
    {

        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options) 
        {
            

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server= .; Database = CompanyMVC03; Trusted_Connection = True; TrustServerCertificate= True");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet <Department> Departments { get; set; }

    }
}
