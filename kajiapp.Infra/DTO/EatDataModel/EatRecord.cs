namespace kajiApp_blazor.Infra.DTO.EatModel
{
    public class EatRecord
    {

        //中身変更すること
        public string? Year { get; set; }
        public string? Month { get; set; }
        public int Amount { get; set; }

    }

    public class EatDetailRecord
    {
        public int Id { get; set; }
        public string? Year { get; set; }
        public string? Month { get; set; }
        public int Amount { get; set; }
       public string? Yyyymm { get; set; } = string.Empty;
    }
}
