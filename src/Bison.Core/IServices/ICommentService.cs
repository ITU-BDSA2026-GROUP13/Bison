namespace Bison.Services.Interfaces;

using Bison.DTOs;

public interface ICommentService
{
    public Task<List<CommentDTO>> GetCommentsByObsId(long postId);
}