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
        [TestMethod]
        public void CreateContact_AddNewContact()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationUserIdentityContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationUserIdentityContext(dbContextOptions);

            var logger = new Mock<IActivityLogger>();
            var userManager = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                null,null,null,null,null,null,null,null
                );

            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            var mockUser = new ApplicationUser { Id = "user123", NormalizedUserName = "ASHWIN", UserName = "ashwin" };

            userManager.Setup(u => u.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(mockUser);

            var mockHttpContext = new DefaultHttpContext();
            httpContextAccessor.Setup(h => h.HttpContext).Returns(mockHttpContext);

            var service = new ContactService(context, logger.Object, userManager.Object, httpContextAccessor.Object);


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
    }
}
