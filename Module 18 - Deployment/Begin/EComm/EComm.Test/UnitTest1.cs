using EComm.Data;
using EComm.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace EComm.Test
{
    [TestClass]
    public class UnitTest1
    {
        private DataContext CreateMockDataContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;
            var context = new DataContext(options);

            context.Products.Add(new Product { Id = 1, ProductName = "Product 1", UnitPrice = 1, SupplierId = 1 });
            context.Products.Add(new Product { Id = 2, ProductName = "Product 2", UnitPrice = 2, SupplierId = 2 });
            context.Products.Add(new Product { Id = 3, ProductName = "Product 3", UnitPrice = 3, SupplierId = 3 });
            context.SaveChanges();

            return context;
        }

        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(4, (2 + 2));
        }

        [TestMethod]
        public void ProductControllerTest()
        {
            // Arrange
            var mockDataContext = CreateMockDataContext();
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<ProductController>();
            var controller = new ProductController(mockDataContext, logger);

            int id = 2;

            // Act
            var result = controller.Get(id);
            var product = mockDataContext.Products.SingleOrDefault(p => p.Id == id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(((OkObjectResult)result).StatusCode, 200);
            Assert.AreSame(((OkObjectResult)result).Value, product);
        }

    }
}
