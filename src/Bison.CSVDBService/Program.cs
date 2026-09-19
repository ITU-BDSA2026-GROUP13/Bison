using Bison;
using SimpleDB;
using DefaultNamespace;



var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Creates DBs
var observations = ObservationDatabase<Cheep>.Instance;
var comments = CommentDatabase<Comment>.Instance;

// Queries
app.MapGet("/observations", () => observations.Read());
app.MapGet("/comments", (long id) => comments.Read().Where(c => c.CheepID == id));

// Commands
app.MapPost("/observation", (Cheep cheep) =>
{
    observations.Store(cheep);
    return Results.Ok();
});

app.MapPost("/comment", (Comment comment) =>
{
    if (!CommentHandling.doesObservationExist(comment.CheepID, observations))
        return Results.NotFound("Referenced observation does not exist");
    comments.Store(comment);
    return Results.Ok();
});

app.Run();