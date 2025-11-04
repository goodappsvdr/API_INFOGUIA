using System.Security.Claims;

namespace Api.Infrastructure.Jwt
{
    public static class Jwt_Helpers
    {
        public static IEnumerable<Claim> GetClaimsByRefreshToken(this Jwt_Claims user)
        {
            return new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Expiration, DateTimeOffset.UtcNow.AddDays(30).ToUnixTimeSeconds().ToString())
            };

        }
        public static IEnumerable<Claim> GetClaimsByAccessToken(this Jwt_Claims user)
        {
            List<Claim> Claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Actor, user.Username.ToString()),
                new(ClaimTypes.Email, user.Email.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Expiration, DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds().ToString())
            };
            
            return Claims;
        }
        public static Jwt_Tokens GetAccessTokens(Jwt_Tokens jwtUser, Jwt_Claims user, Jwt_AccessTokenSettings accessTokenSettings)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // Generar token de acceso
            var accessTokenKey = Encoding.ASCII.GetBytes(accessTokenSettings.IssuerSigningKey);

            var jwtAccessToken = new JwtSecurityToken(
                issuer: accessTokenSettings.ValidIssuer,
                audience: accessTokenSettings.ValidAudience,
                claims: GetClaimsByAccessToken(user),
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.UtcNow.AddMinutes(30)).DateTime,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(accessTokenKey),
                    SecurityAlgorithms.HmacSha256));

            jwtUser.AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtAccessToken);
            jwtUser.ExpiredTime = new DateTimeOffset(DateTime.UtcNow.AddMinutes(30)).DateTime;

            return jwtUser;
        }
        public static string GetRefreshTokens(Jwt_Claims user, Jwt_RefreshTokenSettings refreshTokenSettings, HttpContext httpContext)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // Generar token de refresco
            var refreshTokenKey = Encoding.ASCII.GetBytes(refreshTokenSettings.IssuerSigningKey);

            // Generar token de refresco
            var jwtRefreshToken = new JwtSecurityToken(
               issuer: refreshTokenSettings.ValidIssuer,
               audience: refreshTokenSettings.ValidAudience,
               claims: GetClaimsByRefreshToken(user),
               notBefore: new DateTimeOffset(DateTime.Now).DateTime,
               expires: new DateTimeOffset(DateTime.UtcNow.AddDays(30)).DateTime,
               signingCredentials: new SigningCredentials(
                   new SymmetricSecurityKey(refreshTokenKey),
                   SecurityAlgorithms.HmacSha256));

            var RefreshToken = new JwtSecurityTokenHandler().WriteToken(jwtRefreshToken);


            httpContext.Response.Cookies.Append("RefreshToken", RefreshToken, new CookieOptions
            {
                Expires = new DateTimeOffset(DateTime.UtcNow.AddDays(30)).DateTime,
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            });

            return RefreshToken;
        }
        public static Jwt_Claims GetClaimsByToken(string Token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(Token);
            Jwt_Claims claims = new();
            foreach (var claim in jwtToken.Claims)
            {
                switch (claim.Type)
                {
                    case ClaimTypes.NameIdentifier:
                        claims.UserId = claim.Value;
                        break;
                    case ClaimTypes.Actor:
                        claims.Username = claim.Value;
                        break;
                    case ClaimTypes.Email:
                        claims.Email = claim.Value;
                        break;
                }
            }
            return claims;
        }
        public static string GetDateExpirationByToken(string Token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(Token);
            return jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/expiration")?.Value;
        }
    }
}
