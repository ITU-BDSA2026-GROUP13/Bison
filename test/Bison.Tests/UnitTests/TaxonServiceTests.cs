namespace Bison.Tests.UnitTests;

using DTOs;
using Mocks;
using Moq;
using Services;

public class TaxonServiceTests
{
    private const string KnownId = "MSTSNM:Arter:c68811f4-f785-ea11-aa77-501ac539d1ea";
    private const string KnownName = "Tophejre";
    private const string IdWithoutName = "Anders";
    
    private TaxonService getService()
    {
        var unitOfWorkMock = MockIUnitOfWork.GetMock();
        return new TaxonService(unitOfWorkMock.Object);
    }
    
    // GetTaxonOptions
    [Fact]
    public async Task GetTaxonOptions_ReturnsTaxaWithVernacularName()
    {
        // Arrange
        var service = getService();

        // Act
        var result = await service.GetTaxonOptions();
        
        // Assert
        Assert.Single(result);
        Assert.Equal(KnownName, result.First().VernacularName);
    }

    [Fact]
    public async Task GetTaxonOptions_MapsIdAndNameCorrectly()
    {
        // Arrange
        var service = getService();

        // Act
        var result = await service.GetTaxonOptions();

        // Assert
        var option = Assert.Single(result);
        Assert.Equal(KnownId, option.dwc_TaxonId);
        Assert.Equal(KnownName, option.VernacularName);
    }
    
    [Fact]
    public async Task GetTaxonOptions_DoesNotIncludeTaxonWithoutVernacularName()
    {
        // Arrange
        var service = getService();

        // Act
        var result = await service.GetTaxonOptions();

        // Assert
        Assert.DoesNotContain(result, o => o.dwc_TaxonId == IdWithoutName);
    }
    
    // GetTaxonById
    
    [Fact]
    public async Task GetTaxonById_ReturnsTaxonWhenIdExists()
    {
        // Arrange
        var service = getService();
        
        // Act
        var result = await service.GetTaxonById(KnownId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(KnownName, result.VernacularName);
    }
    
    [Fact]
    public async Task GetTaxonById_ReturnsNullWhenIdDoesNotExist()
    {
        // Arrange
        var service = getService();
        
        // Act
        var result = await service.GetTaxonById("does-not-exist");

        Assert.Null(result);
    }
    
    // GetTaxonByVernacularName
    // TODO: Write tests when the method is implemented
    
    
    
    // GetALlTaxons
    [Fact]
    public async Task GetAllTaxons_ReturnsAllTaxaIncludingThoseWithoutName()
    {
        // Arrange
        var service = getService();
        
        // Act
        var result = await service.GetAllTaxons();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, t => t.dwc_TaxonID == KnownId);
        Assert.Contains(result, t => t.dwc_TaxonID == IdWithoutName);
    }
    
    
}