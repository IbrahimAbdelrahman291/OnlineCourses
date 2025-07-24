using BAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BAL.GenericRepository
{
    public class TokenServices : ITokenServices
    {
        private readonly IConfiguration _configuration;

        public TokenServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsyync(AppUser user, UserManager<AppUser> userManager)
        {
            var Authclaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,user.UserName!),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.DisplayName)
            };
            var Roles = await userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                Authclaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));

            var Token = new JwtSecurityToken
                (
                issuer: _configuration["JWT:ValidIssuer"], 
                audience: _configuration["JWT:Validaud"], 
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:ExpiresToken"]!)),
                claims: Authclaims,
                signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
