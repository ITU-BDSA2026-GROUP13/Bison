namespace Bison.Razor.Services.Interfaces;

public interface IAuthorService
{
    public Task<Author> GetAuthorById(string? authorId);
}