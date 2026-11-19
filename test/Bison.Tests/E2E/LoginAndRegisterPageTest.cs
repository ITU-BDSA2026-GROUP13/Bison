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


    // Github redirection (external source)
    [Fact]
    public async Task TestGithubRedirectionThroughLogin()
    {
        // Arrange
        var basePage = await Page.GotoAsync(BaseUrl + "/Identity/Account/Login");
        var baseUrlHeading = Page.Locator("h1").GetByText("Log in");
        await Expect(baseUrlHeading).ToHaveTextAsync("Log in");

        var githubButton = Page.GetByRole(AriaRole.Button, new() { Name = "Github" }).First;

        // Act
        await githubButton.ClickAsync();

        // Assert
        var authenticationHeader = Page.Locator("div.authentication-header p strong").First; //HTML navigation
        await Expect(authenticationHeader).ToHaveTextAsync("GitHub"); 
    }

    [Fact]
    public async Task TestGithubRedirectionThroughRegister()
    {
         // Arrange
        var basePage = await Page.GotoAsync(BaseUrl + "/Identity/Account/Register");
        var baseUrlHeading = Page.Locator("h1").GetByText("Register");
        await Expect(baseUrlHeading).ToHaveTextAsync("Register");

        var githubButton = Page.GetByRole(AriaRole.Button, new() { Name = "Github" }).First;

        // Act
        await githubButton.ClickAsync();

        // Assert
        var authenticationHeader = Page.Locator("div.authentication-header p strong").First; //HTML navigation
        await Expect(authenticationHeader).ToHaveTextAsync("GitHub"); 
    }


    // Forgot password redirection (external source)
    [Fact]
    public async Task TestForgotPasswordReachesConfirmation()
    {
        // Arrange
        var basePage = await Page.GotoAsync(BaseUrl + "/Identity/Account/ForgotPassword");
        var baseUrlHeading = Page.Locator("h1").GetByText("Forgot your password?");
        await Expect(baseUrlHeading).ToHaveTextAsync("Forgot your password?");

        var forgotPasswordInput = Page.GetByLabel("Email");
        await forgotPasswordInput.FillAsync("Tester@mail.com");

        // Act
        var forgotPasswordSubmitButton = Page.GetByRole(AriaRole.Button, new() { Name = "Reset Password" }).First;
        await forgotPasswordSubmitButton.ClickAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(BaseUrl + "/Identity/Account/ForgotPasswordConfirmation");
        await Expect(Page.Locator("div.body p")).ToHaveTextAsync("Please check your email to reset your password.");
    }


    // Follow Through tests

    // DISCLAIMER: If we can interact directly with the database, we can test simple login and more, but for now, this is fine.

    [Fact]
    public async Task TestFullUseCaseOfRegisteringAndLoggingIn()
    {
        // Arrange
        var basePage = await Page.GotoAsync(BaseUrl + "/Obs");
        var baseUrlHeading = Page.Locator("h2"); 
        await Expect(baseUrlHeading).ToHaveTextAsync("Public Timeline"); //Status Checker

        string uniqueId = Guid.NewGuid().ToString("N")[..8]; // Generate a unique user every time.
        string testLoginUsername = $"TestUser_{uniqueId}";
        string testLoginEmail = $"testuser_{uniqueId}@example.com";
        string testLoginPassword = "TestPassword123!";

        var registerButton = Page.GetByRole(AriaRole.Link, new() { Name = "Register" }).First;

        // Act
        //Register a new test-user
        await registerButton.ClickAsync();
        await Expect(Page.Locator("h2")).ToHaveTextAsync("Create a new account."); //Status Checker

        var usernameInput = Page.GetByLabel("Username");
        var emailInput = Page.GetByLabel("Email");
        var passwordInput = Page.GetByLabel("Password").First;
        var confirmPasswordInput = Page.GetByLabel("Confirm password").First;
        await usernameInput.FillAsync(testLoginUsername);
        await emailInput.FillAsync(testLoginEmail);
        await passwordInput.FillAsync(testLoginPassword);
        await confirmPasswordInput.FillAsync(testLoginPassword);

        var registerSubmitButton = Page.Locator("div.row #registerSubmit");
        await registerSubmitButton.ClickAsync();
        await Expect(Page.Locator("div.body h1")).ToHaveTextAsync("Register confirmation"); //Status Checker

        var conformationButton = Page.Locator("#confirm-link");
        await conformationButton.ClickAsync();

        // Login with the newly registered test-user
        var loginButton = Page.Locator("div.body #login");
        await loginButton.ClickAsync();
        await Expect(Page.Locator("h2")).ToHaveTextAsync("Use a local account to log in."); //Status Checker

        var loginEmailInput = Page.GetByLabel("Email");
        var loginPasswordInput = Page.GetByLabel("Password");

        await loginEmailInput.FillAsync(testLoginEmail);
        await loginPasswordInput.FillAsync(testLoginPassword);

        var loginSubmitButton = Page.Locator("div.row #login-submit");
        await loginSubmitButton.ClickAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(BaseUrl + "/obs"); //Back at obs
        await Expect(Page.Locator("div.body #logout")).ToBeVisibleAsync(); //Logout button is visible
    }


    [Fact]
    public async Task TestLogoutWorksAfterLogin()
    {
        // Arrange
        // It is too complex to interact with the database directly, so I will have to register and login as done above. 

        var basePage = await Page.GotoAsync(BaseUrl + "/Identity/Account/Register");

        string uniqueId = Guid.NewGuid().ToString("N")[..8]; // Generate a unique user every time.
        string testLoginUsername = $"TestUser_{uniqueId}";
        string testLoginEmail = $"testuser_{uniqueId}@example.com";
        string testLoginPassword = "TestPassword123!";

        await Expect(Page.Locator("h2")).ToHaveTextAsync("Create a new account."); //Status Checker

        var usernameInput = Page.GetByLabel("Username");
        var emailInput = Page.GetByLabel("Email");
        var passwordInput = Page.GetByLabel("Password").First;
        var confirmPasswordInput = Page.GetByLabel("Confirm password").First;
        await usernameInput.FillAsync(testLoginUsername);
        await emailInput.FillAsync(testLoginEmail);
        await passwordInput.FillAsync(testLoginPassword);
        await confirmPasswordInput.FillAsync(testLoginPassword);

        var registerSubmitButton = Page.Locator("div.row #registerSubmit");
        await registerSubmitButton.ClickAsync();
        await Expect(Page.Locator("div.body h1")).ToHaveTextAsync("Register confirmation"); //Status Checker

        var conformationButton = Page.Locator("#confirm-link");
        await conformationButton.ClickAsync();

        // Login with the newly registered test-user
        var loginButton = Page.Locator("div.body #login");
        await loginButton.ClickAsync();
        await Expect(Page.Locator("h2")).ToHaveTextAsync("Use a local account to log in."); //Status Checker

        var loginEmailInput = Page.GetByLabel("Email");
        var loginPasswordInput = Page.GetByLabel("Password");

        await loginEmailInput.FillAsync(testLoginEmail);
        await loginPasswordInput.FillAsync(testLoginPassword);

        var loginSubmitButton = Page.Locator("div.row #login-submit");
        await loginSubmitButton.ClickAsync();

        await Expect(Page).ToHaveURLAsync(BaseUrl + "/obs"); //Back at obs
        await Expect(Page.Locator("div.body #logout")).ToBeVisibleAsync(); //Logout button is visible

        // Act
        var logoutButton = Page.Locator("div.body #logout");
        await logoutButton.ClickAsync();
        await Expect(Page).ToHaveURLAsync(BaseUrl + "/Identity/Account/Logout");

        var logoutSubmitButton = Page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).First;
        await logoutSubmitButton.ClickAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(BaseUrl + "/Identity/Account/Logout");
        await Expect(Page.Locator("div.body p")).ToHaveTextAsync("You have successfully logged out of the application.");
    }

}