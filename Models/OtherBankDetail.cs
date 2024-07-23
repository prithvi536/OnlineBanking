using System;
using System.Collections.Generic;

namespace OnlineBanking_Final.Models;

public partial class OtherBankDetail
{
    public long OaccountNumber { get; set; }

    public string OaccountHoldername { get; set; } = null!;

    public string OaccountType { get; set; } = null!;

    public DateOnly? OaccountOpeningDate { get; set; }

    public int? OcustomerId { get; set; }

    public string? OuserName { get; set; }

    public string? Opassword { get; set; }

    public int? OtransactionPassword { get; set; }

    public string? Oemail { get; set; }

    public string? Ophone { get; set; }

    public decimal? OaccountBalance { get; set; }

    public string Oifsccode { get; set; } = null!;

    public virtual ICollection<Payee> Payees { get; set; } = new List<Payee>();
}
