using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TokenBucketLimiterConstants = DoggetTelegramBot.Domain.Common.Constants.Common.Constants.TokenBucketLimiter;

namespace DoggetTelegramBot.Presentation.Controllers
{
    [EnableRateLimiting(TokenBucketLimiterConstants.PolicyName.GlobalLimit)]
    public class HomeController : Controller
    {
        public IActionResult Index() => Content("Bot is running correctly...");
    }
}
