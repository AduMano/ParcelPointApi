namespace ParcelPointApi.Data.Interface.UserGroup
{
    public interface IDeleteMemberRequestDto
    {
        Guid[] Members { get; set; }
        Guid GroupOwnerId { get; set; }
    }
    public class DeleteMemberRequestDto: IDeleteMemberRequestDto
    {
        public Guid[] Members { get; set; }
        public Guid GroupOwnerId { get; set; }
    }
}
