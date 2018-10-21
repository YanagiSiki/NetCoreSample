using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCoreSample.Helper;
using NetCoreSample.Tools;

namespace NetCoreSample.Models
{
    public class MySQLContext : BaseContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationHelper configurationHelper = new ConfigurationHelper("DBconnection");
            var dbUrl = configurationHelper.GetValue("MySQL");

            if (dbUrl.IsNullOrEmpty())
                dbUrl = Environment.GetEnvironmentVariable("dbUrl");
            Console.WriteLine(dbUrl);
            optionsBuilder.UseMySql(dbUrl);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PostTagConfiguration());

            base.OnModelCreating(modelBuilder);
        }

    }
}
