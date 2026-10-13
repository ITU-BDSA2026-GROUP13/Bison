namespace Bison.Services.Interfaces;

public interface ICommentService
{
    public List<Comment> GetCommentsById(long? postId);
}