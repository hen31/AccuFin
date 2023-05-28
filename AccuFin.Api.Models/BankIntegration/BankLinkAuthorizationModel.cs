namespace AccuFin.Api.Models.BankIntegration
{
    public class BankLinkAuthorizationModel
    {
        public string Id { get; set; }
        public string Redirect { get; set; }
        public string Status { get; set; }
        public string Agreements { get; set; }
        public object[] Accounts { get; set; }
        public string Reference { get; set; }
        public string User_language { get; set; }
        public string Link { get; set; }
    }

    public class BankLinkAuthorizationStatusModel
    {
        public string Short { get; set; }
        public string Long { get; set; }
        public string Description { get; set; }
    }


}