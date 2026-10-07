using Bison;

public record ObservationViewModel(long ObservationId, string Author, string Message, string Timestamp);

public interface IObservationService
{
    public List<ObservationViewModel> GetObservations(int page);
    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page = 1);
    public ObservationViewModel? GetObservationFromId(long? id);
}

public class ObservationService : IObservationService
{
    readonly DBFacade db;

    public ObservationService(DBFacade db)
    {
        this.db = db;
    }
    
    public List<ObservationViewModel> GetObservations(int page)
    {
        return db.getObservations(page);
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page = 1)
    {
        return db.getObservations(author, page);
    }

    public ObservationViewModel? GetObservationFromId(long? id)
    {
        return db.getObservationFromId(id);
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }


}
