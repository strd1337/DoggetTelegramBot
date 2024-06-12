using Microsoft.AspNetCore.Mvc;

namespace DoggetTelegramBot.Presentation.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("Bot is running correctly...");
        }
    }
}
