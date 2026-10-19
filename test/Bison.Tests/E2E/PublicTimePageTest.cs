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
    public async Task Author_Link_Goes_Not_To_Wrong_UserTimeline()
    {
        await Page.GotoAsync(BaseUrl);

        await Page.Locator("#messagelist li").First
            .GetByRole(AriaRole.Link, new() { Name = "Nathan Sirmon" })
            .ClickAsync();

        await Expect(Page).ToHaveURLAsync("http://localhost:5273/obs/3");
    }
    [Fact]
    public async Task False_Author_Is_Not_Displayed()
    {
        //Arrange
        await Page.GotoAsync(BaseUrl);

        var posts = Page.Locator("#messagelist");
        await Expect(posts).ToBeVisibleAsync();
        //Act
        var fakeAuthorLink = posts.GetByRole(AriaRole.Link, new() { Name = "Lebron James", Exact = true });
        //Assert
        await Expect(fakeAuthorLink).ToHaveCountAsync(0);

        await Expect(posts).Not.ToContainTextAsync("Lebron James");
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
public async Task Unknown_TaxonId_Shows_Empty_Message()
{
    //Arrange
    var unknownTaxonUrl = BaseUrl + "?taxonId=DOES-NOT-EXIST";

    //Act
    var response = await Page.GotoAsync(unknownTaxonUrl);

    //Assert
    Assert.Equal(200, response!.Status);
    await Expect(Page.Locator("p").First).ToContainTextAsync("DOES-NOT-EXIST");
    await Expect(Page.Locator("#messagelist")).ToHaveCountAsync(0);
    await Expect(Page.Locator("em")).ToHaveTextAsync("There are no observations so far.");
}

[Fact]
public async Task ClearFilter_Link_Is_Not_Shown_Without_Filter()
{
    //Arrange
    var clearFilterLink = Page.GetByRole(AriaRole.Link, new() { Name = "(clear filter)" });

    //Act
    await Page.GotoAsync(BaseUrl);

    //Assert
    await Expect(Page.Locator("#messagelist")).ToBeVisibleAsync();
    await Expect(clearFilterLink).ToHaveCountAsync(0);
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
    
    [Fact]
public async Task Previous_Link_Is_Not_Shown_On_First_Page()
{
    //Arrange
    var pageIndicator = Page.Locator(".pagination span");
    var previousLink = Page.GetByRole(AriaRole.Link, new() { Name = "Previous" });

    //Act
    await Page.GotoAsync(BaseUrl);

    //Assert
    await Expect(pageIndicator).ToHaveTextAsync("Page 1");
    await Expect(previousLink).ToHaveCountAsync(0);
}

[Fact]
public async Task Page_Beyond_Last_Shows_Empty_Message_And_No_Next()
{
    //Arrange
    var pageIndicator = Page.Locator(".pagination span");
    var nextLink = Page.GetByRole(AriaRole.Link, new() { Name = "Next" });

    //Act
    var response = await Page.GotoAsync(BaseUrl + "?page=9999");

    //Assert
    Assert.Equal(200, response!.Status);
    await Expect(Page.Locator("em")).ToHaveTextAsync("There are no observations so far.");
    await Expect(pageIndicator).ToHaveTextAsync("Page 9999");
    await Expect(nextLink).ToHaveCountAsync(0);
}

[Theory]
[InlineData("0")]
[InlineData("-1")]
[InlineData("abc")]
public async Task Invalid_Page_Parameter_Does_Not_Crash(string page)
{
    //Arrange
    var url = BaseUrl + "?page=" + page;

    //Act
    var response = await Page.GotoAsync(url);

    //Assert
    Assert.NotNull(response);
    Assert.True(response.Status < 500, $"Got status {response.Status} for page={page}");
}


}