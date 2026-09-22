namespace Bison;

using SimpleDB;

public static class CommentHandling {
    
    public static bool doesObservationExist(long cheepId, IEnumerable<Cheep> db) 
    {
        foreach (Cheep cheep in db)
        {
            if (cheep.CheepID == cheepId) return true;
        }

        return false;
    }
}