using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Net7Kubernetes.Models;

namespace Net7Kubernetes.Token
{
    public class JwtGenerator : IJwtGenerator
    {
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName!),
                new Claim("UserId", user.Id),
                new Claim("Email", user.Email!)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("wsdUogRrRziw1GSOoU4y6W2oyPPmxKXK"));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }
    }
}