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
            optionsBuilder.UseMySql(dbUrl, serverVersion, options =>
            {
                /*** 加入暫時性失敗的重試機制，避免資料庫暫時中斷時拋出例外 ***/
                options.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null);
            });
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