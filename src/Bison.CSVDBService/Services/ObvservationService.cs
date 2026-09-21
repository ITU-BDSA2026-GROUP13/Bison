namespace Service;
using Bison;
using DefaultNamespace;

public class ObservationService
{
    // Creates DB
    private static readonly ObservationDatabase<Cheep> observations = ObservationDatabase<Cheep>.Instance;
    
    public static void addObservation(Cheep cheep)
    {
        observations.Store(cheep);
    }

    public static IEnumerable<Cheep> getObservations()
    {
       return observations.Read(); 
    } 
}

