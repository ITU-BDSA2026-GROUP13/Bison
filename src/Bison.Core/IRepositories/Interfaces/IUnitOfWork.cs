namespace Bison;

public interface IUnitOfWork
{
    public IObservationRepository observationRepo { get; }
    public ICommentRepository commentRepo { get; }
    public IProposalRepository proposalRepo { get; }
    public ITaxonRepository taxonRepo { get; }
    public IAuthorRepository authorRepo { get;  }
   
    public void Save();
}