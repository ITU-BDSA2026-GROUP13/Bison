namespace Bison;

using SimpleDB;

public static class CommentHandling {
    
    public static bool doesObservationExist(long cheepId, IDatabaseRepository<Cheep> db) 
    {
        foreach (Cheep cheep in db.Read())
        {
            if (cheep.CheepID == cheepId) return true;
        }

        return false;
    }
}