using System;
using System.Collections.Generic;

namespace kajiApp_blazor.Domain.Entity;

public partial class WorkList
{
    public int WorkId { get; set; }

    public string? WorkName { get; set; }

    public int? WorkNamePoint { get; set; }

    public int? 家事分類区分番号 { get; set; }
}
