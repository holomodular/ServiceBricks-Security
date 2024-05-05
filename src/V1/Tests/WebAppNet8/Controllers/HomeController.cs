using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using ServiceBricks;

using ServiceBricks.Security;
using WebApp.ViewModel.Home;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    [Route("")]
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly IBusinessRuleService _businessRuleService;
        private readonly IUserManagerService _userManagerService;

        public HomeController(
            IBusinessRuleService businessRuleService,
            IUserManagerService userManagerService)
        {
            _businessRuleService = businessRuleService;
            _userManagerService = userManagerService;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            HomeViewModel model = new HomeViewModel();
            return View(model);
        }

        [HttpGet]
        [Route("Error")]
        public IActionResult Error(string message = null)
        {
            var model = new ErrorViewModel()
            {
                Message = message
            };
            return View("Error", model);
        }

        [HttpPost]
        [Route("SetLanguage")]
        [ValidateAntiForgeryToken]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            var feature = HttpContext.Features.Get<ITrackingConsentFeature>();
            if (feature != null && feature.HasConsent)
            {
                // Consented, use cookie
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

                if (string.IsNullOrEmpty(returnUrl))
                    return LocalRedirect("/");
                else
                    return LocalRedirect(returnUrl);
            }
            else
            {
                // Not consented, use querystring
                if (string.IsNullOrEmpty(returnUrl))
                    return LocalRedirect("/?culture=" + culture);
                else
                {
                    if (returnUrl.Contains('?'))
                        return LocalRedirect(returnUrl + "&culture=" + culture);
                    else
                        return LocalRedirect(returnUrl + "?culture=" + culture);
                }
            }
        }

        [HttpPost]
        [Route("WithdrawCookiesConsent")]
        [ValidateAntiForgeryToken]
        public IActionResult WithdrawCookiesConsent()
        {
            var feature = HttpContext.Features.Get<ITrackingConsentFeature>();
            feature?.WithdrawConsent();

            //Delete all other cookies used in the site
            Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);

            return LocalRedirect("/");
        }
    }
}