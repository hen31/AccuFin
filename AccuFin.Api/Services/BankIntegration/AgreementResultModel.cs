namespace AccuFin.Api.Services.BankIntegration
{
    public class AgreementResultModel
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public int Max_historical_days { get; set; }
        public int Access_valid_for_days { get; set; }
        public string[] Access_scope { get; set; }
        public string Accepted { get; set; }
        public string Institution_id { get; set; }
    }
}
