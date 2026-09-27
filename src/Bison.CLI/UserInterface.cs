using Spectre.Console;
using System.Globalization;

public static class UserInterface
{
    public static void PrintCheeps(IEnumerable<Bison.Cheep> cheeps)
    {
        Console.WriteLine("Cheeps:");
        foreach (var cheep in cheeps)
        {
            var time = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp).ToLocalTime();
            string formattedTime = time.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            Console.WriteLine($"{cheep.Author} @ {formattedTime}: {cheep.Observation} at {cheep.Location}");
            // InvariantCulture takes care of Windows and Linux formatting date separators differently.
        }
    }

    public static void PrintComments(IEnumerable<Bison.Comment> comments, long CheepID)
    {
        foreach (var comment in comments)
        {
            if (comment.CheepID == CheepID)
            {
                var time = DateTimeOffset.FromUnixTimeSeconds(comment.Timestamp).ToLocalTime();
                string formattedTime = time.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                Console.WriteLine($"{comment.Author} @ {formattedTime}: {comment.Message}");
            }
        }
    }

    public static void PrintProposals(IEnumerable<Bison.Proposal> proposals, long CheepID)
    {
        foreach (var proposal in proposals)
        {
            if (proposal.CheepID == CheepID)
            {
                var time = DateTimeOffset.FromUnixTimeSeconds(proposal.Timestamp).ToLocalTime();
                string formattedTime = time.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                Console.WriteLine($"{proposal.Author} @ {formattedTime}: {proposal.TaxonID}");
            }
        }
    }
}