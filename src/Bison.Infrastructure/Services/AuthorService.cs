namespace Bison.Services;

using Razor.Services.Interfaces;

public class AuthorService : IAuthorService
{
    readonly IUnitOfWork unitOfWork;

    public AuthorService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
    
    public async Task<Author> GetAuthorByID(string? authorId)
    {
        return await unitOfWork.authorRepository.GetAuthorByID(authorId);
    }

}