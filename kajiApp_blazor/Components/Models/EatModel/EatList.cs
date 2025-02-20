namespace kajiApp_blazor.Components.Models.EatModel
{
    public class EatList
    {

        //中身変更すること
        public int Id { get; set; }
        public DateTime Day { get; set; }
        public string Name { get; set; }
        public string WorkName { get; set; }
        public double Percent { get; set; }
    }
}
