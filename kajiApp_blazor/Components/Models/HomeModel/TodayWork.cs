using System.ComponentModel.DataAnnotations;

namespace kajiApp_blazor.Components.Models.HomeModel

{
    public class TodayWork
    {

        public int WorkId { get; set; }
        //[Required(ErrorMessage = "‰Æ––¼‚ğ“ü—Í‚µ‚Ä‚­‚¾‚³‚¢")]
        public string WorkName { get; set; } = "";
        //[Required(ErrorMessage = "“ú•t‚ğ“ü—Í‚µ‚Ä‚­‚¾‚³‚¢")]
        public DateTime Day { get; set; } = DateTime.Today;
        //[Required(ErrorMessage = "–¼‘O‚ğ“ü—Í‚µ‚Ä‚­‚¾‚³‚¢")]
        public string Name { get; set; } = "";
        //[Required(ErrorMessage = "Š„‡‚ğ“ü—Í‚µ‚Ä‚­‚¾‚³‚¢")]
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
