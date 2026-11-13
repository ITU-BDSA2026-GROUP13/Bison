namespace Bison.DTOs;

public record CommentDTO
    (
        long PostId,
        string AuthorId,
        string AuthorName,
        string Text,
        string TimeStamp
    );