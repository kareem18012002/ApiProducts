using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NoRiskNoFun.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NoRiskNoFun.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(JwtOptionscs jwtOptionscs , ApplicationDbContext dbContext) : ControllerBase
    {
        [HttpPost]
        [Route("auth")]
        public IActionResult Authenticate(AuthenticationRequest request)
        {
            var user = dbContext.Set<User>().FirstOrDefault(u => u.Name == request.Username && u.Password == request.Password);
            if (user == null) {
                return Unauthorized();
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptionscs.Issuer,
                Audience = jwtOptionscs.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptionscs.SigningKey)),
                SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, "K@123.Com"),
                }
                )
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accesstoken = tokenHandler.WriteToken(securityToken);

            return Ok(new { AccessToken = accesstoken });
        }
    }
}