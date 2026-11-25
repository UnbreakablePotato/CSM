using csm.Server.Riot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DotNetEnv;
using csm.Server.Pipeline;
using csm.Server.DTO;
using System.Text.Json;
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

//Ikke helt done

summoner.MapGet("/{server}/{name+tag}", async (RiotService riotservice, string server, string name, string tag, int count = 10) =>
{
    var route = RegionRouter.Resolve(server);
    var RegionalServer = route.Regional;
    var PlatformServer = route.Platform;

    try
    {
        var obj = await riotservice.GetPuuidAsync(RegionalServer, name, tag);
        var user = await riotservice.GetLEDTO(PlatformServer, obj.PuuidId);

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
});

summoner.MapGet("/{server}/{name+tag}/{matchid}", async (RiotService riotservice, string server, string matchid) =>
{
    var route = RegionRouter.Resolve(server);

    var regional = route.Regional;

    try
    {
        var obj = await riotservice.GetMatchData(regional, matchid);
        return obj;
    }
    catch(Exception ex)
    {
        throw new Exception($"Could not fetch match data from: {matchid}", ex);
    }

});

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


//Get ladder for a specific server
ladder.MapGet("/{server}", async (string server, RiotService riotService) =>
{
    var obj = await riotService.GetLadder(server);

    if (obj == null)
    {
        return Results.NotFound($"Could not return the ranked ladder for '{server}'");
    }

    return Results.Ok(obj);

}
);

//Update ladder with PUT req
//ladder.MapPut("/{server}", lambda)


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
