namespace DefaultNamespace;

using SimpleDB;

public sealed class CommentDatabase<T> : CSVDatabase<T>
{
    private static CommentDatabase<T> instance = null;
    private static readonly object padlock = new object();
    private static readonly string pathToCsvFile = "../Bison.CLI/bison_comments_cli_db.csv"; 
    CommentDatabase() : base(pathToCsvFile)
    {
        
    }
    
    public static CommentDatabase<T> Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new CommentDatabase<T>();
                }
                return instance;
            }
        }
    }
} 
