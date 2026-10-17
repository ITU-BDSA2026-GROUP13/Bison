namespace Bison.Tests.UnitTests;

using Bison.Repository;
using Bison.Tests.Mocks;
using DTOs;
using Moq;
using Services;

public class ProposalServiceTests
{
    [Fact]
    public async Task GetProposalsByObsId()
    {
        // Arrange
        var unitOfWork = MockIUnitOfWork.GetMock();
        var service = new ProposalService(unitOfWork.Object);
        long id = 1;
        // Act
        List<ProposalDTO> result = await service.GetProposalsByObsId(id);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result[0].PostId);
        Assert.Equal(3, result[1].PostId);
    }
}