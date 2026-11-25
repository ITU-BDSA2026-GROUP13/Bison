namespace Bison.Repository;

using Microsoft.EntityFrameworkCore;

public class AuthorRepository : IAuthorRepository
{
    private readonly BisonDBContext dbContext;

    public AuthorRepository(BisonDBContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public async Task<Author> GetAuthorByID(string? authorId)
    {
        var authors = await dbContext.Authors.ToListAsync();
        return authors.FirstOrDefault(a => a.Id == authorId);
    }
        
}