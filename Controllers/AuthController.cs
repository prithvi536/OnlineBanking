using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineBanking_Final.Models;

namespace OnlineBanking_Final.Controllers
{
    public class AuthController : Controller
    {
        OnlineBanking2Context ctx = new OnlineBanking2Context();
        
        private readonly EmailService _email;

        public AuthController( EmailService email)
        {
            _email = email;
        }

        public IActionResult Pin(FundTransfer obj)
        {
            ViewBag.accountNumber = obj.AccountHolderNumber;

            return View();
        }

        [HttpPost]
        public IActionResult Pin(AccountHolder acc,  FundTransfer obj)
        {
            long accNum = (long)obj.AccountHolderNumber;
            var val = ctx.AccountHolders.FirstOrDefault(x => x.AccountNumber == accNum);
            if (val.TransactionPassword != acc.TransactionPassword)
            {
                return View(obj);
            }

            // Generate OTP
            var otp = GenerateOtp();
            TempData["OTP"] = otp;
            TempData["AccountNumber"] = accNum.ToString();  // Convert to string

            // Send OTP to email
            var subject = "Your OTP Code";
            var body = $"Your OTP code is {otp}";
            _email.SendEmail(val.Email, subject, body);

            return RedirectToAction("VerifyOtp",obj);

        }
        public IActionResult VerifyOtp(FundTransfer obj)
        {          
            return View(obj);
        }
        [HttpPost]
        public IActionResult VerifyOtp(string otp, FundTransfer obj)
        {
            var expectedOtp = TempData["OTP"]?.ToString();
            var accountNumberString = TempData["AccountNumber"]?.ToString();

            if (otp != expectedOtp || long.TryParse(accountNumberString, out long accountNumber))
            {
                ViewBag.Message = "Invalid OTP";
                TempData.Keep("OTP");
                TempData.Keep("AccountNumber");
                return View(obj);

            }

            TempData.Remove("OTP");

            return RedirectToAction("Transfer", obj);

        }
        public IActionResult Transfer(FundTransfer fundTransfer)
        {

                decimal a = fundTransfer.Amount;
                fundTransfer.ReferenceNumber = GenerateReferenceNumber();

                var account = ctx.AccountHolders.FirstOrDefault(p => p.AccountNumber == fundTransfer.AccountHolderNumber);
                var payee = ctx.Payees.FirstOrDefault(p => p.PayeeId == fundTransfer.SelectedPayeeId);
                long acc = (long)payee.PayeeAccountNumber;
                var other = ctx.OtherBankDetails.FirstOrDefault(p => p.OaccountNumber == acc);

          

                fundTransfer.TransactionDate = DateTime.Now;
                    fundTransfer.Status = "Completed";
                   

                    account.AccountBalance -= fundTransfer.Amount;
                    other.OaccountBalance += fundTransfer.Amount;
                    ctx.FundTransfers.Add(fundTransfer);
                    ctx.SaveChanges();

/*                    ViewBag.AccountNumber = payee.PayeeAccountNumber;
*/
                    return RedirectToAction("TransactionDetails", fundTransfer);

}

        public IActionResult TransactionDetails(FundTransfer fundTransfer)
        {
            var f = ctx.FundTransfers.FirstOrDefault(ft => ft.TransferId == fundTransfer.TransferId);
           
            ViewBag.AccountNumber = ctx.Payees.FirstOrDefault(p => p.PayeeId == fundTransfer.SelectedPayeeId)?.PayeeAccountNumber;

            return View(fundTransfer);
        }


        private string GenerateOtp()
        {
            var random = new Random();
            return random.Next(1000, 9999).ToString(); 
        }
        private string GenerateReferenceNumber()
        {
            
            Guid guid = Guid.NewGuid();

            string newGuid = Convert.ToBase64String(guid.ToByteArray())
                                       .Replace("/", "_")
                                       .Replace("+", "-")
                                       .Substring(0, 10); // Take the first 10 characters

            return $"FT{newGuid}";
        }
    }
}
