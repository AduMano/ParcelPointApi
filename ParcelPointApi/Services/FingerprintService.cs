using ParcelPointApi.Helpers;
using ParcelPointApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FingerprintService : IFingerprintService
{
    private readonly ParcelPointDbContext _context;

    public FingerprintService(ParcelPointDbContext context)
    {
        _context = context;
    }

    // Store fingerprint template in the database
    public async Task<bool> SaveFingerprintAsync(UserbioFp fingerprint)
    {
        _context.UserbioFps.Add(fingerprint);
        await _context.SaveChangesAsync();
        return true;
    }

    private static bool CompareFingerprints(byte[] scanned, byte[] stored, double similarityThreshold = 0.85)
    {
        if (scanned.Length != stored.Length)
        {
            Console.WriteLine("Fingerprint sizes do not match.");
            return false;
        }

        int totalBytes = scanned.Length;
        int differentBytes = 0;

        for (int i = 0; i < totalBytes; i++)
        {
            if (scanned[i] != stored[i])
            {
                differentBytes++;
            }
        }

        double similarity = 1.0 - ((double)differentBytes / totalBytes); // Normalize similarity (0.0 - 1.0)
        Console.WriteLine($"Similarity Score: {similarity * 100}%");

        return similarity >= similarityThreshold;
    }

    // Retrieve the stored fingerprint template for a given user
    public async Task<UserbioFp?> GetStoredFingerprintAsync(byte[] scannedFingerprint)
    {
        var storedFingerprints = _context.UserbioFps.ToList();

        foreach (var fingerprint in storedFingerprints)
        {
            byte[] storedData = fingerprint.FingerprintData;

            // Decrypt stored fingerprint template before sending it back
            if (!string.IsNullOrEmpty(fingerprint.FingerprintKey))
            {
                storedData = AESHelper.DecryptAES(storedData, fingerprint.FingerprintKey);
            }

            // Compare fingerprints using similarity threshold (90%)
            if (CompareFingerprints(scannedFingerprint, storedData, 0.20)) // 90% similarity threshold
            {
                return fingerprint;
            }

            // Instead of matching here, we send the stored template to the microcontroller
            return fingerprint;
        }

        return null; // No match found
    }
}
