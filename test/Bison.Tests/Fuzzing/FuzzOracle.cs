public class FuzzOracle
{
    private readonly List<ObservationDto> _observations = new();
    private readonly List<CommentDto> _comments = new();
    
    //private readonly List<ProposalDto> _proposals = new();

    public void RecordObservation(ObservationDto o) => _observations.Add(o);
    public void RecordComment(CommentDto c) => _comments.Add(c);

    public bool Matches(IEnumerable<ObservationDto> serverResponse)
        => serverResponse.SequenceEqual(_observations, comparer);
}