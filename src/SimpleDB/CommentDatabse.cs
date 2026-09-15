namespace DefaultNamespace;

using SimpleDB;

public sealed class CommentDatabase<T> : CSVDatabase<T>
{
    private static CommentDatabase<T> instance = null;
    private static readonly object padlock = new object();
    public CommentDatabase(string pathToCsvFile) : base(pathToCsvFile)
    {
        
    }
    
    public CommentDatabase<T> Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new CommentDatabase<T>(pathToCsvFile);
                }
                return instance;
            }
        }
    }
} 