using AtmService.Services.Commands;
using AtmService.Services.Manager;
using AtmService.Services.Queries;
using AtmService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Read config
var configuration = builder.Configuration;

// Add CORS
var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(allowedOrigins ?? new[] { "http://localhost:4200" })
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Convert Enumerations
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
 });


// EF Core setup (switchable)
var dbProvider = configuration["DatabaseOptions:Provider"];
var dbName = configuration["DatabaseOptions:DatabaseName"];

if (dbProvider == "InMemory")
{  // Set an Entityframework Core InMemory Database 
    builder.Services.AddDbContext<AtmDbContext>(opt =>
        opt.UseInMemoryDatabase(dbName ?? "AtmDb"));
}

builder.Services.AddScoped<IAccountQueryService, AccountQueryService>();
builder.Services.AddScoped<IAccountCommandService, AccountCommandService>();
builder.Services.AddScoped<IAtmManager, AtmManager>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Initialize InMemory DB
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AtmDbContext>();

    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors();
app.Run();
