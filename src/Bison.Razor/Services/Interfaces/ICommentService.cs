namespace Bison.Services.Interfaces;

public interface ICommentService
{
    public Task<List<Comment>> GetCommentsByObsId(long postId);
}