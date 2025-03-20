using System;
using System.Collections.Generic;

namespace kajiApp_blazor.Components.Entity;

public partial class Payment
{
    public int Id { get; set; }

    public string? Year { get; set; }

    public string? Month { get; set; }

    public int? NameCode { get; set; }

    public int? Pay { get; set; }

    public DateTime? InputTime { get; set; }

    public string? Yyyymm { get; set; }

    public string? 支払状態 { get; set; }

    public string? 決裁 { get; set; }
}
