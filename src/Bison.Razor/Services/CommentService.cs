namespace Bison.Services;

using Bison.DTOs;


using Interfaces;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork unitOfWork;
    
    
    public CommentService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<List<CommentDTO>> GetCommentsByObsId(long postId)
    {
        List<Comment> comments = await unitOfWork.commentRepo.GetCommentsByObsId(postId);

        return comments.Select(c => new CommentDTO
        {
            PostId = c.PostId,
            AuthorId = c.AuthorId,
            AuthorName = c.Author.Name,
            Text = c.Text,
            Timestamp = c.Timestamp.ToString("yyyy-MM-dd HH:mm:ss")
        }).ToList();
    }
}