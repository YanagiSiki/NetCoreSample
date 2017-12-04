using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCoreSample.Helper;

namespace NetCoreSample.Models
{
    public class MSSQLDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationHelper configurationHelper = new ConfigurationHelper();
            Console.WriteLine(configurationHelper.GetValue("connectionStrings:MSSQLdb"));
            optionsBuilder.UseSqlServer(configurationHelper.GetValue("connectionStrings:MSSQLdb"));
        }
        public DbSet<User> User { get; set; }
    }
}
