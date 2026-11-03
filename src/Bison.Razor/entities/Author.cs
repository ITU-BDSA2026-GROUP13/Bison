namespace Bison;

using Microsoft.AspNetCore.Identity;

// Id, UserName and Email are inherited from IdentityUser
public class Author : IdentityUser
{
    public List<Post> Posts { get; set; } = new();
}
