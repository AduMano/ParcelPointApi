using ParcelPointApi.Models;

public interface IFingerprintService
{
    Task<bool> SaveFingerprintAsync(UserbioFp fingerprint);
    Task<UserbioFp?> GetStoredFingerprintAsync(byte[] scannedFingerprint);
}
