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
    public class HerokuNpgContext : BaseContext
    {
        /* https://bambit.ch/blog/aspnet-core-mit-postgresql-aus-heroku */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationHelper configurationHelper = new ConfigurationHelper("DBconnection");
            var dbUrl = configurationHelper.GetValue("HerokuNpg");

            if (dbUrl.IsNullOrEmpty())
                dbUrl = Environment.GetEnvironmentVariable("dbUrl");
            Console.WriteLine(dbUrl);
            if (dbUrl.IsNullOrEmpty()) throw new Exception("資料庫連線失敗");
            optionsBuilder.UseNpgsql(dbUrl);
            var loggerFactory = new LoggerFactory();
            optionsBuilder.UseLoggerFactory(loggerFactory);
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