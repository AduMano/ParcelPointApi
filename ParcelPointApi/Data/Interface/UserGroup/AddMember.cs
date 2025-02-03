namespace ParcelPointApi.Data.Interface.UserGroup
{
    public interface IAddMember
    {
        Guid MemberId { get; set; }

        Guid RelationshipId { get; set; }

        bool IsAuthorized { get; set; }

        Guid CreatedBy { get; set; }
    }

    public class AddMemberDto : IAddMember
    {
        public Guid MemberId { get; set; }

        public Guid RelationshipId { get; set; }

        public bool IsAuthorized { get; set; }

        public Guid CreatedBy { get; set; }
    }
}
