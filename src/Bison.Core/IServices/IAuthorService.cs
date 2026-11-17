namespace Bison.Razor.Services.Interfaces;

public interface IAuthorService
{
    public Task<Author> GetAuthorByID(string? authorId);
}