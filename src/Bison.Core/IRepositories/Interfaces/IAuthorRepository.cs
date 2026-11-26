namespace Bison;

public interface IAuthorRepository
{ 
    public Task<Author> GetAuthorById(string? authorId);

}