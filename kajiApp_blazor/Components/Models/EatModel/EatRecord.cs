namespace kajiApp_blazor.Components.Models.EatModel
{
    public class EatRecord
    {

        //中身変更すること
        public int year { get; set; }
        public int month { get; set; }
        public int amount { get; set; }

    }

    public class EatDetailRecord
    {
        public int id { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int amount { get; set; }
       public string yyyymm { get; set; } = string.Empty;
    }
}
