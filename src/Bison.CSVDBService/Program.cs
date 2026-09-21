using Bison;
using SimpleDB;
using DefaultNamespace;
using Service;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


// Queries
app.MapGet("/observations", () => ObservationService.getObservations());
app.MapGet("/comments", (long id) => CommentService.getComments(id));

// Commands
app.MapPost("/observation", (Cheep cheep) =>
{
    ObservationService.addObservation(cheep);
    return Results.Ok();
});

app.MapPost("/comment", (Comment comment) =>
{
    try
    {
        CommentService.addComment(comment); 
        return Results.Ok();
    } catch (InvalidOperationException)
    {
        return Results.NotFound("Referenced observation does not exist");
    }
});

app.Run();