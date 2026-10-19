using Microsoft.Playwright.Xunit;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace Bison.Tests.E2E;


public class PublicTimePageTest : PageTest
{
    private readonly string BaseUrl = "http://localhost:5273/obs";

    //Site Loader Test
    [Fact]
    public async Task PublicTimeline_Loads()
    {
        var response = await Page.GotoAsync(BaseUrl);


        Assert.Equal(200, response!.Status);

        await Expect(Page).ToHaveTitleAsync("Bison");
        await Expect(Page.Locator("h2")).ToContainTextAsync("Public Timeline");

    }

    [Fact]
    public async Task Author_Link_Goes_To_UserTimeline()
    {
        await Page.GotoAsync(BaseUrl);

        await Page.Locator("#messagelist li").First
            .GetByRole(AriaRole.Link, new() { Name = "Nathan Sirmon" })
            .ClickAsync();

        await Expect(Page).ToHaveURLAsync("http://localhost:5273/obs/4");
    }
    [Fact]
    public async Task Navigate_To_TaxonFilter_And_Back()
    {
        //Arrange
        await Page.GotoAsync(BaseUrl);
        var posts = Page.Locator("#messagelist");
        var taxonLink = posts.GetByRole(AriaRole.Link, new() { Name = "Skestork" }).First;
        var clearFilterLink = Page.GetByRole(AriaRole.Link, new() { Name = "(clear filter)" }).First;

        //Act
        await taxonLink.ClickAsync();

        //Assert
        await Expect(Page).ToHaveURLAsync(BaseUrl + "?taxonId=MSTSNM:Arter:f3fa2bf9-f785-ea11-aa77-501ac539d1ea");
        await Expect(clearFilterLink).ToBeVisibleAsync();

        await clearFilterLink.ClickAsync();
        await Expect(Page).ToHaveURLAsync(BaseUrl + "?");
    }

    [Fact]
    public async Task Pagination_Works_For_Obs()
    {
        //Arrange
        await Page.GotoAsync(BaseUrl);

        var pagination = Page.Locator(".pagination");
        await Expect(pagination.Locator("span")).ToHaveTextAsync("Page 1");

        var nextPageLink = Page.GetByRole(AriaRole.Link, new() { Name = "Next" }).First;
        await Expect(nextPageLink).ToBeVisibleAsync();

        //Act
        await nextPageLink.ClickAsync();

        //Assert
        await Expect(Page).ToHaveURLAsync(BaseUrl + "?taxonId=&page=2");

        await Expect(pagination.Locator("span")).ToHaveTextAsync("Page 2");

        var previousPageLink = Page.GetByRole(AriaRole.Link, new() { Name = "Previous" }).First;
        await Expect(previousPageLink).ToBeVisibleAsync();
    }
    


}