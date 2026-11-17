namespace Bison.Repository;

using System.Transactions;

public class UnitOfWork : IUnitOfWork
{
    
    private readonly BisonDBContext dbContext;
    public IObservationRepository observationRepo { get; }
    public ICommentRepository commentRepo { get; }
    public IProposalRepository proposalRepo { get; }
    
    public UnitOfWork(BisonDBContext dbContext)
    {
        this.dbContext = dbContext;
        
        this.observationRepo = new ObservationRepository(dbContext);
        this.commentRepo = new CommentRepository(dbContext);
        this.proposalRepo = new ProposalRepository(dbContext);
    }

    public void Save()
    {
        dbContext.SaveChanges();
    }
    
}