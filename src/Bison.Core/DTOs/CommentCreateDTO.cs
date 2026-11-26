namespace Bison.DTOs;

using System.ComponentModel.DataAnnotations;

public class CommentCreateDTO
{
    [Required(ErrorMessage = "WRITE COMMENT!")]
    public string Text { get; set; }
}