namespace Bison.FuzzTests;

using Bison;

public class ProposalGenerator
{
    private readonly Random ran;
    private readonly Func<long> pickCheepId;
    private readonly IReadOnlyList<string> knownTaxonIds;

    public ProposalGenerator(Random ran, Func<long> pickCheepId, Taxonomy taxonomy)
    {
        this.ran = ran;
        this.pickCheepId = pickCheepId;
        knownTaxonIds = taxonomy.AllTaxons.Select(t => t.TaxonID).ToList();
    }

    // ~90% valid taxon IDs, the rest random, so error handling is exercised too
    public string PickTaxonId() =>
        knownTaxonIds.Count > 0 && ran.NextDouble() < 0.9
            ? knownTaxonIds[ran.Next(knownTaxonIds.Count)]
            : $"MSTSNM:Arter:{Guid.NewGuid()}";

    public Proposal GenerateRandom() => new Proposal(pickCheepId(), PickTaxonId());

    // Keeps CheepID fixed and only varies the TaxonID
    public Proposal Mutate(Proposal proposal) => new Proposal(proposal.CheepID, PickTaxonId());
}