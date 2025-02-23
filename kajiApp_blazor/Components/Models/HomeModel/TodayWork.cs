using System.ComponentModel.DataAnnotations;

namespace kajiApp_blazor.Components.Models.HomeModel

{
    public class TodayWork
    {

        public int WorkId { get; set; }
        //[Required(ErrorMessage = "�Ǝ�������͂��Ă�������")]
        public string WorkName { get; set; } = "";
        //[Required(ErrorMessage = "���t����͂��Ă�������")]
        public DateTime Day { get; set; } = DateTime.Today;
        //[Required(ErrorMessage = "���O����͂��Ă�������")]
        public string Name { get; set; } = "";
        //[Required(ErrorMessage = "��������͂��Ă�������")]
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
