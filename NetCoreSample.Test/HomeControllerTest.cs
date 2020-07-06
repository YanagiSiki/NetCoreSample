using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetCoreSample.Controllers;
using NetCoreSample.Models;
using NSubstitute;
using NUnit.Framework;

namespace NetCoreSample.Test.Controller
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
            serviceProvider = services.BuildServiceProvider();

            var dbContext = serviceProvider.GetService<BaseContext>();
            _target = new HomeController(dbContext);
        }

        [Test]
        public void Test1()
        {
            // Arrange
            var user = new User { UserId = 1, Name = "Ya" };

            // Act
            var actual = _target.Tags();

            // Assert
            Assert.IsInstanceOf<IActionResult>(actual);
            // (ActionResult)actual

            Assert.AreEqual(1, 1);

        }
    }
}