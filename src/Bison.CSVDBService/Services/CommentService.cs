namespace Service;
using Bison;
using DefaultNamespace;

public class CommentService
{
    // Creates DBs
    private static readonly ObservationDatabase<Cheep> observations = ObservationDatabase<Cheep>.Instance;
    private static readonly CommentDatabase<Comment> comments = CommentDatabase<Comment>.Instance;
    
    public static void addComment(Comment comment)
    {
        if (!CommentHandling.doesObservationExist(comment.CheepID, observations)) throw new InvalidOperationException("Observation id does not exist");
        comments.Store(comment);
    }

    public static IEnumerable<Comment> getComments(long observationID)
    {
        return comments.Read().Where(c => c.CheepID == observationID);
    }
}

