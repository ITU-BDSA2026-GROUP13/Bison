using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace Bison.Tests.E2E;

public class UserTimelinePageTest : PageTest
{
    private readonly string BaseUrl = "http://localhost:5273";


    [Fact]
    public async Task Can_Go_To_UserTimelinePage_Through_URL()
    {
        await Page.GotoAsync(BaseUrl + "/obs/3");

        var heading = Page.Locator("h2").First;


        await Expect(Page).ToHaveURLAsync(BaseUrl + "/obs/3");
        await Expect(heading).ToBeVisibleAsync();
        await Expect(heading).ToHaveTextAsync("Wendell Ballan Observations");
    }

    [Fact]
    public async Task Taxon_Link_Redirects_To_Correct_TaxonTimeline()
    {
        //Arrange
        await Page.GotoAsync(BaseUrl + "/obs/3");

        var taxonRedirectLink =  Page.GetByRole(AriaRole.Link, new() {Name = "- Rørdrum"}).First;
        await Expect(taxonRedirectLink).ToBeVisibleAsync();

        //Act
        await taxonRedirectLink.ClickAsync();

        //Assert
        await Expect(Page).ToHaveURLAsync(BaseUrl + "/obs/3?taxonId=MSTSNM:Arter:2d9011f4-f785-ea11-aa77-501ac539d1ea");

        var paragraph = Page.Locator("p").First;
        await Expect(paragraph).ToBeVisibleAsync();
        await Expect(paragraph).ToHaveTextAsync("Showing observations of MSTSNM:Arter:2d9011f4-f785-ea11-aa77-501ac539d1ea (clear filter)");
    }

    [Fact]
    public async Task Author_Link_ON_TaxonTimeline_Redirects_To_Correct_UserTimeline()
    {
        //Arrange
        await Page.GotoAsync(BaseUrl + "/obs/3?taxonId=MSTSNM:Arter:2d9011f4-f785-ea11-aa77-501ac539d1ea");

        var authorRedirectLink =  Page.GetByRole(AriaRole.Link, new() {Name = "Wendell Ballan"}).First;
        await Expect(authorRedirectLink).ToBeVisibleAsync();

        //Act
        await authorRedirectLink.ClickAsync();

        //Assert
        await Expect(Page).ToHaveURLAsync(BaseUrl + "/obs/3");

        var heading = Page.Locator("h2").First;
        await Expect(heading).ToBeVisibleAsync();
        await Expect(heading).ToHaveTextAsync("Wendell Ballan Observations");
    }

    [Fact]
    public async Task ClearFilter_Link_On_TaxonTimeline_Redirects_To_Correct_UserTimeline()
    {
        //Arrange
        await Page.GotoAsync(BaseUrl + "/obs/3?taxonId=MSTSNM:Arter:2d9011f4-f785-ea11-aa77-501ac539d1ea");

        var clearFilterLink =  Page.GetByRole(AriaRole.Link, new() {Name = "(clear filter)"}).First;
        await Expect(clearFilterLink).ToBeVisibleAsync();

        //Act
        await clearFilterLink.ClickAsync();

        //Assert
        await Expect(Page).ToHaveURLAsync(BaseUrl + "/obs/3?");

        var heading = Page.Locator("h2").First;
        await Expect(heading).ToBeVisibleAsync();
        await Expect(heading).ToHaveTextAsync("Wendell Ballan Observations");
    }

    [Fact]
    public async Task Does_Display_Observation()
    {
        await Page.GotoAsync(BaseUrl + "/obs/3");

        var observations = Page.Locator("#messagelist");
        await Expect(observations.Locator("li")).ToHaveCountAsync(32);

        var userRedirectLink =  Page.GetByRole(AriaRole.Link, new() {Name = "Wendell Ballan"}).First;

        await Expect(userRedirectLink).ToHaveAttributeAsync("href", "3");
        await Expect(Page.GetByText("Four bitterns resting near the fjord.")).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Pagination_Works()
    {
        //Arrange
        await Page.GotoAsync(BaseUrl + "/obs/3");

        var pagination = Page.Locator(".pagination");
        await Expect(pagination.Locator("span")).ToHaveTextAsync("Page 1");

        var nextPageLink =  Page.GetByRole(AriaRole.Link, new() {Name = "Next"}).First;
        await Expect(nextPageLink).ToBeVisibleAsync();

        //Act
        await nextPageLink.ClickAsync();

        await Expect(Page).ToHaveURLAsync(BaseUrl + "/obs/3?taxonId=&page=2");

        var previousPageLink =  Page.GetByRole(AriaRole.Link, new() {Name = "Previous"}).First;
        await Expect(previousPageLink).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Are_Observations_Different_For_Pages()
    {
        await Page.GotoAsync(BaseUrl + "/obs/3");

        var firstObservationLocator = Page.Locator("#messagelist").Locator("small").First;
        var firstPageTimestamp = await firstObservationLocator.InnerTextAsync();

        var nextPageLink = Page.GetByRole(AriaRole.Link, new() { Name = "Next" });
        await Expect(nextPageLink).ToBeVisibleAsync();
        await nextPageLink.ClickAsync();

        var secondPageObservationLocator = Page.Locator("#messagelist").Locator("small").First;
        var secondPageTimestamp = await secondPageObservationLocator.InnerTextAsync();

        Assert.NotEqual(secondPageTimestamp, firstPageTimestamp);
    }

    [Fact]
    public async Task UserTimeline_Author_With_No_Timeline()
    {
        var response = await Page.GotoAsync(BaseUrl + "/obs/9999999999");

        Assert.NotNull(response);
        Assert.Equal(200, response.Status);

        var missingObservationsText = Page.Locator("em").First;

        await Expect(missingObservationsText).ToBeVisibleAsync();
        await Expect(missingObservationsText).ToHaveTextAsync("There are no Observations so far.");
    }













        
}