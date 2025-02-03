namespace ParcelPointApi.Data.Interface.UserGroup
{
    public interface IUpdateMember
    {
        Guid GroupMemberId { get; set; }
        bool IsAuthorized { get; set; }
        Guid RelationshipId { get; set; }
    }

    public class UpdateMemberDto : IUpdateMember
    {
        public Guid GroupMemberId { get; set; }
        public bool IsAuthorized { get; set; }
        public Guid RelationshipId { get; set; }
    }

    public class UpdateMemberCollectionDto
    {
        public List<UpdateMemberDto> Members { get; set; }
    }

    public class MemberResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}