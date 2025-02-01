namespace ParcelPointApi.Data.Interface.UserGroup
{
    public interface IUpdateMember
    {
        Guid GroupId { get; set; }
        bool IsAuthorized { get; set; }
        Guid RelationshipId { get; set; }
    }
    public class UpdateMemberDto : IUpdateMember
    {
        public Guid GroupId { get; set; }
        public bool IsAuthorized { get; set; }
        public Guid RelationshipId { get; set; }
    }

}
