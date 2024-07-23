using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBanking_Final.Models;
using System.Security.Claims;

namespace OnlineBanking.Controllers
{
    public class DebitCardController : Controller
    {
        OnlineBanking2Context ctx = new OnlineBanking2Context();

        public IActionResult DebitCardDetails()
        {
            string accountNumberString = HttpContext.Session.GetString("LoggedInAccountNumber");
            long loggedInAccountNumber = long.Parse(accountNumberString);

            var card = ctx.DebitCards.FirstOrDefault(d=>d.AccountNumber== loggedInAccountNumber);    
            return View(card);
        }

        [HttpPost]  
        public IActionResult ChangePin(int cardId, int oldPin, int newPin)
        {
            
                var card = ctx.DebitCards.Find(cardId);
                if (card == null)
                {
                    return NotFound();
                }

                if (card.Pin != oldPin)
                {   
                    ViewBag.Pin = "Entered Old Pin is Incorrect";

                    return View("DebitCardDetails", card);
                }

                card.Pin = newPin;
                ctx.SaveChanges();

                ViewBag.Message = "PIN successfully changed.";
                return View("DebitCardDetails", card);
            
        }
    } 
}


