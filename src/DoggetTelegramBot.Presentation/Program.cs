using DoggetTelegramBot.Presentation;
using DoggetTelegramBot.Application;
using DoggetTelegramBot.Infrastructure;
using DoggetTelegramBot.Infrastructure.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

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
