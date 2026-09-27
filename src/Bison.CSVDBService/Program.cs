using Bison;
using SimpleDB;
using DefaultNamespace;
using Service;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IDatabaseRepository<Cheep>>(
    ObservationDatabase<Cheep>.Instance
);

builder.Services.AddSingleton<IDatabaseRepository<Comment>>(
    CommentDatabase<Comment>.Instance
);

builder.Services.AddSingleton<ObservationService>();
builder.Services.AddSingleton<CommentService>();


var app = builder.Build();


// Queries
app.MapGet("/observations",
    (ObservationService service) =>
        service.getObservations());

app.MapGet("/comments",
    (long id, CommentService service) =>
        service.getComments(id));

app.MapPost("/observation",
    (Cheep cheep, ObservationService service) =>
{
    service.addObservation(cheep);
    return Results.Ok();
});

app.MapPost("/comment",
    (Comment comment, CommentService service) =>
{
    try
    {
        service.addComment(comment);
        return Results.Ok();
    }
    catch (InvalidOperationException)
    {
        return Results.NotFound("Referenced observation does not exist");
    }
});

app.Run();