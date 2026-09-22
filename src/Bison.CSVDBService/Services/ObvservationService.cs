namespace Service;
using Bison;
using DefaultNamespace;
using SimpleDB;

public class ObservationService
{
    // Creates DB
    private readonly IDatabaseRepository<Cheep> observations;
    
    public ObservationService(IDatabaseRepository<Cheep> observations)
    {
        this.observations = observations;
    }
    public void addObservation(Cheep cheep)
    {
        observations.Store(cheep);
    }

    public IEnumerable<Cheep> getObservations()
    {
       return observations.Read(); 
    } 
}

