using Bison;

public record ObservationViewModel(string Author, string Message, string Timestamp);

public interface IObservationService
{
    public List<ObservationViewModel> GetObservations(int page);
    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page = 1);
}

public class ObservationService : IObservationService
{
    readonly DBFacade db;

    public ObservationService(DBFacade db)
    {
        this.db = db;
    }
    // These would normally be loaded from a database for example
    private static readonly List<ObservationViewModel> _obs = new()
        {
            new ObservationViewModel("Peter", "I saw a heron", UnixTimeStampToDateTimeString(1690892208)),
            new ObservationViewModel("Paul", "There is a bison on Amager", UnixTimeStampToDateTimeString(1690895308)),
        };

    public List<ObservationViewModel> GetObservations(int page)
    {
        return db.getObservations(page);
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page = 1)
    {
        return db.getObservations(author, page);
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }


}
