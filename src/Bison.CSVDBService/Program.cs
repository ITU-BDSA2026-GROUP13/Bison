using Bison;
using DefaultNamespace;
using SimpleDB;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Creates DBs
var observations = ObservationDatabase<Cheep>.Instance;
var comments = CommentDatabase<Comment>.Instance;

// Read
app.MapGet("/observations", () => observations.Read());
app.MapGet("/comments", () => comments.Read());

// Store
app.MapPost("/comment", () => "Hello World!");
app.MapPost("/observation", () => "Hello World!");


app.Run();