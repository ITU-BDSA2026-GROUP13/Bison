using Bison;
using SimpleDB;
using DefaultNamespace;
using Service;

var builder = WebApplication.CreateBuilder(args);


//Makes sure that the file is a new one everytime
builder.Services.AddSingleton<IDatabaseRepository<Cheep>>(_ =>
    new CSVDatabase<Cheep>(
        Environment.GetEnvironmentVariable("BISON_OBSERVATION_DB") ?? "observations.csv"));

builder.Services.AddSingleton<IDatabaseRepository<Comment>>(_ =>
    new CSVDatabase<Comment>(
        Environment.GetEnvironmentVariable("BISON_COMMENT_DB") ?? "comments.csv"));

builder.Services.AddSingleton<IDatabaseRepository<Proposal>>(_ =>
    new CSVDatabase<Proposal>(
        Environment.GetEnvironmentVariable("BISON_PROPOSAL_DB") ?? "proposals.csv"));

builder.Services.AddSingleton<ObservationService>();
builder.Services.AddSingleton<CommentService>();
builder.Services.AddSingleton<ProposalService>();


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
    try
    {
        service.addObservation(cheep);
        return Results.Ok();
    }
    catch (InvalidOperationException)
    {
        return Results.BadRequest("Referenced observation does not exist");
    }
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
        return Results.BadRequest("Referenced observation does not exist");
    }
});

app.MapGet("/proposals", 
    (long id, ProposalService service) => 
        service.getProposals(id));

app.MapPost("/proposal",
    (ProposalService service, Proposal proposal) =>
    {
        try
        {
            service.addProposal(proposal);
            return Results.Ok();
        } 
        catch (InvalidOperationException)
        {
            return Results.BadRequest("Referenced observation does not exist");
        }
    });

app.Run();