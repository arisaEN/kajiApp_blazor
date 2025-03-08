namespace kajiApp_blazor.Components.DTO.LifeModel
{
    public class LifeRecord
    {
        public int Id { get; set; }
        public string? Year { get; set; }
        public string? Month { get; set; }
        //家賃
        public int Rent { get; set; }
        //水道
        public int Water { get; set; }
        //電気
        public int Electricity { get; set; }
        //ガス
        public int Gas { get; set; }
    }
}
