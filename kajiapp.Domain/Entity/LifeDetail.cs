﻿using System;
using System.Collections.Generic;

namespace kajiApp_blazor.Domain.Entity;

public partial class LifeDetail
{
    public int Id { get; set; }

    public string? Year { get; set; }

    public string? Month { get; set; }

    public int? Rent { get; set; }

    public int? Water { get; set; }

    public int? Electricity { get; set; }

    public int? Gas { get; set; }

    public DateTime? InputTime { get; set; }

    public string? Yyyymm { get; set; }
}
