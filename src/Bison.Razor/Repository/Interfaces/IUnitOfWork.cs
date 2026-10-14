namespace Bison;

using Repository;

public interface IUnitOfWork
{
    public IObservationRepository observationRepo { get; }
    public ICommentRepository commentRepo { get; }
    public IProposalRepository proposalRepo { get; }
    
    public void Save();
}