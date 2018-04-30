using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCoreSample.Helper;

namespace NetCoreSample.Models
{
    public class NpgsqlContext : DbContext
    {
        #region Migrations 說明
        //https://dotblogs.com.tw/wuanunet/2016/02/26/entity-framework-code-first-postgresql
        /*** Migrations資料夾的東西皆由Power shell產出  ***/
        /*** CLI 參考 https://docs.microsoft.com/zh-tw/ef/core/managing-schemas/migrations/  ***/
        //  Add-Migration InitialCreate
        //  Update-Database
        #endregion

        public DbSet<User> User { get; set; }
        public DbSet<InterviewExperience> InterviewExperience { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationHelper configurationHelper = new ConfigurationHelper("Postgresql");
            var dbUrl = configurationHelper.GetValue("connectionStrings:postgresql");

            if (dbUrl.isNullOrEmpty())
                dbUrl = Environment.GetEnvironmentVariable("dbUrl");
            Console.WriteLine(dbUrl);
            optionsBuilder.UseNpgsql(dbUrl);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Entity<InterviewExperience>()
                .HasOne(ie => ie.User)
                .WithMany(u => u.InterviewExperience);


            base.OnModelCreating(modelBuilder);
        }


    }
}
