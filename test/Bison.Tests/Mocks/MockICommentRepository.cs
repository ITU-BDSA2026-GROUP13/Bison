namespace Bison.Tests.Mocks;

using Moq;

public class MockICommentRepository
{

    public static Mock<ICommentRepository> GetMock()
    {

        
        var observation = new Observation // For referencing the same comment.
        {
            PostId = 6,
            Author = new Author { AuthorId = 1, Name = "Mikkel", Email = "msyj@itu.dk" },
            Taxon = new Taxon
            {
                TaxonId = 29, dwc_TaxonID = "MSTSNM:Arter:c68811f4-f785-ea11-aa77-501ac539d1ea", VernacularName = "Tophejre", Parent = null
            },
            Text = "Der en fugl",
            Location = "Der hjemme",
            TimeStamp = DateTime.Now
        };

        var comments = new List<Comment>
        {
            new Comment
            {
                PostId = 67,
                Author = new Author { AuthorId = 2, Name = "Lasse", Email = "LassesGoofyMail@itu.dk" },
                Text = "Der var virkelig en fugl",
                TimeStamp = DateTime.Now,
                Observation = observation
            },
            new Comment
            {
                PostId = 69,
                Author = new Author {AuthorId = 3, Name = "Lars", Email = "Lars@Lars.dk"},
                Text = "Bro. Crazy fugl",
                TimeStamp = DateTime.Now,
                Observation = observation
            }
        };
        var mock = new Mock<ICommentRepository>();
        mock.Setup(m => m.GetCommentsByObsId(It.IsAny<long>()))
            .ReturnsAsync((long id) => comments.Where(c => c.Observation.PostId == id).ToList());
        return mock;
    }
}