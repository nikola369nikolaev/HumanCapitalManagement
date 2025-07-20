using Microsoft.AspNetCore.Identity;
using Moq;


namespace HumanCapitalManagement.Tests.Services
{
    public class AccountServiceTests
    {
        [Fact]
        public async Task CreateUserWithRole_ShouldCreateUserAndAssignRole()
        {
            var email = "user@example.com";
            var password = "SecurePass123!";
            var role = "EMPLOYEE";

            var mockUserManager = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);

            mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), password))
                .ReturnsAsync(IdentityResult.Success);

            mockUserManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), role))
                .ReturnsAsync(IdentityResult.Success);

            var user = new IdentityUser { Email = email };

            var result = await mockUserManager.Object.CreateAsync(user, password);
            await mockUserManager.Object.AddToRoleAsync(user, role);

            Assert.True(result.Succeeded);
            mockUserManager.Verify(x => x.CreateAsync(It.IsAny<IdentityUser>(), password), Times.Once);
            mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), role), Times.Once);
        }
    }
}