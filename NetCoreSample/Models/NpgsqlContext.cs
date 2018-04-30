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

        public NpgsqlContext() {
            Database.EnsureCreated();   // Create the database
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationHelper configurationHelper = new ConfigurationHelper();
            var dbUrl = configurationHelper.GetValue("connectionStrings:postgresql");
            Console.WriteLine(dbUrl);
            if (dbUrl.isNullOrEmpty())
                dbUrl = Environment.GetEnvironmentVariable("dbUrl");
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
