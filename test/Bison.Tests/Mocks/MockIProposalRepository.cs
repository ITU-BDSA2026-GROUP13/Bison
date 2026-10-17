namespace Bison.Tests.Mocks;

using Moq;
public class MockIProposalRepository
{
    public static Mock<IProposalRepository> GetMock()
    {
        // Observation used as a reffernce 
        var observation = new Observation
        {
            PostId = 1,
            Author = new Author { AuthorId = 69, Name = "Jacob", Email = "jcor@itu.dk" },
            Taxon = new Taxon { TaxonId = 420, dwc_TaxonID = "Anders", VernacularName = "Frederik", Parent = null },
            Text = "Jeg har set observation 1",
            Location = "Hos Lasse",
            TimeStamp = DateTime.Now
        };
        
        //Proposals to the above observation
        var proposals = new List<Proposal>
        { 
            new Proposal
            {
                PostId = 2,
                Author = new Author { AuthorId = 69, Name = "Jacob", Email = "jcor@itu.dk" },
                Text = "Det har jeg også proposal id2",
                TimeStamp = DateTime.Now,

                Taxon = new Taxon { TaxonId = 420, dwc_TaxonID = "Anders", VernacularName = "Frederik", Parent = null },
                Observation =  observation
            },

            new Proposal
            {
                PostId = 3,
                Author = new Author { AuthorId = 69, Name = "Jacob", Email = "jcor@itu.dk" },
                Text = "Det har jeg også proposal id3",
                TimeStamp = DateTime.Now,

                Taxon = new Taxon { TaxonId = 420, dwc_TaxonID = "Anders", VernacularName = "Frederik", Parent = null },
                Observation = observation
            },
        };

        var mock = new Mock<IProposalRepository>();
        mock.Setup(m => m.GetProposalsByObsId(It.IsAny<long>()))
            .ReturnsAsync((long id) => proposals.Where(p => observation.PostId == id).ToList());
        return mock;
    }
}