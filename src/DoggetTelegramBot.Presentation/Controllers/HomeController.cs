using DoggetTelegramBot.Domain.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DoggetTelegramBot.Presentation.Controllers
{
    [EnableRateLimiting(Constants.TokenBucketLimiter.PolicyName.GlobalLimit)]
    public class HomeController : Controller
    {
        public IActionResult Index() => Content("Bot is running correctly...");
    }
}
