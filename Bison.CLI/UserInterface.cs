public static class UserInterface
{
    public static void PrintCheeps(IEnumerable<Bison.Cheep> cheeps)
    {
        foreach (var cheep in cheeps)
        {
            var time = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp).ToLocalTime();
            Console.WriteLine($"{cheep.Author} @ {time:MM/dd/yy HH:mm:ss}: {cheep.Observation}");
        }
    }

    public static void PrintComments(IEnumerable<Bison.Comment> comments, long CheepID)
    {
        foreach (var comment in comments)
        {
            if (comment.CheepID == CheepID)
            {
                var time = DateTimeOffset.FromUnixTimeSeconds(comment.Timestamp).ToLocalTime();
                Console.WriteLine($"{comment.Author} @ {time:MM/dd/yy HH:mm:ss}: {comment.Message}");
            }
        }
    }
}