using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace Bison.Tests.E2E;

public class ObservationDetailPageTest : PageTest
{
    private readonly string BaseUrl = "http://localhost:5273";


    [Fact]
    public async Task Can_Go_To_ObservationDetailPage_Through_URL()
    {
        await Page.GotoAsync(BaseUrl + "/ob/3");

        var heading = Page.Locator("h2");


        await Expect(Page).ToHaveURLAsync(BaseUrl + "/ob/3");
        await Expect(heading).ToBeVisibleAsync();
        await Expect(heading).ToHaveTextAsync("Observation");
    }

    [Fact]
    public async Task Author_Link_Redirects_To_Correct_UserTimeline()
    {
        //Arrange
        await Page.GotoAsync(BaseUrl + "/ob/3");

        var userRedirectLink =  Page.GetByRole(AriaRole.Link, new() {Name = "Wendell Ballan"}).First;
        await Expect(userRedirectLink).ToBeVisibleAsync();

        //Act
        await userRedirectLink.ClickAsync();

        //Assert
        await Expect(Page).ToHaveURLAsync(BaseUrl + "/obs/3");

        var heading = Page.Locator("h2");
        await Expect(heading).ToBeVisibleAsync();
        await Expect(heading).ToHaveTextAsync("Wendell Ballan Observations");
    }

    [Fact]
    public async Task Does_Display_Observation()
    {
        await Page.GotoAsync(BaseUrl + "/ob/3");

        var userRedirectLink =  Page.GetByRole(AriaRole.Link, new() {Name = "Wendell Ballan"}).First;

        await Expect(userRedirectLink).ToHaveAttributeAsync("href", "/obs/3");
        await Expect(Page.GetByText("Large white heron in a wet meadow. Yellow bill, black legs.")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Does_Display_Proposals()
    {
        await Page.GotoAsync(BaseUrl + "/ob/3");

        var proposals = Page.Locator("#proposalList");
        await Expect(proposals).ToBeVisibleAsync();
        await Expect(proposals.Locator("li")).ToHaveCountAsync(2);
        await Expect(proposals.GetByText("Silkehejre")).ToBeVisibleAsync();
        await Expect(proposals.GetByText("Sølvhejre")).ToBeVisibleAsync();
        await Expect(proposals.GetByRole(AriaRole.Link, new() { Name = "Roger Histand" })).ToHaveAttributeAsync("href", "/obs/1");
        await Expect(proposals.GetByRole(AriaRole.Link, new() { Name = "Nathan Sirmon" })).ToHaveAttributeAsync("href", "/obs/4");
    }

    [Fact]
    public async Task Does_Display_Comments()
    {
        await Page.GotoAsync(BaseUrl + "/ob/3");

        var comments = Page.Locator("#commentList");
        await Expect(comments).ToBeVisibleAsync();
        await Expect(comments.Locator("li")).ToHaveCountAsync(2);
        await Expect(comments.GetByText("Do you have a photo? Size is hard to judge at distance.")).ToBeVisibleAsync();
        await Expect(comments.GetByText("Only a blurry one. It looked bigger than a grey heron.")).ToBeVisibleAsync();
        await Expect(comments.GetByRole(AriaRole.Link, new() { Name = "Roger Histand" })).ToHaveAttributeAsync("href", "/obs/1");
        await Expect(comments.GetByRole(AriaRole.Link, new() { Name = "Wendell Ballan" })).ToHaveAttributeAsync("href", "/obs/3");
    }

    [Fact]
    public async Task Does_Display_Missing_Text_On_Proposals()
    {
        await Page.GotoAsync(BaseUrl + "/ob/4");

        var missing_proposal_text = Page.Locator("em").First;
        await Expect(missing_proposal_text).ToHaveTextAsync("There are no proposals so far.");
    }

    [Fact]
    public async Task Does_Display_Missing_Text_On_Comments()
    {
        await Page.GotoAsync(BaseUrl + "/ob/4");

        var missing_proposal_text = Page.Locator("em").Nth(1);
        await Expect(missing_proposal_text).ToHaveTextAsync("There are no comments so far.");
    }

    [Fact]
    public async Task DetailPage_With_Invalid_ObservationId()
    {
        var response = await Page.GotoAsync(BaseUrl + "/ob/9999999999");

        Assert.NotNull(response);
        Assert.Equal(404, response.Status);
    }

    [Fact]
    public async Task Missing_ObservationId_Should_Redirect_To_PublicPage()
    {
        await Page.GotoAsync(BaseUrl + "/ob");

        await Expect(Page).ToHaveURLAsync(BaseUrl + "/obs");
        
    }



}
