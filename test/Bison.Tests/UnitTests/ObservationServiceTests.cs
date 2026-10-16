namespace Bison.Tests.UnitTests;

using Bison.Services;
using Bison.Tests.Mocks;
using Moq;

public class ObservationServiceTests
{
    
    // Tests GetObservationById()
    [Fact]
    public async Task canReturnObservationWithValidID()
    {
        // Arrange
        var service = CreateService();
        long id = 67;
        
        // Act
        var result = await service.GetObservationById(id);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.ObservationId);
        Assert.Equal(69, result.AuthorId);
    }

    [Fact]
    public async Task returnsNullOnNonExistingID()
    {
        // Arrange
        var service = CreateService();
        long id = 68;
        
        // Act
        var result = await service.GetObservationById(id);
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetObservationById_TaxonWithoutDanishNameUsesDwcTaxonId()
    {
        // Arrange
        var service = CreateService();
        long id = 67;
        
        // Act
        var result = await service.GetObservationById(id);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Anders", result.TaxonName);
    }
    
    // Tests GetObservationByTaxonAndAuthorAndPage()

    [Fact]
    public async Task GetObservationByTaxonAndAuthorAndPage_ReturnsOneDtoPerObservation()
    {
        // Arrange
        var service = CreateService();
        
        // Act
        var result = await service.GetObservationByTaxonAndAuthorAndPage(null, 1, 1);
        
        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetObservationByTaxonAndAuthorAndPage_KeepsRepositoryOrder()
    {
        // Arrange
        var service = CreateService();
        
        // Act
        var result = await service.GetObservationByTaxonAndAuthorAndPage(null, 1, 1);

        // Assert
        Assert.Equal(new long[] {8, 67}, result.Select(o => o.ObservationId));
    }
    
    [Fact]
    public async Task GetObservationByTaxonAndAuthorAndPage_TaxonWithoutDanishName_UsesDwcTaxonId()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.GetObservationByTaxonAndAuthorAndPage(null, 1, 1);

        // Assert
        Assert.Equal("Anders", result[1].TaxonName);
    }
    
    
    // Tests GetObservationByTaxonAndPage
    [Fact]
    public async Task GetObservationByTaxonAndPage_ReturnsOneDtoPerObservation()
    {
        // Arrange
        var service = CreateService();
        
        // Act
        var result = await service.GetObservationByTaxonAndPage(null);
        
        // Assert
        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public async Task GetObservationByTaxonAndPage_KeepsRepositoryOrder()
    {
        // Arrange
        var service = CreateService();
        
        // Act
        var result = await service.GetObservationByTaxonAndPage(null);

        // Assert
        Assert.Equal(new long[] {8, 67}, result.Select(o => o.ObservationId));
    }

    [Fact]
    public async Task GetObservationByTaxonAndPage_MapsAllFields()
    {
        // Arrange
        var service = CreateService();
        
        // Act
        var result = await service.GetObservationByTaxonAndPage(null);

        // Assert
        var dto = result[0];
        Assert.Equal(8, dto.ObservationId);
        Assert.Equal(1, dto.AuthorId);
        Assert.Equal("Mikkel", dto.AuthorName);
        Assert.Equal("Der en fugl", dto.Text);
        Assert.Equal("MSTSNM:Arter:c68811f4-f785-ea11-aa77-501ac539d1ea", dto.dwc_TaxonId);
        Assert.Equal("Tophejre", dto.TaxonName);
        Assert.Equal("2026-10-16 12:00:00", dto.TimeStamp);
    }
    
    
    
    // Helper to create service
    private static ObservationService CreateService()
    {
        var unitOfWork = MockIUnitOfWork.GetMock();
        return new ObservationService(unitOfWork.Object);
    }
}