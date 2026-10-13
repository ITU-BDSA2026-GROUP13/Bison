namespace Bison.Services;

using Interfaces;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork unitOfWork;
    
    
    public CommentService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<List<Comment>> GetCommentsByObsId(long postId)
    {
        return await unitOfWork.commentRepo.GetCommentsByObsId(postId);
    }
}