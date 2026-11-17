namespace Bison;

using Razor.Services.Interfaces;
using Repository;

public interface IUnitOfWork
{
    public IObservationRepository observationRepo { get; }
    public ICommentRepository commentRepo { get; }
    public IProposalRepository proposalRepo { get; }
    public ITaxonRepository taxonRepo { get; }
    public IAuthorRepository authorRepository { get;  }
    
    public void Save();
}