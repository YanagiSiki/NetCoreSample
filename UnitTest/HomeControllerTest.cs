using Coravel.Scheduling.Schedule.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetCoreSample.Controllers;
using NetCoreSample.Models;
using NUnit.Framework;
using Coravel;

namespace UnutTest.Controller
{
    /* 參考 http://www.zakwillis.com/post/2019/07/09/net-core-unit-testing-and-dependency-injection */
    
    public class HomeControllerTest
    {
        private ServiceProvider serviceProvider { get; set; }
        private HomeController _target { get; set; }


        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddScoped<BaseContext, UnitTestContext>();
            services.AddScheduler();
            serviceProvider = services.BuildServiceProvider();

            var dbContext = serviceProvider.GetService<BaseContext>();
            var scheduler = serviceProvider.GetService<IScheduler>();

            _target = new HomeController(dbContext, scheduler);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}