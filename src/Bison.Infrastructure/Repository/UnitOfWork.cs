namespace Bison.Repository;

using System.Transactions;

public class UnitOfWork : IUnitOfWork
{
    
    private readonly BisonDBContext dbContext;
    public IObservationRepository observationRepo { get; }
    public ICommentRepository commentRepo { get; }
    public IProposalRepository proposalRepo { get; }
    public ITaxonRepository taxonRepo { get; }
    public IAuthorRepository authorRepository { get; }
    
    public UnitOfWork(BisonDBContext dbContext)
    {
        this.dbContext = dbContext;
        
        this.observationRepo = new ObservationRepository(dbContext);
        this.commentRepo = new CommentRepository(dbContext);
        this.proposalRepo = new ProposalRepository(dbContext);
        this.taxonRepo = new TaxonRepository(dbContext);
        this.authorRepository = new AuthorRepository(dbContext);
    }

    public void Add(Observation observation)
    {
        dbContext.Observations.Add(observation);
    }
    public void Save()
    {
        dbContext.SaveChanges();
    }
    
}