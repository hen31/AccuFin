namespace AccuFin.Api.Services.BankIntegration
{
    internal class SecretResult
    {
        public string Access { get; set; }
        public int Access_expires { get; set; }
        public string Refresh { get; set; }
        public int Refresh_expires { get; set; }
    }
}