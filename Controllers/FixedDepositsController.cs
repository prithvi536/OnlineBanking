using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBanking_Final.Models;
using Rotativa.AspNetCore;

namespace OnlineBanking.Controllers
{
    public class FixedDepositsController : Controller
    {
        OnlineBanking2Context ctx = new OnlineBanking2Context();

        public IActionResult CreateFD()
        {
            string accountNumberString = HttpContext.Session.GetString("LoggedInAccountNumber");
            long loggedInAccountNumber = long.Parse(accountNumberString);

         /*   var account = ctx.AccountHolders.FirstOrDefault();*/
            if (loggedInAccountNumber == null)
            {
                return NotFound();
            }
            ViewBag.AccountNumber = loggedInAccountNumber;
            return View();
        }

        [HttpPost]
        public IActionResult CreateFD(FixedDeposit fixeddeposit)
        {
            if (ModelState.IsValid)

            {
                string accountNumberString = HttpContext.Session.GetString("LoggedInAccountNumber");
                long loggedInAccountNumber = long.Parse(accountNumberString);

                //var account = ctx.AccountHolders.FirstOrDefault();
                var account = ctx.AccountHolders.FirstOrDefault(a => a.AccountNumber == loggedInAccountNumber);
                if (fixeddeposit.TenureInMonths < 6)
                {
                    ModelState.AddModelError("TenureInMonths", "Minimum tenure for fixed deposit should be at least 6 months.");
                    return View();
                }

                if (account != null && account.AccountBalance >= fixeddeposit.FdAmount)
                {
                    fixeddeposit.MaturityDate = DateOnly.FromDateTime(DateTime.Now.AddMonths((int)fixeddeposit.TenureInMonths));
                    fixeddeposit.FdOpeningDate = DateOnly.FromDateTime(DateTime.Now);
                    fixeddeposit.InterestRate = (decimal?)6.5;
                    fixeddeposit.AccountNumber = account.AccountNumber;

                    double interestRatePerMonth = 6.5 / 12 / 100;
                    long Interest = (long)((long)fixeddeposit.FdAmount * interestRatePerMonth * fixeddeposit.TenureInMonths );
                    fixeddeposit.MaturityAmount =fixeddeposit.FdAmount + Interest;

                    account.AccountBalance -= fixeddeposit.FdAmount;
                    ctx.AccountHolders.Update(account);
                    ctx.SaveChanges();

                    //fd
                    ctx.FixedDeposits.Add(fixeddeposit);
                    ctx.SaveChanges();

                   // ViewBag.fd = fixeddeposit;

                    
                    return RedirectToAction("Details", new { id = fixeddeposit.FdId });

                }
                else
                {
                    ModelState.AddModelError("FdAmount", "Insufficient balance in account.");
                }
                
            }

            return View(fixeddeposit);
        }
        public IActionResult Details(int id)
        {
            
            var fixedDeposit = ctx.FixedDeposits.FirstOrDefault(fd => fd.FdId == id);

            if (fixedDeposit == null)
            {
                return NotFound(); 
            }

            return View(fixedDeposit);
            
        }
        public IActionResult DownloadPdf(int id)
        {
            var fixedDeposit = ctx.FixedDeposits.FirstOrDefault(fd => fd.FdId == id);

            if (fixedDeposit == null)
            {
                return NotFound();
            }

            return new ViewAsPdf("DownloadPdf", fixedDeposit)
            {
                FileName = "FixedDepositDetails.pdf"
            };
        }

        public IActionResult Index()
        {
            return View(new FDCalculatorModel());
        }

        [HttpPost]
        public IActionResult Calculate(FDCalculatorModel model)
        {
            if (ModelState.IsValid)
            {
                double interestRatePerMonth = model.InterestRate / 12 / 100;
                model.InterestEarned = model.PrincipalAmount * (decimal)interestRatePerMonth * model.TimePeriod;
                model.MaturityAmount = model.PrincipalAmount + model.InterestEarned;
            }

            return View("Index", model);
        }
    }
}
