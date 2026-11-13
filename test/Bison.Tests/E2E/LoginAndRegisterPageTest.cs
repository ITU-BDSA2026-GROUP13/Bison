namespace Bison.Tests.E2E;

using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;


public class LoginAndRegisterPageTest : PageTest
{
    private readonly string BaseUrl = "http://localhost:5273";


    // Short interaction tests
    [Fact]
    public async Task TestLoginButtonRedirectsToLogin()
    {
        // Arrange
        var basePage = await Page.GotoAsync(BaseUrl + "/Obs");
        var baseUrlHeading = Page.Locator("h2"); 
        await Expect(baseUrlHeading).ToHaveTextAsync("Public Timeline");

        var loginButton = Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).First;

        // Act
        await loginButton.ClickAsync();
    
        // Assert
        await Expect(Page).ToHaveURLAsync(BaseUrl + "/Identity/Account/Login");
    }

    [Fact]
    public async Task TestRegisterButtonRedirectsToRegister()
    {
        // Arrange
        var basePage = await Page.GotoAsync(BaseUrl + "/Obs");
        var baseUrlHeading = Page.Locator("h2"); 
        await Expect(baseUrlHeading).ToHaveTextAsync("Public Timeline");

        var registerButton = Page.GetByRole(AriaRole.Link, new() { Name = "Register" }).First;

        // Act
        await registerButton.ClickAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(BaseUrl + "/Identity/Account/Register");
    }

    [Fact]
    public async Task TestForgotPasswordButtonRedirectsToForgotPassword()
    {
        // Arrange
        var basePage = await Page.GotoAsync(BaseUrl + "/Identity/Account/Login");
        var baseUrlHeading = Page.Locator("h1").GetByText("Log in");
        await Expect(baseUrlHeading).ToHaveTextAsync("Log in");

        var forgotPasswordButton = Page.GetByRole(AriaRole.Link, new() { Name = "Forgot your password?" }).First;

        // Act
        await forgotPasswordButton.ClickAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(BaseUrl + "/Identity/Account/ForgotPassword");
    }

    [Fact]
    public async Task TestResendEmailConfirmationButtonRedirectsToResendEmailConfirmation()
    {
        // Arrange
        var basePage = await Page.GotoAsync(BaseUrl + "/Identity/Account/Login");
        var baseUrlHeading = Page.Locator("h1").GetByText("Log in");
        await Expect(baseUrlHeading).ToHaveTextAsync("Log in");

        var resendEmailConfirmationButton = Page.GetByRole(AriaRole.Link, new() { Name = "Resend email confirmation" }).First;

        // Act
        await resendEmailConfirmationButton.ClickAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(BaseUrl + "/Identity/Account/ResendEmailConfirmation");
    }

    [Fact]
    public async Task TestRegisterThroughLoginPageRedirectsToRegister()
    {
        // Arrange
        var basePage = await Page.GotoAsync(BaseUrl + "/Identity/Account/Login");
        var baseUrlHeading = Page.Locator("h1").GetByText("Log in");
        await Expect(baseUrlHeading).ToHaveTextAsync("Log in");

        var registerThroughLoginPageButton = Page.GetByRole(AriaRole.Link, new() { Name = "Register as a new user" }).First;

        // Act
        await registerThroughLoginPageButton.ClickAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(BaseUrl + "/Identity/Account/Register?returnUrl=%2Fobs");
    }



    // Follow Through tests

    [Fact]
    public async Task TestFullUseCaseOfRegisteringAndLoggingIn()
    {
        // TEST FAILS!! WRONG ACCESSING OF INPUT FIELDS I THINK
        
        // Arrange
        var basePage = await Page.GotoAsync(BaseUrl + "/Obs");
        var baseUrlHeading = Page.Locator("h2"); 
        await Expect(baseUrlHeading).ToHaveTextAsync("Public Timeline");

        var registerButton = Page.GetByRole(AriaRole.Link, new() { Name = "Register" }).First;

        // Act
        //Register a new test-user
        await registerButton.ClickAsync();

        var usernameInput = Page.GetByLabel("Username");
        var emailInput = Page.GetByLabel("Email");
        var passwordInput = Page.GetByLabel("Password");
        var confirmPasswordInput = Page.GetByLabel("Confirm password");

        await usernameInput.FillAsync("TestUser");
        await emailInput.FillAsync("testuser@example.com");
        await passwordInput.FillAsync("TestPassword123!");
        await confirmPasswordInput.FillAsync("TestPassword123!");

        var registerSubmitButton = Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).First;
        await registerSubmitButton.ClickAsync();

        var conformationButton = Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your account" }).First;
        await conformationButton.ClickAsync();

        // Login with the newly registered test-user
        var loginButton = Page.GetByRole(AriaRole.Link, new() { Name = "Log in" }).First;
        await loginButton.ClickAsync();

        var loginEmailInput = Page.GetByLabel("Email");
        var loginPasswordInput = Page.GetByLabel("Password");

        await loginEmailInput.FillAsync("testuser@example.com");
        await loginPasswordInput.FillAsync("TestPassword123!");

        var loginSubmitButton = Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).First;
        await loginSubmitButton.ClickAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(BaseUrl + "/Obs");
    }


}