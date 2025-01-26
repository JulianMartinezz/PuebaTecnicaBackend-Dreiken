using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HRMedicalRecordsManagement.Helpers;
using HRMedicalRecordsManagement.Models.BaseResponse;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;

namespace HRMedicalRecordsManagement.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _configuration;

    public AuthenticationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<BaseResponse<string>> AuthenticateAsync(LoginRequest loginRequest)
    {
        // Validate credentials. Hardcoded for simplicity
        if (loginRequest.Email != "admin@gmail.com" || loginRequest.Password != "password")
        {
            return ResponseHelper.BadRequest<string>("Invalid email or password");
        }

        var token = GenerateJwtToken();

        return ResponseHelper.Success(token, "Authentication successful");
    }

    private string GenerateJwtToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "admin")}),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}