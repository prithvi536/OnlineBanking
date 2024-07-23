using System.Numerics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineBanking_Final.Models;

namespace OnlineBanking_Final.Controllers
{
    public class UserLoginController : Controller
    {
        OnlineBanking2Context obc = new OnlineBanking2Context();
        public IActionResult ULogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ULogin(AccountHolder account)
        {
            if (account.UserName != null && account.Password != null)
            {
                var val = obc.AccountHolders.SingleOrDefault(x => x.UserName == account.UserName && x.Password == account.Password);
                if (val != null)
                {
                    //  HttpContext.Session.SetInt32("LoggedInAccountNumber", (int)val.AccountNumber);

                    HttpContext.Session.SetString("LoggedInAccountNumber", val.AccountNumber.ToString());

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Invalid username or password.";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

    }
}
