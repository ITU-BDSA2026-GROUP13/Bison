namespace Bison.Tests.Mocks;

using Moq;

public class MockIObservationRepository
{
    public static Mock<IObservationRepository> GetMock()
    {
        var author1 = new Author { Id = "1", UserName = "Mikkel", Email = "msyj@itu.dk" };
        var author2 = new Author { Id = "69", UserName = "Jacob", Email = "jcor@itu.dk" };
        var taxon1 = new Taxon
        {
            TaxonId = 29,
            dwc_TaxonID = "MSTSNM:Arter:c68811f4-f785-ea11-aa77-501ac539d1ea",
            VernacularName = "Tophejre"
        };
        var taxon2 = new Taxon
        {
            TaxonId = 420,
            dwc_TaxonID = "Anders",
            VernacularName = null
        };

        var observation1 = new Observation
        {
            PostId = 8,
            Author = author1,
            Taxon = taxon1,
            Text = "Der en fugl",
            Location = "Der hjemme",
            TimeStamp = new DateTime(2026, 10, 16, 12, 0, 0)
        };
        var observation2 = new Observation
        {
            PostId = 67,
            Author = author2,
            Taxon = taxon2,
            Text = "Jeg har set noget Anderset",
            Location = "Hos Lasse",
            TimeStamp = new DateTime(2026, 10, 17, 12, 0, 0)
        };
        var observations = new List<Observation> { observation1, observation2 };
        var taxons = new List<Taxon> { taxon1, taxon2 };

        var mock = new Mock<IObservationRepository>();
        mock.Setup(m => m.GetAllObservations()).ReturnsAsync(observations);

        mock.Setup(m => m.GetAllObservationsByAuthor(It.IsAny<string>()))
            .ReturnsAsync((string authorId) => observations.Where(o => o.Author.Id == authorId).ToList());  

        mock.Setup(m => m.GetObservationsByPage(It.IsAny<int>()))
            .ReturnsAsync(observations);

        mock.Setup(m => m.GetObservationsByAuthorAndPage(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync((string authorId, int page) => observations.Where(o => o.Author.Id == authorId).ToList());

        mock.Setup(m => m.ReadObservation(It.IsAny<long>()))
            .ReturnsAsync((long id) => observations.FirstOrDefault(o => o.PostId == id));
        
        return mock;
    }
}
