using System;
using System.Collections.Generic;

namespace kajiApp_blazor.Components.Models;

public partial class Eat
{
    public int Id { get; set; }

    public string? Year { get; set; }

    public string? Month { get; set; }

    public int? Amount { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Yyyymm { get; set; }
}
