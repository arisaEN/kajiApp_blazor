namespace kajiApp_blazor.Infra.DTO.AdminModel
{
    public class PaymentApprovalRecord
    {
        public int Id { get; set; }
        public string YearMonth { get; set; }
        public string Payer { get; set; }
        public int Payment { get; set; }
        public string Status { get; set; }

    }
}
