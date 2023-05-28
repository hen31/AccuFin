namespace AccuFin.Api.Services.BankIntegration
{
    public class ListBankAccountResultModel
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Agreements { get; set; }
        public string[] Accounts { get; set; }
        public string Reference { get; set; }

    }
}
