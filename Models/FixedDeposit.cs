using System;
using System.Collections.Generic;

namespace OnlineBanking_Final.Models;

public partial class FixedDeposit
{
    public int FdId { get; set; }

    public long? AccountNumber { get; set; }

    public decimal FdAmount { get; set; }

    public decimal? InterestRate { get; set; }

    public int? TenureInMonths { get; set; }

    public decimal? MaturityAmount { get; set; }

    public DateOnly? FdOpeningDate { get; set; }

    public DateOnly? MaturityDate { get; set; }

    public DateOnly? WithdrawlDate { get; set; }

    public virtual AccountHolder? AccountNumberNavigation { get; set; }
}
public class FDCalculatorModel
{
    public decimal PrincipalAmount { get; set; }
    public double InterestRate { get; set; } // Annual interest rate in percentage
    public int TimePeriod { get; set; } // Time period in months
    public decimal MaturityAmount { get; set; }
    public decimal InterestEarned { get; set; }
}
public class FDClosureViewModel
{
    public FixedDeposit FixedDeposit { get; set; }
    public decimal CalculatedIntrest { get; set; }
    public decimal MaturityAmount { get; set; }
    public decimal PenaltyAmount { get; set; }
    public virtual FixedDeposit? FdIdNavigation { get; set; }




}
