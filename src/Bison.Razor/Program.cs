using Bison;

var builder = WebApplication.CreateBuilder(args);
var dbPath = Environment.GetEnvironmentVariable("BISONDBPATH") ??
             Path.Combine(Path.GetTempPath(), "bison.db");

Taxonomy taxonomy = new Taxonomy();
taxonomy.Taxonloader();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton(taxonomy);
builder.Services.AddSingleton(new DBFacade(dbPath));
builder.Services.AddSingleton<IObservationService, ObservationService>();
builder.Services.AddSingleton<ICommentService, CommentService>();
builder.Services.AddSingleton<IProposalService, ProposalService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();
