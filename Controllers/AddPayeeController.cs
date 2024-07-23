using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OnlineBanking_Final.Models;
namespace OnlineBanking_Final.Controllers
{
    public class AddPayeeController : Controller
    {
        OnlineBanking2Context obc = new OnlineBanking2Context();
        public IActionResult PAdd()
        {
            string accountNumberString = HttpContext.Session.GetString("LoggedInAccountNumber");
            long loggedInAccountNumber = long.Parse(accountNumberString);

            Payee newPayee = new Payee();
            newPayee.AccountNumberHolder = loggedInAccountNumber;

            return View(newPayee);
        }
        [HttpPost]
        public IActionResult PAdd(Payee p)
        {
            // int? loggedInAccountNumber = HttpContext.Session.GetInt32("LoggedInAccountNumber");
            string accountNumberString = HttpContext.Session.GetString("LoggedInAccountNumber");
            long loggedInAccountNumber = long.Parse(accountNumberString);

            Payee newPayee = new Payee();
            newPayee.AccountNumberHolder = loggedInAccountNumber;
            if (p.PayeeId != null && p.PayeeAccountNumber != null && p.AccountNumberHolder != null && p.NickName != null)
            {
                var v1 = obc.Payees.SingleOrDefault(x => x.PayeeId == p.PayeeId);
                if(v1!=null)
                {
                    ModelState.AddModelError("PayeeId", "These PayeeId is Already Present Enter Another PayeeId");
                    return View(newPayee);
                }
                
                
                
                var val = obc.Payees.SingleOrDefault(x => x.AccountNumberHolder == p.AccountNumberHolder  && x.PayeeAccountNumber == p.PayeeAccountNumber);
                if(val!=null)
                {
                    ModelState.AddModelError("PayeeAccountNumber", "This Payee Already Added");
                 
                    return View(newPayee);
                }

                if (p.AccountNumberHolder == loggedInAccountNumber)
                {
                    obc.Payees.Add(p);
                    obc.SaveChanges();
                    ViewBag.PayeeAdded = "Payee Added Sucessfully";
                     return View("PAdd");
                }
                else
                {
                    ViewBag.Message = "Enter Correct AccountNumber.";
                    return View(newPayee);
                }
            }
            else
            {
                return View(newPayee);
            }

        }
    }
}
