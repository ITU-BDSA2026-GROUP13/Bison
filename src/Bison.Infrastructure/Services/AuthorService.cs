namespace Bison.Services;

using Razor.Services.Interfaces;

public class AuthorService : IAuthorService
{
    readonly IUnitOfWork unitOfWork;

    public AuthorService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
    
    public async Task<Author> GetAuthorById(string? authorId)
    {
        return await unitOfWork.authorRepo.GetAuthorById(authorId);
    }

}