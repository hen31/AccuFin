using System.ComponentModel.DataAnnotations.Schema;

namespace AccuFin.Data.Entities
{
    public class UserAdministrationLink : BaseEntityGuidId
    {
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public AuthorizedUser User { get; set; }

        [ForeignKey(nameof(Administration))]
        public Guid AdministrationId { get; set; }
        public Administration Administration { get; set; }

        public UserRoleInAdministration Roles { get; set; }

    }
}
