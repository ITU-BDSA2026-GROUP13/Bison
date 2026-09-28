namespace DefaultNamespace;

using SimpleDB;

public sealed class ObservationDatabase<Cheep> : CSVDatabase<Cheep>
{
    private static ObservationDatabase<Cheep> instance = null;
    private static readonly object padlock = new object();
    static readonly string pathToCsvFile =
    Environment.GetEnvironmentVariable("BISON_OBSERVATION_DB")
    ?? "../Bison.CLI/bison_observe_cli_db.csv"; // a change to Fuzzing

    ObservationDatabase() : base(pathToCsvFile)
    {
        
    }
    
    public static ObservationDatabase<Cheep> Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new ObservationDatabase<Cheep>();
                }
                return instance;
            }
        }
    }
}