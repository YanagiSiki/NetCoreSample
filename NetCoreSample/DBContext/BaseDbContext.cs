using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCoreSample.Helper;
using NetCoreSample.Tools;

namespace NetCoreSample.Models
{
    public class BaseContext : DbContext
    {
        #region Migrations 說明
        //https://dotblogs.com.tw/wuanunet/2016/02/26/entity-framework-code-first-postgresql
        /*** Migrations資料夾的東西皆由Power shell產出  ***/
        /*** CLI 參考 https://docs.microsoft.com/zh-tw/ef/core/managing-schemas/migrations/  ***/
        //  Add-Migration InitialCreate
        //  Update-Database
        //  Remove-Migration        
        //  Add-Migration init -Context PartsDbContext
        //  Update-Database -Context PartsDbContext

        #endregion

        #region dotnet ef cli
        //read https://github.com/aspnet/EntityFrameworkCore/issues/8996#issuecomment-326849252
        //  dotnet ef migrations add InitialCreate
        //  dotnet ef database update
        //  dotnet ef migrations remove
        /*** 以下是使用多個Context ***/
        //      -s save path，預設就全部塞到Migrations吧
        //      --context 指定使用的Context
        //  dotnet ef migrations add InitialCreate -o MultipleDbContexts/ --context UsersDbContext
        //  dotnet ef database update --context UsersDbContext
        #endregion

        protected readonly ILoggerFactory _loggerFactory;

        public BaseContext(DbContextOptions options, ILoggerFactory loggerFactory) : base(options)
        {
            this._loggerFactory = loggerFactory;
        }

        public DbSet<User> User { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<PostTag> PostTag { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Comment> Comment { get; set; }

    }
}