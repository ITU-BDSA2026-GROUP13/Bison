using Bison;
using Bison.Repository;
using Bison.Services;
using Bison.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);
var dbPath = Environment.GetEnvironmentVariable("BISONDBPATH") ??
             Path.Combine(Path.GetTempPath(), "bison.db");


// Add services to the container.
builder.Services.AddRazorPages();
dbPath = Path.GetFullPath(dbPath);
Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BisonDBContext>(options => options.UseSqlite(connectionString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IObservationRepository, ObservationRepository>();
builder.Services.AddScoped<IObservationService, ObservationService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IProposalService, ProposalService>();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    using var context = scope.ServiceProvider.GetRequiredService<BisonDBContext>();
    context.Database.Migrate();
    DbInitializer.SeedDatabase(context);
}

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
