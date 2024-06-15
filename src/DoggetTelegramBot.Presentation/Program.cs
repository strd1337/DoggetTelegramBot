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

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var bot = app.Services.GetRequiredService<TelegramBotInitializer>();
await bot.InitializeAndRunAsync(app.Services.GetService<IServiceProvider>()!);

app.Run();
