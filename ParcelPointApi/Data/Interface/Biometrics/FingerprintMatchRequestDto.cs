namespace ParcelPointApi.Data.Interface.Biometrics
{

    // DTO for Fingerprint Matching Request
    public class FingerprintMatchRequestDto
    {
        public string FingerprintData { get; set; } // Base64 encoded fingerprint template
    }
}
