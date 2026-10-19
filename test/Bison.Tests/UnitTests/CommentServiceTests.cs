namespace Bison.Tests.UnitTests;

using Bison.Services;
using Bison.Tests.Mocks;
using Moq;

public class CommentServiceTests
{
    
    [Fact]
    public async Task canReturnCommentsFromObsId()
    {
        // Arrange
        var unitOfWork = MockIUnitOfWork.GetMock();
        var service = new CommentService(unitOfWork.Object);
        long obsId = 6;
        
        // Act
        var result = service.GetCommentsByObsId(obsId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Der var virkelig en fugl", result.Result.First().Text);
    }
    
    [Fact]
    public async Task returnsEmptyOnNonExistingObsID()
    {
        // Arrange
        var unitOfWork = MockIUnitOfWork.GetMock();
        var service = new CommentService(unitOfWork.Object);
        long obsId = 68;
        
        // Act
        var result = await service.GetCommentsByObsId(obsId);
        
        // Assert
        Assert.Empty(result);
    }
    
}