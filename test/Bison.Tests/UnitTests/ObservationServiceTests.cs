namespace Bison.Tests.UnitTests;

using Bison.Services;
using Bison.Tests.Mocks;
using Moq;

public class ObservationServiceTests
{
    [Fact]
    public async Task canReturnObservationWithValidID()
    {
        // Arrange
        var unitOfWork = MockIUnitOfWork.GetMock();
        var service = new ObservationService(unitOfWork.Object);
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
        var unitOfWork = MockIUnitOfWork.GetMock();
        var service = new ObservationService(unitOfWork.Object);
        long id = 68;
        
        // Act
        var result = await service.GetObservationById(id);
        
        // Assert
        Assert.Null(result);
    }
}