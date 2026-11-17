namespace Bison;

<<<<<<< HEAD
using Razor.Services.Interfaces;
using Repository;

=======
>>>>>>> bb8a28b (refac:)
public interface IUnitOfWork
{
    public IObservationRepository observationRepo { get; }
    public ICommentRepository commentRepo { get; }
    public IProposalRepository proposalRepo { get; }
    public ITaxonRepository taxonRepo { get; }
    public IAuthorRepository authorRepository { get;  }
   
    public void Save();
}