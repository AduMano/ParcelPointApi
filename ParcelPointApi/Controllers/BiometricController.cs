using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Threading.Tasks;
using ParcelPointApi.Helpers;
using ParcelPointApi.Models;
using ParcelPointApi.Data.Interface.Biometrics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

[Route("api/Biometric")]
[ApiController]
public class BiometricController : ControllerBase
{
    private readonly IFingerprintService _fingerprintService;
    private readonly ParcelPointDbContext _context;
    private readonly PasswordHelper _passwordHelper;

    public BiometricController(IFingerprintService fingerprintService, ParcelPointDbContext db, PasswordHelper ph)
    {
        _fingerprintService = fingerprintService;
        _context = db;
        _passwordHelper = ph;
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

    [HttpPost("RegisterUser")]
    public async Task<IActionResult> EnrollUsers( [FromBody] EnrollmentFormDto enrollUserForm )
    {
        try
        {
            var userID = new Guid();

            // Make User First
            var user = new User
            {
                Username = enrollUserForm.Username,
                Password = _passwordHelper.HashPassword(enrollUserForm.Password),
                CreatedAt = DateTime.Now,
                CreatedBy = enrollUserForm.OperatorID,
                IsActive = true,
                Id = userID,
                RoleId = await _context.Roles.Where(r => r.Name == "Users").Select(r => r.Id).FirstOrDefaultAsync()
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            userID = user.Id;

            // Then Make User Information
            var userInfo = new UserInformation
            {
                Id = new Guid(),
                FirstName = enrollUserForm.FirstName,
                MiddleName = enrollUserForm.MiddleName,
                LastName = enrollUserForm.LastName,
                Address = enrollUserForm.Address,
                Birthdate = DateOnly.FromDateTime(enrollUserForm.BirthDate),
                ContactNumber = enrollUserForm.ContactNumber,
                CreatedAt = DateTime.Now,
                CreatedBy = enrollUserForm.OperatorID,
                Email = enrollUserForm.Email,
                GenderId = await _context.Genders.Where(g => g.Name == enrollUserForm.Gender).Select(g => g.Id).FirstOrDefaultAsync(),
                PhotoUrl = "",
                Suffix = enrollUserForm.Suffix,
                UserId = userID
            };

            await _context.UserInformations.AddAsync(userInfo);

            // Then Make Biometrics ID
            await _context.Database.ExecuteSqlRawAsync(
                "INSERT INTO USERBIO_TEMP (id, bio_id, owner_id, created_at, created_by) VALUES (@Id, @BioId, @OwnerId, @CreatedAt, @CreatedBy)",
                new SqlParameter("@Id", Guid.NewGuid()),
                new SqlParameter("@BioId", await _context.SystemModes.Select(bio => bio.BiometricId).FirstOrDefaultAsync()),
                new SqlParameter("@OwnerId", userID),
                new SqlParameter("@CreatedAt", DateTime.Now),
                new SqlParameter("@CreatedBy", enrollUserForm.OperatorID)
            );

            // Make Group
            var newGroup = new UserGroup
            {
                Id = new Guid(),
                CreatedAt = DateTime.Now,
                CreatedBy = enrollUserForm.OperatorID,
                OwnerId = user.Id
            };

            await _context.UserGroups.AddAsync(newGroup);

            // Make Logs
            // Insert User Logs
            var log = new ActivityLog
            {
                ActionTitle = "Admin Registered a User",
                ActionContext = $"Admin Just created an account for {user.Username}",
                CreatedAt = DateTime.Now,
                CreatedBy = user.Id,
                Module = "Utilities",
                SubModule = "User Logs"
            };


            // Then Save
            await _context.ActivityLogs.AddAsync(log);

            await _context.Database.ExecuteSqlRawAsync("UPDATE system_mode SET biometric_id = 0, current_state = 'scanning'");

            await _context.SaveChangesAsync();

            return Ok("Successfully Registered!");

        }
        catch (Exception er)
        {
            return StatusCode(500, $"Error on enrolling a user: {er.Message}");
        }
    }
}
