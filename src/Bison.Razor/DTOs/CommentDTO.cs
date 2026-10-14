namespace Bison.DTOs;

public record CommentDTO
    (
        long PostId,
        long AuthorId,
        string AuthorName,
        string Text,
        string TimeStamp
    );