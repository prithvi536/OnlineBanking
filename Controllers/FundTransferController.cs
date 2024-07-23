using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineBanking_Final.Models;
using System.Linq;
using Newtonsoft.Json;

namespace OnlineBanking_Final.Controllers
{

    public class FundTransferController : Controller
    {
        OnlineBanking2Context obc = new OnlineBanking2Context();
        [HttpGet]
        public IActionResult FTransfer()
        {
            string accountNumberString = HttpContext.Session.GetString("LoggedInAccountNumber");
            long loggedInAccountNumber = long.Parse(accountNumberString);

            // long loggedInAccountNumber = 100000000001;

            List<Payee> payees = obc.Payees
                .Where(p => p.AccountNumberHolder == loggedInAccountNumber)
                .ToList();

            ViewBag.Payees = new SelectList(payees, "PayeeId", "NickName");
            
            ViewBag.AccountHolderNumber = loggedInAccountNumber;
            FundTransfer fundTransfer = new FundTransfer
            {
                AccountHolderNumber = loggedInAccountNumber
            };

            return View(fundTransfer);
        }
        [HttpPost]
        public IActionResult FTransfer(FundTransfer f)
        {

            string accountNumberString = HttpContext.Session.GetString("LoggedInAccountNumber");
            long loggedInAccountNumber = long.Parse(accountNumberString);


            List<Payee> payees = obc.Payees
                .Where(p => p.AccountNumberHolder == loggedInAccountNumber)
                .ToList();

            ViewBag.Payees = new SelectList(payees, "PayeeId", "NickName");


            if (f.SelectedPayeeId == null)
            {
                ModelState.AddModelError("SelectedPayeeId", "Select the Payee Name");
                return View();
            }

            if (f.AccountHolderNumber != loggedInAccountNumber)
            {

                ModelState.AddModelError("AccountHolderNumber", "Enter Correct Account Number");
                return View();
            }

            var account = obc.AccountHolders.FirstOrDefault(x => x.AccountNumber == f.AccountHolderNumber);

            if (f.Amount > account.AccountBalance)
            {
                ModelState.AddModelError("Amount", "Insufficient balance ,Your Account Balance is :"+ account.AccountBalance);
                return View();
            }



            switch (f.PaymentMethod)
            {
                case "IMPS":
                    if (f.Amount < 1 || f.Amount > 200000)
                    {
                        ModelState.AddModelError("Amount", "Enter Amount between 1 to 200000");
                        return View();
                    }
                    break;
                case "NEFT":

                    if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                    {
                        ModelState.AddModelError("PaymentMethod", "Today is a holiday for NEFT transfers. Please select another Payment Method.");
                        return View();
                    }

                    TimeSpan startTime = TimeSpan.Parse("08:00"); // 8:00 AM
                    TimeSpan endTime = TimeSpan.Parse("19:00");   // 7:00 PM
                    TimeSpan currentTime = DateTime.Now.TimeOfDay;

                    if (currentTime < startTime || currentTime > endTime)
                    {
                        ModelState.AddModelError("PaymentMethod", "NEFT transfers are only available between 8:00 AM to 7:00 PM.");
                        return View();
                    }
                    if (f.Amount < 1)
                    {
                        ModelState.AddModelError("Amount", "Enter Amount greater than 1");
                        return View();
                    }
                    break;

                case "RTGS":

                    if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                    {
                        ModelState.AddModelError("PaymentMethod", "Today is a holiday for RTGS transfers. Please select another Payment Method.");
                        return View();
                    }

                    startTime = TimeSpan.Parse("09:00"); // 9:00 AM
                    endTime = TimeSpan.Parse("16:30");   // 4:30 PM
                    currentTime = DateTime.Now.TimeOfDay;

                    if (currentTime < startTime || currentTime > endTime)
                    {
                        ModelState.AddModelError("PaymentMethod", "RTGS transfers are only available between 9:00 AM to 4:30 PM.");
                        return View();
                    }
                    if (f.Amount < 200000)
                    {
                        ModelState.AddModelError("Amount", "Enter Amount Greater than or Equal to 200000 or Above.");
                        return View();
                    }



                    break;
                default:
                    ModelState.AddModelError("PaymentMethod", "Please select a valid Payment Method");
                    return View();

            }


            return RedirectToAction("Pin", "Auth",f);
        }

    }
}
