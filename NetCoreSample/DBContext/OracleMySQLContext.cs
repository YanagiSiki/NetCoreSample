using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using NetCoreSample.Helper;
using NetCoreSample.Tools;

namespace NetCoreSample.Models
{

    public class OracleMySQLContext : BaseContext
    {
        public OracleMySQLContext(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationHelper configurationHelper = new ConfigurationHelper("DBconnection");
            var dbUrl = configurationHelper.GetValue("OracleMySQL");

            if (dbUrl.IsNullOrEmpty())
                dbUrl = Environment.GetEnvironmentVariable("dbUrl");
            Console.WriteLine(dbUrl);
            if (dbUrl.IsNullOrEmpty()) throw new Exception("資料庫連線失敗");
            var serverVersion = new MariaDbServerVersion(new Version(10, 3, 23));
            optionsBuilder.UseMySql(dbUrl, serverVersion);
            /*** https://ohke.hateblo.jp/entry/2017/03/03/000000 ***/
            var loggerFactory = new LoggerFactory();
            optionsBuilder.UseLoggerFactory(loggerFactory);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PostTagConfiguration());

            base.OnModelCreating(modelBuilder);
        }

    }
}