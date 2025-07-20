using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;
using System.Threading.Tasks;

public class AccountTests
{
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly Mock<SignInManager<IdentityUser>> _signInManagerMock;

    public AccountTests()
    {
        var store = new Mock<IUserStore<IdentityUser>>();
        _userManagerMock = new Mock<UserManager<IdentityUser>>(
            store.Object, null, null, null, null, null, null, null, null);

        var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
        _signInManagerMock = new Mock<SignInManager<IdentityUser>>(
            _userManagerMock.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);
    }

    [Fact]
    public async Task Register_CreatesUser_Successfully()
    {
        var user = new IdentityUser("user@example.com");
        _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), "Password123!"))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _userManagerMock.Object.CreateAsync(user, "Password123!");

        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task Login_WithCorrectCredentials_Succeeds()
    {
        _signInManagerMock.Setup(m => m.PasswordSignInAsync("user@example.com", "Password123!", false, false))
            .ReturnsAsync(SignInResult.Success);

        var result = await _signInManagerMock.Object.PasswordSignInAsync("user@example.com", "Password123!", false, false);
        
        Assert.True(result.Succeeded);
    }
}