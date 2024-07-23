using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBanking_Final.Models;
using System.Collections.Generic;

namespace MyProj.Controllers
{
    public class FdCloseController : Controller
    {
        OnlineBanking2Context ctx = new OnlineBanking2Context();
        public IActionResult Read()
        {

            ViewBag.Message = TempData["Message"] as string;
            string accountNumberString = HttpContext.Session.GetString("LoggedInAccountNumber");
            long loggedInAccountNumber = long.Parse(accountNumberString);

            var fdClose = ctx.FixedDeposits
                     .Where(a => a.AccountNumber == loggedInAccountNumber)
                     .ToList();

            /* IEnumerable<FixedDeposit> lst = ctx.FixedDeposits.ToList<FixedDeposit>();*/

            return View(fdClose);
        }

        [HttpPost]
        public IActionResult CloseFD(int id)
        {
            var fd = ctx.FixedDeposits.FirstOrDefault(f => f.FdId == id);
            if (fd == null)
            {
                return NotFound();
            }

            var viewModel = new FDClosureViewModel
            {
                FixedDeposit = fd,
                CalculatedIntrest = CalculatedInterest(fd),
                MaturityAmount = CalculateMaturityAmount(fd),


            };
            DateTime newMaturityDate = DateTime.Now;
            fd.MaturityDate = DateOnly.FromDateTime(newMaturityDate);

            return View("ConfirmCloseFD", viewModel);
        }

        

        [HttpPost]
        public IActionResult ConfirmCloseFD(int id)
        {
            var fd = ctx.FixedDeposits.FirstOrDefault(f => f.FdId == id);


            if (fd == null)
            {
                return NotFound();
            }
            var account = ctx.AccountHolders.FirstOrDefault(a => a.AccountNumber == fd.AccountNumber);
            if (account != null)
            {
                var maturityAmount = CalculateMaturityAmount(fd);
                account.AccountBalance += maturityAmount;
                ctx.AccountHolders.Update(account);
            }
            ctx.FixedDeposits.Remove(fd);
            ctx.SaveChanges();

            TempData["Message"] = $"FD closed successfully. Maturity amount of {fd.MaturityAmount:C} has been credited to your savings account.";

            return RedirectToAction("Read");
        }

       

        private decimal CalculatedInterest(FixedDeposit fd)
        {
            return (decimal)(fd.FdAmount * (decimal)(fd.InterestRate / 100) * fd.TenureInMonths / 12);

            //var openingDate = fd.FdOpeningDate?.ToDateTime(TimeOnly.MinValue) ?? DateTime.MinValue;
            //var currentDate = DateTime.Now;
            //var totalDays = (currentDate - openingDate).TotalDays;

            //return (fd.FdAmount.GetValueOrDefault() * (fd.InterestRate.GetValueOrDefault() / 100) * (decimal)totalDays) / 365;
        }
        private decimal CalculateMaturityAmount(FixedDeposit fd)
        {
            return (decimal)(fd.FdAmount + CalculatedInterest(fd));
        }
       



    }
}
