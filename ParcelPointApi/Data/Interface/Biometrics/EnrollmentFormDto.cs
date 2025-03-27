namespace ParcelPointApi.Data.Interface.Biometrics
{
    public interface IEnrollmentFormDto
    {
        String FirstName { get; set; }
        String MiddleName { get; set; }
        String LastName { get; set; }
        String Suffix { get; set; }
        DateTime BirthDate { get; set; }
        String Gender { get; set; }
        String Address { get; set; }
        String ContactNumber { get; set; }
        String Email { get; set; }
        String Username { get; set; }
        String Password { get; set; }
        Guid OperatorID { get; set; }
    }
    public class EnrollmentFormDto : IEnrollmentFormDto
    {
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public String Suffix { get; set; }
        public String Gender { get; set; }
        public String Address { get; set; }
        public String ContactNumber { get; set; }
        public String Email { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public Guid OperatorID { get; set; }
    }
}
