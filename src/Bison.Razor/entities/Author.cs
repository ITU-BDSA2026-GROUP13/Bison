namespace Bison;

public class Author
{
    public long AuthorId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<Post> Posts { get; set; } = new();
}