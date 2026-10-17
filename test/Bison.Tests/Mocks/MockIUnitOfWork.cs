namespace Bison.Tests.Mocks;

using Moq;

public class MockIUnitOfWork
{
    public static Mock<IUnitOfWork> GetMock()
    {
        var mock = new Mock<IUnitOfWork>();

        // Create the repo mock once so every access returns the same instance
        var observationRepo = MockIObservationRepository.GetMock();
        mock.Setup(m => m.observationRepo).Returns(() => MockIObservationRepository.GetMock().Object);
        //mock.Setup(m => m.commentRepo).Returns(MockICommentRepository.GetMock().Object);
        //mock.Setup(m => m.proposalRepo).Returns(MockIProposalRepository.GetMock().Object);

        return mock;
    }
}