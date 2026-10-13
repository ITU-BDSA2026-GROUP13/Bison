namespace Bison.Repository;

using Microsoft.EntityFrameworkCore;

public class CommentRepository : ICommentRepository
{
    
    private readonly BisonDBContext dbContext;

    public CommentRepository(BisonDBContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    
    public async Task CreateComment(Comment comment)
    {
        await dbContext.Posts.AddAsync(comment);
    }

    public async Task<List<Comment>> ReadComment(long commentId)
    {
        var query = from post in dbContext.Comments 
            where post.PostId == commentId
            select post;

        var response = await query.ToListAsync();
        return response;
    }

    public Task UpdateComment(Comment comment)
    {
        throw new NotImplementedException();
    }

}