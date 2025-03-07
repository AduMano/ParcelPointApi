// DTO for Fingerprint Data

namespace ParcelPointApi.Data.Interface.Biometrics
{
    public class FingerprintRequestDto
    {
        public Guid UserId { get; set; }
        public string FingerprintData { get; set; } // Base64 string of the fingerprint template
    }
}
