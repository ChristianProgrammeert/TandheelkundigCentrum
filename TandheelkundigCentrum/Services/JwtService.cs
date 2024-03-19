using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TandheelkundigCentrum.Data.Models;

namespace TandheelkundigCentrum.Services;

public class JwtService
{
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
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.Fullname),
                .. user.Groups.Select(g => new Claim(ClaimTypes.Role, g.Name.ToString())).ToList(),
            ]),
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
        if (token == null) return false;
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
        catch (SecurityTokenException)
        {
            return false;
        }
    }


    public ClaimsPrincipal GetClaimsIdentity(string? token)
    {
        var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        var identity = new ClaimsIdentity(jsonToken?.Claims, "Bearer");
        return new ClaimsPrincipal(identity);
    }

    public string? GetUserId(string? token)
    {
        if (!ValidateToken(token)) return null;
        var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        var nameClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "unique_name");
        return nameClaim?.Value;
    }

    public string? GetUserEmail(string? token)
    {
        if (!ValidateToken(token)) return null;
        var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        var nameClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "email");
        return nameClaim?.Value;
    }

    public string? GetUsername(string? token)
    {
        if (!ValidateToken(token)) return null;
        var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        var nameClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "given_name");
        return nameClaim?.Value;
    }

    public IEnumerable<Group.GroupName> GetUserGroups(string? token)
    {
        if (!ValidateToken(token)) return [];
        var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        var nameClaim = jsonToken?.Claims.Where(c => c.Type == "role");
        return nameClaim?.Select(
            claim => Enum.Parse<Group.GroupName>(claim.Value)
        ) ?? [];
    }
}