using csm.Server.Riot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DotNetEnv;
using csm.Server.Pipeline;
Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHostedService<Initial>();
builder.Services.AddHostedService<MatchIdWorker>();
builder.Services.AddHostedService<MatchDataWorker>();
builder.Services.AddHostedService<DBWorker>();

builder.Services.AddSingleton<SharedQueue>();

//Ihttpclientfactory
builder.Services.AddHttpClient();

builder.Services.AddTransient<RiotService>();

builder.Services.AddDbContext<csm.Server.Data.AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

var summoner = app.MapGroup("/api/v1/summoner");

// get all player info and display it using wpf
//summoner.MapGet("/{server}/{name}", (string name, int count = 10) => );

/*
Get a specific match on player profile
summoner.MapGet("/{server}/{name}/{matchid}", lambda)

Get most played champions
summoner.MapGet("/{server}/{name}/champions", lambda)

Get live game
summoner.MapGet("/{server}/{name}/live", lambda function)

We should have a PUT request for a refresh

summoner.MapPut("/{server}/{name}/refresh", lambda)
 */

var ladder = app.MapGroup("/api/v1/ladder");

/*
Get ladder for a specific server
ladder.MapGet("/{server}", lambda)

Update ladder with PUT req
ladder.MapPut("/{server}", lambda)
*/

var build = app.MapGroup("/api/v1/champions");

/*
Get a champions build based on tier and lane 
build.MapGet("/{championName}", (string championName, string lane, string tier) => ...) 
*/

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class Crawler : BackgroundService
{
    private readonly ILogger<Crawler> _logger;

    public Crawler(ILogger<Crawler> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stopToken)
    {
        _logger.LogInformation("Crawler is starting");

        while (!stopToken.IsCancellationRequested)
        {
            _logger.LogInformation("Crawler is working...");

            //await SomeWork();
        }

        _logger.LogInformation("Crawler has stopped work...");
    }
}
