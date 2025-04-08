// TMFApplication/Services/Authentication/JwtTokenService.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TMFDomain.Entities.Authentication;
using TMFDomain.Interfaces;
using TMFDomain.Interfaces.Staff;

namespace TMFApplication.Services.Authentication
{
    public interface IJwtTokenService
    {
        Task<string> GenerateAccessTokenAsync(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal? ValidateRefreshToken(string refreshToken);
        int RefreshTokenExpiryDays { get; }
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IRoleRepository _roleRepository;

        public JwtTokenService(
            IConfiguration configuration,
            IRoleRepository roleRepository)
        {
            _configuration = configuration;
            _roleRepository = roleRepository;
        }

        public int RefreshTokenExpiryDays =>
            _configuration.GetValue<int>("JwtSettings:RefreshTokenExpiryDays", 7);

        public async Task<string> GenerateAccessTokenAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Token"]);

            var role = await _roleRepository.GetByRoleIdAsync(user.role_code);
            var privileges = role?.Privileges?.Select(p => p.Name) ?? new List<string>();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.staff_code),
                new Claim(ClaimTypes.Email, user.email ?? string.Empty),
                new Claim(ClaimTypes.Role, user.role_code ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var privilege in privileges)
            {
                claims.Add(new Claim("privilege", privilege));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSettings:AccessTokenExpiryMinutes", 15)),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal? ValidateRefreshToken(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Token"]);

            try
            {
                var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}