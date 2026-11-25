namespace Bison;

public interface IAuthorRepository
{ 
    public Task<Author> GetAuthorByID(string? authorId);

}