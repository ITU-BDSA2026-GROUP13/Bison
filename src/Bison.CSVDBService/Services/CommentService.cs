namespace Service;

using Bison;
using DefaultNamespace;
using SimpleDB;

public class CommentService
{
    // Creates DBs
    private readonly IDatabaseRepository<Cheep> observations;
    private readonly IDatabaseRepository<Comment> comments;
    
    public CommentService(IDatabaseRepository<Comment> comments, IDatabaseRepository<Cheep> observations)
    {
        this.comments = comments;
        this.observations = observations;
    }

    public void addComment(Comment comment)
    {
        if (!CommentHandling.doesObservationExist(comment.CheepID, observations.Read())) throw new InvalidOperationException("Observation id does not exist");
        comments.Store(comment);
    }

    public IEnumerable<Comment> getComments(long observationID)
    {
        return comments.Read().Where(c => c.CheepID == observationID);
    }
}

