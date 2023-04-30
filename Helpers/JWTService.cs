using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace server.Helpers
{
    public class JWTService
    {
        private static string secureKey = "this is not a very secure key for a jwt";

        public static string Generate(string id)
        {
            var ssk = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var credentials = new SigningCredentials(ssk, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);
            var payload = new JwtPayload(id, null, null, null, DateTime.Today.AddDays(1));
            var jwt = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static JwtSecurityToken Verify(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secureKey);

            tokenHandler.ValidateToken(jwt, new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,

            }, out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }
    }
}