using System;
using System.Collections.Generic;

namespace OnlineBanking_Final.Models;

public partial class DebitCard
{
    public int Id { get; set; }

    public long? CardNumber { get; set; }

    public string? AccountHoldername { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public int? Pin { get; set; }

    public long? AccountNumber { get; set; }

    public virtual AccountHolder? AccountNumberNavigation { get; set; }
}
