using HalloDoc.Entities.DataModels;
using HalloDoc.Services.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Services.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext _context;
        public JwtService(IConfiguration configuration, ApplicationDbContext context)
        {
            this.configuration = configuration;
            _context = context;
        }
        public string GenerateJwtToken(AspNetUser user)
        {
            var userRole = _context.AspNetUserRoles.FirstOrDefault(u=>u.UserId==user.Id);
            var roleId = userRole.RoleId;
            //var AdminID = _context.Admins.FirstOrDefault(u=>u.AspNetUserId==user.Id);
            //int adminID = AdminID.AdminId;

            var claims = new List<Claim>()
            {
                 new Claim(ClaimTypes.Email, user.Email),
                 new Claim(ClaimTypes.Role, roleId),
                 new Claim("userId",user.Id),
                 //new Claim("AdminID",adminID.ToString())
                
             };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires =
                DateTime.Now.AddMinutes(120);

            var token = new JwtSecurityToken(
               configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);


        }
        public dynamic GetTokenData(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken.Claims.First(claim => claim.Type == "userId").Value;
         
        }

        public bool ValidateToken(string token, out JwtSecurityToken jwtSecurityToken)
        {
            jwtSecurityToken = null;

            if (token == null)
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero

                }, out SecurityToken validatedToken);


                jwtSecurityToken = (JwtSecurityToken)validatedToken;
                if (jwtSecurityToken != null)
                    return true;

                return false;
            }
            catch
            {
                return false;
            }

        }
    }
}

