using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using CRMApp.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CRMApp.Tests.UnitTests
{
    [TestClass]
    public class CustomerServiceUnitTest
    {
        [TestMethod]
        public void GetCustomers_ReturnAllCustomersmockSetApproach()
        {

            var options = new DbContextOptionsBuilder<ApplicationUserIdentityContext>().
                UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var customerData = new List<Customer>
        {
            new Customer { Id = 1, Name = "Ashwin" },
            new Customer { Id = 2, Name = "Tarun" }
        }.AsQueryable();


            var mockSet = new Mock<DbSet<Customer>>();
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(customerData.Provider);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(customerData.Expression);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(customerData.ElementType);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(customerData.GetEnumerator());



            var mockContext = new Mock<ApplicationUserIdentityContext>(options);
            mockContext.Setup(c => c.Customers).Returns(mockSet.Object);

            Mock<IActivityLogger> logger = new Mock<IActivityLogger>();
            Mock<IContactService> contactService = new Mock<IContactService>();


            var service = new CustomerService(mockContext.Object, logger.Object, contactService.Object);

            var result = service.GetCustomers();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Ashwin", result[0].Name);
            Assert.AreEqual("Tarun", result[1].Name);

        }


        [TestMethod]
        public void GetCustomers_ReturnAllCustomers()
        {
            var options = new DbContextOptionsBuilder<ApplicationUserIdentityContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationUserIdentityContext(options);
            var logger = new Mock<IActivityLogger>();
            var contactService = new Mock<IContactService>();

            context.Customers.AddRange(
                new Customer { Id = 1, Name = "Ashwin", Email = "Ashwin@example.com", Phone = "1234567890" },
                new Customer { Id = 2, Name = "Tarun", Email = "tarun@example.com", Phone = "1234567890" }
                );

            context.SaveChanges();

            var service = new CustomerService(context, logger.Object, contactService.Object);

            var result = service.GetCustomers();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Ashwin", result[0].Name);
            Assert.AreEqual("Tarun", result[1].Name);
            Assert.AreEqual("Ashwin@example.com", result[0].Email);
            Assert.AreEqual("tarun@example.com", result[1].Email);
        }
    }
}
