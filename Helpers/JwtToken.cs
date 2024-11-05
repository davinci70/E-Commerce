using e_commerce.Models.Auth;
using e_commerce.Models.Users;
using e_commerce.Services.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace e_commerce.Helpers
{
    public class JwtToken
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly JWT _jwt;

        //public JwtToken(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt)
        //{
        //    _userManager = userManager;
        //    _jwt = jwt.Value;
        //}

        public static async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser User, UserManager<ApplicationUser> _userManager, JWT _jwt)
        {
            var userClaims = await _userManager.GetClaimsAsync(User);
            var roles = await _userManager.GetRolesAsync(User);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, User.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, User.Email),
                new Claim("uid", User.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
            signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}
