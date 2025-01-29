using Microsoft.EntityFrameworkCore;
using OtpNet;
using ProjectName.DataAccess.DatabaseContext;
using ProjectName.DataAccess.Entities;
using QRCoder;

namespace ProjectName.DataAccess.Repository;

public class AuthenticationService(IDbContextFactory<ProjectNameContext> context)
{
    private const string Issuer = "Projectname";

    // Generate secret key for user
    public string GenerateSecretKey()
    {
        var key = KeyGeneration.GenerateRandomKey(20);
        return Base32Encoding.ToString(key);
    }

    // Generate the otpauth URI for Google Authenticator
    public string GenerateQrCodeUri(string userEmail, string secretKey)
    {
        return $"otpauth://totp/{Issuer}:{userEmail}?secret={secretKey}&issuer={Issuer}&digits=6";
    }

    // Generate a base64 image of the QR code
    public string GenerateQrCodeImage(string qrCodeUri)
    {
        using QRCodeGenerator qrGenerator = new QRCodeGenerator();
        using QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeUri, QRCodeGenerator.ECCLevel.Q);
        using Base64QRCode qrCode = new Base64QRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }

    // Store the secret key for the user
    public async Task EnableTwoFactorAuthenticationAsync(string userId, string secretKey)
    {
        await using ProjectNameContext context1 = await context.CreateDbContextAsync();
        var user = await context1.Users.FirstOrDefaultAsync(x => x.Email == userId);
        if (user != null)
        {
            user.SecretKey = secretKey;
            await context1.SaveChangesAsync();
        }
    }

    // Retrieve the secret key for the user
    public async Task<string> GetUserSecretKeyAsync(string userId)
    {
        await using ProjectNameContext context1 = await context.CreateDbContextAsync();
        var user = await context1.Users.FirstAsync(x => x.Email == userId);
        return user.SecretKey;
    }

    // Validate the OTP
    public bool ValidateOtpCode(string userInputCode, string secretKey)
    {
        // Check if the input code is null, empty, or not the expected length (usually 6 digits)
        if (string.IsNullOrEmpty(userInputCode) || userInputCode.Length != 6 || !userInputCode.All(char.IsDigit))
        {
            return false;
        }

        try
        {
            // Generate a TOTP object with the secret key
            var totp = new Totp(Base32Encoding.ToBytes(secretKey));

            // Verify the OTP code
            return totp.VerifyTotp(userInputCode, out _, new VerificationWindow(2, 2));
        }
        catch (Exception)
        {
            return false;
        }
    }
}