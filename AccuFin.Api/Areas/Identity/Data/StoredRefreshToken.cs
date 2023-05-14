namespace AccuFin.Api.Areas.Identity.Data
{
    public class StoredRefreshToken
    {
        public ulong Id { get; set; }
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Revoked { get; set; }
        public string ClientId { get; set; }
    }
}
