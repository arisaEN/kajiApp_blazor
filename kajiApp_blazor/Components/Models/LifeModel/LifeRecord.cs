namespace kajiApp_blazor.Components.Models.LifeModel
{
    public class LifeRecord
    {
        public int id { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        //家賃
        public int rent { get; set; }
        //水道
        public int water { get; set; }
        //電気
        public int electricity { get; set; }
        //ガス
        public int gas { get; set; }
    }
}
