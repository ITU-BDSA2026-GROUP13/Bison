public static class UserInterface
{
    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        foreach (var cheep in cheeps)
        {
            var time = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp).ToLocalTime();
            Console.WriteLine($"{cheep.Author} @ {time:MM/dd/yy HH:mm:ss} {cheep.Observation}");
        }
    }
}