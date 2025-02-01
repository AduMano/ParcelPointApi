namespace ParcelPointApi.Data.Interface.UserGroup
{
    public interface IMemberUpdateAuthorization
    {
        Guid Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Address { get; set; }
        string Username { get; set; }
        DateOnly? BirthDate { get; set; }
    }
    public class MemberUpdateAuthorizationDTO : IMemberUpdateAuthorization
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public DateOnly? BirthDate { get; set; }
    }

}
