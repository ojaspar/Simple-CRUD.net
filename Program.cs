using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidation();
builder.AddGameStoreDb();

builder.Services.AddScoped<IGenreService, GenreService>();

var app = builder.Build();
app.MapGamesEndpoints();
app.MapGenreEndpoints();

app.MigrateDb();


app.Run();
