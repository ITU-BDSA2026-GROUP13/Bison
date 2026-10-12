namespace Bison;

public static class CommentHandling {
    
    public static bool doesObservationExist(long observationID, IEnumerable<Observation> db) 
    {
        foreach (Observation observation in db)
        {
            if (observation.PostId == observationID) return true;
        }

        return false;
    }
}