namespace Bison;

using Repository;

public interface IUnitOfWork
{
    public IObservationRepository observationRepo { get; }
    public ICommentRepository commentRepo { get; }
    public IProposalRepository proposalRepo { get; }
    public ITaxonRepository taxonRepo { get; }
    
    public void Save();
}