using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using CRMApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMApp.Tests.UnitTests
{
    [TestClass]
    public class ContactServiceUnitTest
    {
        // for context
        private DbContextOptions<ApplicationUserIdentityContext> GetContextOptions()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationUserIdentityContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return dbContextOptions;
        }

        //for usermanager
        private UserManager<ApplicationUser> GetUserManager()
        {
            var userManager = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                null, null, null, null, null, null, null, null
                );

            var mockUser = new ApplicationUser { Id = "user123", NormalizedUserName = "ASHWIN", UserName = "ashwin" };

            userManager.Setup(u => u.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(mockUser);
            return userManager.Object;
        }

        private ContactService CreateService(ApplicationUserIdentityContext context)
        {

            var logger = new Mock<IActivityLogger>();
            var userManager = GetUserManager();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();


            var mockHttpContext = new DefaultHttpContext();
            httpContextAccessor.Setup(h => h.HttpContext).Returns(mockHttpContext);

            var service = new ContactService(context, logger.Object, userManager, httpContextAccessor.Object);
            return service;
        }


        [TestMethod]
        public void CreateContact_AddNewContact()
        {
            var dbContextOptions = GetContextOptions();
            var context = new ApplicationUserIdentityContext(dbContextOptions);
            var service = CreateService(context);

            var contact = new CustomerContact
            {
                Id = 0,
                ContactName = "test Customer",
                Relation = Relation.Others,
                ContactType = Models.Type.Email,
                Contact = "test@Example.com",
                CustomerId = 1
            };

            var result = service.CreateContact(contact);

            Assert.IsTrue(result);
            Assert.AreEqual(1, context.CustomerContacts.Count());
        }

        [TestMethod]
        public void GetContacts_ReturnsContact()
        {
            var dbContextOptions = GetContextOptions();
            var context = new ApplicationUserIdentityContext(dbContextOptions);
            var service = CreateService(context);
            var contact = new CustomerContact
            {
                Id = 0,
                ContactName = "test Customer",
                Relation = Relation.Others,
                ContactType = Models.Type.Email,
                Contact = "test@Example.com",
                CustomerId = 1
            };

            service.CreateContact(contact);
            var result = service.GetContacts(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }
    }
}
