namespace Bison.Tests.Mocks;

using Moq;

public class MockIUnitOfWork
{
    public static Mock<IUnitOfWork> GetMock()
    {
        var mock = new Mock<IUnitOfWork>();

        mock.Setup(m => m.observationRepo).Returns(() => MockIObservationRepository.GetMock().Object);
        mock.Setup(m => m.proposalRepo).Returns(() => MockIProposalRepository.GetMock().Object);
        
        mock.Setup(m => m.commentRepo).Returns(() => MockICommentRepository.GetMock().Object);

        return mock;
    }
}