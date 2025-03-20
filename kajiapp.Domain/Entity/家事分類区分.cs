using System;
using System.Collections.Generic;

namespace kajiApp_blazor.Domain.Entity;

public partial class 家事分類区分
{
    public int 家事分類区分id { get; set; }

    public int? 区分番号 { get; set; }

    public string? 区分名 { get; set; }
}
