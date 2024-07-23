using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineBanking_Final.Models;

public partial class Payee
{
    [Required(ErrorMessage = "PayeeId is Required")]
    public int? PayeeId { get; set; }
    [Required(ErrorMessage = "PayeeAccount Number is Required")]
    public long? PayeeAccountNumber { get; set; }
    [Required(ErrorMessage = "Account Number Of Holder is Required")]
    public long? AccountNumberHolder { get; set; }
    [Required(ErrorMessage = "Nick Name is Required")]
    public string NickName { get; set; } = null!;

    public virtual AccountHolder AccountNumberHolderNavigation { get; set; } = null!;

    public virtual ICollection<FundTransfer> FundTransfers { get; set; } = new List<FundTransfer>();

    public virtual OtherBankDetail PayeeAccountNumberNavigation { get; set; } = null!;
}
