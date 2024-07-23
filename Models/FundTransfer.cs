using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineBanking_Final.Models;

public partial class FundTransfer
{
    public int TransferId { get; set; }
    [Required(ErrorMessage = "Select PayeeId.")]
    public int? SelectedPayeeId { get; set; }
    [Required(ErrorMessage = "Holder Account Number is Required")]
    public long? AccountHolderNumber { get; set; }
    [Required(ErrorMessage = "Amount is Required")]
    public decimal Amount { get; set; }
    [Required(ErrorMessage = "Select PaymentMethod")]
    public string PaymentMethod { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? TransactionDate { get; set; }

    public string? ReferenceNumber { get; set; }

    public virtual AccountHolder AccountHolderNumberNavigation { get; set; } = null!;

    public virtual Payee SelectedPayee { get; set; } = null!;
}
