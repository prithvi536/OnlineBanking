using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineBanking_Final.Models;

public partial class AccountHolder
{
    public long AccountNumber { get; set; }

    public string AccountHoldername { get; set; } = null!;

    public string AccountType { get; set; } = null!;

    public DateOnly? AccountOpeningDate { get; set; }

    public int CustomerId { get; set; }
    [Required(ErrorMessage = "UserName is Required")]
    public string UserName { get; set; } = null!;
    [Required(ErrorMessage = "Password is Required")]

    public string Password { get; set; } = null!;

    public int TransactionPassword { get; set; }

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public decimal AccountBalance { get; set; }

    public string Ifsccode { get; set; } = null!;

    public virtual ICollection<DebitCard> DebitCards { get; set; } = new List<DebitCard>();

    public virtual ICollection<FixedDeposit> FixedDeposits { get; set; } = new List<FixedDeposit>();

    public virtual ICollection<FundTransfer> FundTransfers { get; set; } = new List<FundTransfer>();

    public virtual ICollection<Payee> Payees { get; set; } = new List<Payee>();
}
