using DoggetTelegramBot.Presentation;
using DoggetTelegramBot.Application;
using DoggetTelegramBot.Infrastructure;
using DoggetTelegramBot.Infrastructure.BotManagement;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.UseExceptionHandler();

app.UseRateLimiter();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var bot = app.Services.GetRequiredService<BotInitializer>();
await bot.InitializeAndRunAsync(app.Services.GetService<IServiceProvider>()!);

app.Run();
