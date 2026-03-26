using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ResearchProjectManagementSystem.Models;
using ResearchProjectManagementSystem.Payloads;
using ResearchProjectManagementSystem.Services;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace ResearchProjectManagementSystem.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class LoginController: ControllerBase
    {
        private IConfiguration _config;
        private ResearchProjectsContext _db;

        public LoginController(IConfiguration config, ResearchProjectsContext db)
        {
            _config = config;
            _db = db;
        }

        [HttpPost("[action]")]
        public IActionResult LoginUser([FromBody] LoginPayload payload)
        {
            if (payload == null)
            {
                return BadRequest();
            }

            UserModel? user = _db.Users.SingleOrDefault(u => u.EmailAddress == payload.Email);
            if (user == null)
            {
                return Forbid();
            }

            string passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: payload.Password,
                salt: Convert.FromBase64String(user.Salt),
                KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32
            ));
            if (passwordHash == user.PasswordHash)
            {
                return Ok(new { token = GenerateJsonWebSecurityToken(user) });
            }

            return Forbid();
        }

        private string GenerateJsonWebSecurityToken(UserModel user)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("role", user.UserRole));
            claims.Add(new Claim("displayName", user.DisplayName));
            claims.Add(new Claim("userId", user.IdUser.ToString()));

            var tokenOptions = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }
    }
}
