namespace DefaultNamespace;

using SimpleDB;

public sealed class ObservationDatabase<T> : CSVDatabase<T>
{
    private static ObservationDatabase<T> instance = null;
    private static readonly object padlock = new object();
    public ObservationDatabase(string pathToCsvFile) : base(pathToCsvFile)
    {
        
    }
    
    public ObservationDatabase<T> Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new ObservationDatabase<T>(pathToCsvFile);
                }
                return instance;
            }
        }
    }
} 