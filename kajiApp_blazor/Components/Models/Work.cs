using System;
using System.Collections.Generic;

namespace kajiApp_blazor.Components.Models;

public partial class Work
{
    public int Id { get; set; }

    public DateOnly? Day { get; set; }

    public string? Name { get; set; }

    public int? WorkId { get; set; }

    public string? Work1 { get; set; }

    public string? Percent { get; set; }
}
