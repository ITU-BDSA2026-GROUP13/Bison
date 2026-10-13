namespace Bison;

public interface ICommentRepository
{
    Task CreateComment(Comment comment);
    
    Task<List<Comment>> ReadComment(long commentId);
    
    Task UpdateComment(Comment comment);
}