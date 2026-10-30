namespace Bison.Tests.Mocks;

using Moq;

public class MockIObservationRepository
{
    public static Mock<IObservationRepository> GetMock()
    {
        var observations = new List<Observation>
        {
            new Observation
            {
                PostId = 8,
                Author = new Author { Id = "1", UserName = "Mikkel", Email = "msyj@itu.dk" },
                Taxon = new Taxon
                    { TaxonId = 29, dwc_TaxonID = "MSTSNM:Arter:c68811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Tophejre", Parent = null },
                Text = "Der en fugl",
                Location = "Der hjemme",
                TimeStamp = new DateTime(2026, 10, 16, 12, 00, 00)
            },
            new Observation
            {
                PostId = 67,
                Author = new Author {Id = "69", UserName = "Jacob", Email = "jcor@itu.dk"},
                Taxon = new Taxon { TaxonId = 420, dwc_TaxonID = "Anders", VernacularName = null, Parent = null},
                Text = "Jeg har set noget Anderset",
                Location = "Hos Lasse",
                TimeStamp = DateTime.Now
            }
        };
        var mock = new Mock<IObservationRepository>();
        mock.Setup(m => m.ReadObservation(It.IsAny<long>()))
            .ReturnsAsync((long id) => observations.FirstOrDefault(o => o.PostId == id));
        
        mock.Setup(m => m.GetObservationByTaxonAndPage(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(observations);
        
        mock.Setup(m => m.GetObservationByTaxonAndAuthorAndPage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(observations);
        
        return mock;
    }
}