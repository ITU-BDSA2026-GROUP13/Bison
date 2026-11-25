namespace Bison.Services;

using Bison.DTOs;
using Bison.Services.Interfaces;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork unitOfWork;
    
    
    public CommentService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task AddComment(long observationId, string authorId, Bison.DTOs.CommentCreateDTO newComment)
    {
        
        var obs = await unitOfWork.observationRepo.ReadObservation(observationId);
        //var author = await.unitOfWork......... (authorepo?)
        Comment commentToAdd = new Comment()
        {
            Text = newComment.Text,
            TimeStamp = DateTime.Now,     
            Observation = obs,
            //Author = author,
        };

        await unitOfWork.commentRepo.CreateComment(commentToAdd);
        unitOfWork.Save();
    }

    public async Task<List<CommentDTO>> GetCommentsByObsId(long postId)
    {
        List<Comment> comments = await unitOfWork.commentRepo.GetCommentsByObsId(postId);

        return comments.Select(c => new CommentDTO(
            c.PostId,
            c.Author.Id,
            c.Author.UserName ?? "",
            c.Text,
            c.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")
        )).ToList();
    }
}