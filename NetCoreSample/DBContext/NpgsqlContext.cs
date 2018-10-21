using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCoreSample.Helper;
using NetCoreSample.Tools;

namespace NetCoreSample.Models
{
    public class NpgsqlContext : BaseContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationHelper configurationHelper = new ConfigurationHelper("DBconnection");
            var dbUrl = configurationHelper.GetValue("Postgresql");

            if (dbUrl.IsNullOrEmpty())
                dbUrl = Environment.GetEnvironmentVariable("dbUrl");
            Console.WriteLine(dbUrl);
            optionsBuilder.UseNpgsql(dbUrl);
            optionsBuilder.UseLoggerFactory(MyLogger.MyLoggerFactory);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");            
            modelBuilder.ApplyConfiguration(new PostTagConfiguration());

            base.OnModelCreating(modelBuilder);
        }

    }
}
