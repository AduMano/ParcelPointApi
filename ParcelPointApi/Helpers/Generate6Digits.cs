namespace ParcelPointApi.Helpers
{
    public class Generate6Digits
    {
        public static string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999) + ""; // Generates a number between 100000 and 999999
        }
    }
}
