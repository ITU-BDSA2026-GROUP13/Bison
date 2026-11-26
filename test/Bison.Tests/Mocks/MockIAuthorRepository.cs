namespace Bison.Tests.Mocks;

using Moq;

public class MockIAuthorRepository
{
    public static Mock<IAuthorRepository> GetMock()
    {
        var authors = new List<Author>
        {
            new Author()
            {
                Id = "1",
                UserName = "User1",
                Email = "User1@fakemail.com",
                PasswordHash = "1"

            },
            new Author()
            {
                Id = "2",
                UserName = "User2",
                Email = "User2@fakemail.com",
                PasswordHash = "2"

            },
            new Author()
            {
                Id = "3",
                UserName = "User3",
                Email = "User3@fakemail.com",
                PasswordHash = "3"

            }
        };

        var mock = new Mock<IAuthorRepository>();
        mock.Setup(m => m.GetAuthorById(It.IsAny<string>()))
            .ReturnsAsync((string? id) => authors.FirstOrDefault(a => a.Id == id));

        return mock;
    }
    
}