using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TandheelkundigCentrum.Data;
using TandheelkundigCentrum.Data.Base;
using TandheelkundigCentrum.Data.Models;
using TandheelkundigCentrum.Utilities;

namespace TandheelkundigCentrum.Services;

public class AuthService(ApplicationDbContext context) : BaseRepository<User, Guid>(context)
{
    /// <summary>
    /// Get a user by its email
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <returns>The user with the given email or null.</returns>
    public async Task<User?> GetUserByEmail(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }


    /// <summary>
    /// Gets the user by its email and return it if the password match.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="password">The password to match.</param>
    /// <returns>The user where the email and password match.</returns>
    public async Task<User?> GetUserByEmailAndPassword(string email, string password)
    {
        var user = await GetUserByEmail(email);
        if (user == null || !PasswordHasher.VerifyPassword(password, user.Password))
            return null;
        return user;
    }


    private static readonly byte[] secretKey = "A-VERY-SAFE-SECRET-KEY"u8.ToArray();
    private JwtSecurityTokenHandler tokenHandler = new();

    /// <summary>
    /// Generate a JWT token that holds user claims.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public string GenerateToken(User user)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Validate a JWT token.
    /// </summary>
    /// <param name="token">The token to validate.</param>
    /// <returns>True if the token is valid.</returns>
    public bool ValidateToken(string? token)
    {
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return true;
        }
        catch (SecurityTokenException) { return false; }
    }

    /// <summary>
    /// Get the user contained in the JWT token.
    /// </summary>
    /// <param name="token">The token to get the user of.</param>
    /// <returns>A user if token contained one else null</returns>
    public async Task<User?> GetUserByToken(string? token)
    {
        var id = GetUserIdByToken(token);
        if (id == null) return null;
        return await GetByIdAsync(Guid.Parse(id));
    }
    
    private string? GetUserIdByToken(string? token)
    {
        if (!ValidateToken(token)) return null;
        var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        var nameClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "unique_name");
        return nameClaim?.Value;
    }
}