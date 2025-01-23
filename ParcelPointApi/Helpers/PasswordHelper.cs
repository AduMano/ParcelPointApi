using Microsoft.AspNetCore.Identity;

public class PasswordHelper
{
    private readonly PasswordHasher<object> _passwordHasher;

    public PasswordHelper()
    {
        _passwordHasher = new PasswordHasher<object>();
    }

    // Method to hash the password
    public string HashPassword(string plainPassword)
    {
        if (string.IsNullOrEmpty(plainPassword))
            throw new ArgumentNullException(nameof(plainPassword), "Password cannot be null or empty.");

        return _passwordHasher.HashPassword(null, plainPassword);  
    }

    // Method to validate the password
    public bool ValidatePassword(string plainPassword, string hashedPassword)
    {
        if (string.IsNullOrEmpty(plainPassword) || string.IsNullOrEmpty(hashedPassword))
            throw new ArgumentNullException("Password and hashed password cannot be null or empty.");

        var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, plainPassword);

        return result == PasswordVerificationResult.Success;
    }
}
