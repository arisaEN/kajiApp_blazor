namespace kajiApp_blazor.Components.Models.HomeModel
{
    public class TodayWork
    {
        public int WorkId { get; set; }
        public string WorkName { get; set; } = "";
        public DateTime Day { get; set; } = DateTime.Today;

        public string Name { get; set; } = "";
        public int Percent { get; set; } = 100;
    }

    public class WorkItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public WorkItem(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

}
