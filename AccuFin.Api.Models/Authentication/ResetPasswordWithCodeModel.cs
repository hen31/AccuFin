namespace AccuFin.Api.Models.Authentication
{
    public class ResetPasswordWithCodeModel
    {
        public string Email { get; set; }
        public string ResetCode { get; set; }
        public string Password { get; set; }
    }
}
