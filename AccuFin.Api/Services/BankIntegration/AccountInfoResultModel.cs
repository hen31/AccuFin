namespace AccuFin.Api.Services.BankIntegration
{
    public class AccountInfoResultModel
    {


        public string Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Last_accessed { get; set; }
        public string Iban { get; set; }
        public string Institution_id { get; set; }
        public string Status { get; set; }
        public string Owner_name { get; set; }

    }
}
