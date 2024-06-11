using DoggetTelegramBot.Presentation;
using DoggetTelegramBot.Application;
using DoggetTelegramBot.Infrastructure;
using DoggetTelegramBot.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

var bot = app.Services.GetRequiredService<TelegramBotInitializer>();
await bot.InitializeAndRunAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
