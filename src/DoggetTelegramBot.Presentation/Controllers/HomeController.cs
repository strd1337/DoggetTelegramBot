using Microsoft.AspNetCore.Mvc;

namespace DoggetTelegramBot.Presentation.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => Content("Bot is running correctly...");
    }
}
