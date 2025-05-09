using System.ComponentModel.DataAnnotations;

namespace kajiApp_blazor.Infra.DTO.HomeModel

{
    public class TodayWork
    {

        public int WorkId { get; set; }
        //[Required(ErrorMessage = "家事名を入力してください")]
        public string WorkName { get; set; } = "";
        //[Required(ErrorMessage = "日付を入力してください")]
        public DateTime Day { get; set; } = DateTime.Today;
        //[Required(ErrorMessage = "名前を入力してください")]
        public string Name { get; set; } = "";
        //[Required(ErrorMessage = "割合を入力してください")]
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
