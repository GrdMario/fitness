namespace Fitness.Infrastructure.Adapter.Security.Internal
{
    using Fitness.Application.Contracts.Security;
    using Fitness.Blocks.Common.Kernel;
    using Fitness.Domain;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Claim = System.Security.Claims.Claim;

    internal sealed class TokenService : ITokenService
    {
        private readonly SecurityAdapterSettings settings;

        public TokenService(IOptions<SecurityAdapterSettings> settings)
        {
            this.settings = settings.Value;
        }

        public string GenerateAccessToken(User user)
        {
            var issuer = this.settings.TokenSettings.Issuer;
            var audience = this.settings.TokenSettings.Audience;
            var nbf = SystemClock.Now.DateTime;
            var expires = SystemClock.Now.AddMinutes(this.settings.TokenSettings.ExpiresInMinutes).DateTime;
            var key = Encoding.UTF8.GetBytes(this.settings.TokenSettings.Secret);
            var signInCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, SystemGuid.NewGuid.ToString()),
                new Claim(ClaimTypes.Role, "test-role")
            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                nbf,
                expires,
                signInCredentials);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }

        public RefreshToken GenerateRefreshToken(User user)
        {
            var token = new RefreshToken(
                SystemGuid.NewGuid,
                user.Id,
                SystemClock.UtcNow.AddDays(30),
                SystemClock.UtcNow);

            return token;
        }
    }
}
