namespace Bison.Services.Interfaces;

using Bison.DTOs;

public interface ICommentService
{
    public Task AddComment(long observationId, string authorId, Bison.DTOs.CommentCreateDTO newComment);
    public Task<List<CommentDTO>> GetCommentsByObsId(long postId);
    public Task<List<CommentDTO>> GetCommentsByAuthor(string authorId);

}