﻿namespace kajiApp_blazor.Components.DataModels.HomeModel
{
    public class WorkList
    {
        public int Id { get; set; }
        public DateTime Day { get; set; }
        public string? Name { get; set; }
        public string? WorkName { get; set; }
        public double Percent { get; set; }
    }
}
