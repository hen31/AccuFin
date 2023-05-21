namespace AccuFin.Data.Entities
{
    public class UserAdministrationLink : BaseEntityGuidId
    {
        public Guid UserId { get; set; }
        public AuthorizedUser User { get; set; }

        public Guid AdministrationId { get; set; }
        public Administration Administration { get; set; }

        public UserRoleInAdministration Roles { get; set; }

    }
}
