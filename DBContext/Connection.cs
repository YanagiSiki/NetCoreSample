using System.Linq;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace NetCoreSample.Models
{
    public class Connection
    {

        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection("input your connect string");
        }
        public void test()
        {
            using(var tmp = new TestContext()){
                var aa = tmp.User.Where(_ => _.Name == "Ye").ToList();
                aa.ForEach((_)=>{
                    _.Name = $"{_.Name}A";
                });
            };
            
        }

    }

    public class TestContext : DbContext
    {
        public DbSet<User> User { get; set; }
        /* https://bambit.ch/blog/aspnet-core-mit-postgresql-aus-heroku */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(new Connection().CreateConnection());
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }
}