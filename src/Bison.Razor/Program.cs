using Bison;
using Bison.Repository;
using Bison.Services;
using Bison.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var dbPath = Environment.GetEnvironmentVariable("BISONDBPATH") ??
             Path.Combine(Path.GetTempPath(), "bison.db");

builder.Services.AddDefaultIdentity<Author>(Options =>
    {
        Options.SignIn.RequireConfirmedAccount = true;
        // Users log in by email, so no two users may share one
        Options.User.RequireUniqueEmail = true;
        
        // To make sure the Definition is done is met,
        // we have to make sure the passwords 123456 and password are valid.
        // The following options makes sure of this
        Options.Password.RequireDigit = false;
        Options.Password.RequireNonAlphanumeric = false;
        Options.Password.RequireLowercase = false;
        Options.Password.RequireUppercase = false;
    }).AddEntityFrameworkStores<BisonDBContext>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
