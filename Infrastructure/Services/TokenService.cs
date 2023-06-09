using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public TokenService(IConfiguration config, UserManager<User> userManager,  RoleManager<IdentityRole> roleManager)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> CreateToken(User user)
        {

            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(_key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
