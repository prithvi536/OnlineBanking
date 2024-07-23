using Microsoft.AspNetCore.Mvc;

namespace OnlineBanking_Final.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Signout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("ULogin","UserLogin");
        }
    }
}
