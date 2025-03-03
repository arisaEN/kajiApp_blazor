using System;
using System.Collections.Generic;

namespace kajiApp_blazor.Components.Models;

public partial class MonthlyWorkSummaryView
{
    public string? Yyyymm { get; set; }

    public string? Name { get; set; }

    public double? TotalPoints { get; set; }

    public int? Percentage { get; set; }
}
