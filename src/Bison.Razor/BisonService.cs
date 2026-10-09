using Bison;

public record ObservationViewModel(long ObservationId, string Author, string Message, string Timestamp);
public record CommentViewModel(long Comment_id, long Observation_id, string Author, string Message, string Timestamp);
public record ProposalViewModel(long Proposal_id, long Observation_id, string Author, string Taxon_id, string Timestamp);

public interface IObservationService    
{
    public List<ObservationViewModel> GetObservations(int page);
    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page = 1);
    public ObservationViewModel? GetObservationFromId(long? id);

}

public interface ICommentService
{
        public List<CommentViewModel> GetCommentViewModels(long? observationId);

}

public interface IProposalService
{
        public List<ProposalViewModel> GetProposalViewModel(long? proposalId);

}


public class ObservationService : IObservationService
{
    readonly DBFacade db;


    public ObservationService(DBFacade db)
    {
        this.db = db;
    }
    
    public List<ObservationViewModel> GetObservations(int page)
    {
        return db.getObservations(page);
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page = 1)
    {
        return db.getObservations(author, page);
    }

    public ObservationViewModel? GetObservationFromId(long? id)
    {
        return db.getObservationFromId(id);
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }


}

public class CommentService : ICommentService
{
     readonly DBFacade db;

    public CommentService(DBFacade db)
    {
        this.db = db;

    }
    public List<CommentViewModel> GetCommentViewModels(long? observationId)
    {
        return db.GetCommentViewModels(observationId);
    }
    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
    
}

public class ProposalService : IProposalService
{
     readonly DBFacade db;
    readonly Taxonomy tax;

    public ProposalService(DBFacade db, Taxonomy tax)
    {
        this.db = db;
        this.tax = tax;
    }
    public List<ProposalViewModel> GetProposalViewModel(long? proposalId)
    {
        return db.GetProposalViewModels(proposalId).Select(p =>
            {
                var name = tax.GetTaxonById(p.Taxon_id)?.VernacularName;
                return p with { Taxon_id = string.IsNullOrWhiteSpace(name) ? p.Taxon_id : name };
            })
            .ToList();;
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }


}
