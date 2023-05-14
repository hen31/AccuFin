namespace AccuFin.Api.Models.Authentication
{
    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public bool ChangePassword { get; set; }
    }
}
