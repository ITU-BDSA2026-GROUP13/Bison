namespace Bison;

public interface ICommentRepository
{
    Task CreateComment(Comment comment);
    
    Task<List<Comment>> ReadComment(long commentId);

    Task<List<Comment>> GetCommentsByObsId(long obsId);
    
    Task UpdateComment(Comment comment);
}