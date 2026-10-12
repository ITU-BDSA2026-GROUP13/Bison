namespace Bison;

public abstract class Post
{
    public long PostId { get; set; }
    public Author Author { get; set; }
    public string Text { get; set; }
    public DateTime TimeStamp { get; set; }
}