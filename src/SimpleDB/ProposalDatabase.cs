using DefaultNamespace;

namespace SimpleDB;

public class ProposalDatabase<T> : CSVDatabase<T>
{
    private static ProposalDatabase<T> instance = null;
    private static readonly object padlock = new object();
    private static readonly string pathToCsvFile = "../SimpleDB/bison_proposals_cli_db.csv"; 
    ProposalDatabase() : base(pathToCsvFile)
    {
        
    }
    
    public static ProposalDatabase<T> Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new ProposalDatabase<T>();
                }
                return instance;
            }
        }
    }
}