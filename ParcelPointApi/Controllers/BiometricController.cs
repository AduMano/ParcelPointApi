using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Threading.Tasks;
using ParcelPointApi.Helpers;
using ParcelPointApi.Models;
using ParcelPointApi.Data.Interface.Biometrics;

[Route("api/Biometric")]
[ApiController]
public class BiometricController : ControllerBase
{
    private readonly IFingerprintService _fingerprintService;

    public BiometricController(IFingerprintService fingerprintService)
    {
        _fingerprintService = fingerprintService;
    }

    // 1️⃣ Register a fingerprint in the database
    [HttpPost("RegisterFingerprint")]
    public async Task<IActionResult> RegisterFingerprint([FromBody] FingerprintRequestDto request)
    {
        try
        {
            byte[] fingerprintTemplate = Convert.FromBase64String(request.FingerprintData);

            // Encrypt fingerprint template before storing (for security)
            string encryptionKey = "0123456789abcdef0123456789abcdef"; // Use a secure key
            byte[] encryptedData = AESHelper.EncryptAES(fingerprintTemplate, encryptionKey);

            // Create fingerprint record
            var userBio = new UserbioFp
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                FingerprintData = encryptedData, // Store encrypted template
                FingerprintKey = encryptionKey, // Store encryption key (can be omitted)
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.UserId
            };

            await _fingerprintService.SaveFingerprintAsync(userBio);
            return Ok("Fingerprint registered successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error registering fingerprint: {ex.Message}");
        }
    }

    // 2️⃣ Retrieve a stored fingerprint template for matching (sent to microcontroller)
    [HttpPost("MatchFingerprint")]
    public async Task<IActionResult> MatchFingerprint([FromBody] FingerprintMatchRequestDto request)
    {
        try
        {
            byte[] scannedFingerprint = Convert.FromBase64String(request.FingerprintData);

            // Retrieve stored fingerprint (without matching in API)
            var matchedFingerprint = await _fingerprintService.GetStoredFingerprintAsync(scannedFingerprint);

            if (matchedFingerprint != null)
            {
                // Convert stored fingerprint to Base64 to send to the microcontroller
                string encodedFingerprint = Convert.ToBase64String(matchedFingerprint.FingerprintData);

                return Ok(new
                {
                    UserId = matchedFingerprint.UserId,
                    Message = "Send this fingerprint template to the microcontroller for matching.",
                    FingerprintTemplate = encodedFingerprint
                });
            }

            return StatusCode(404, "No matching fingerprint found.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error during fingerprint matching: {ex.Message}");
        }
    }
}
