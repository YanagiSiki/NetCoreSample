using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCoreSample.Helper;
using NetCoreSample.Tools;

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
        //  Remove-Migration
        #endregion

        #region dotnet ef cli
        //read https://github.com/aspnet/EntityFrameworkCore/issues/8996#issuecomment-326849252
        //  dotnet ef migrations add InitialCreate
        //  dotnet ef database update
        //  dotnet ef migrations remove
        #endregion

        public DbSet<User> User { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<PostTag> PostTag { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Comment> Comment { get; set; }

        // public DbSet<InterviewExperience> InterviewExperience { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationHelper configurationHelper = new ConfigurationHelper("Postgresql");
            var dbUrl = configurationHelper.GetValue("connectionStrings:postgresql");

            if (dbUrl.IsNullOrEmpty())
                dbUrl = Environment.GetEnvironmentVariable("dbUrl");
            Console.WriteLine(dbUrl);
            optionsBuilder.UseNpgsql(dbUrl);
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
