namespace kajiApp_blazor.Components.DTO.AdminModel
{
    public class PaymentDetail
    {
        public string YearMonth { get; set; }
        public string Food { get; set; }  // 文字列型に変更
        public string Rent { get; set; }  // 文字列型に変更
        public string Water { get; set; }  // 文字列型に変更
        public string Electricity { get; set; }  // 文字列型に変更
        public string Gas { get; set; }  // 文字列型に変更
        public string LifeTotal { get; set; }  // 文字列型に変更
        public string FullTotal { get; set; }  // 文字列型に変更
        public string SharedCost { get; set; }  // 文字列型に変更
        public string AdjustedSharedCost { get; set; }  // 文字列型に変更
        public string Percentage { get; set; }  // 既に string なのでそのまま
        public string Payment { get; set; }  // 文字列型に変更
        public string Payer { get; set; }  // 既に string なのでそのまま
        public string Status { get; set; }  // 既に string なのでそのまま
    }
}
