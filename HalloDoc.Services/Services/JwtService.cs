﻿using HalloDoc.Entities.DataModels;
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
        //public string GenerateJwtToken(string Email, string AspNetUserId,string FirstName, string LastName, string Id, bool IsAdmin, bool IsPhysician, bool IsPatient)
        //{
        //    var claims = new List<Claim>()
        //    {
        //         new Claim(ClaimTypes.Email, Email),
        //        new Claim("AspNetUserId", AspNetUserId),
        //        new Claim("UserId", Id),
        //        new Claim("UserName",FirstName+" "+LastName)
        //     };

        //    if (IsAdmin)
        //    {
        //        {
        //            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        //        };
        //    }
        //    if (IsPhysician)
        //    {
        //        {
        //            claims.Add(new Claim(ClaimTypes.Role, "Physician"));
        //        };
        //    }
        //    if (IsPatient)
        //    {
        //        {
        //            claims.Add(new Claim(ClaimTypes.Role, "Patient"));
        //        };
        //    }


        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var expires =
        //        DateTime.Now.AddMinutes(20);

        //    var token = new JwtSecurityToken(
        //       configuration["Jwt:Issuer"],
        //        configuration["Jwt:Audience"],
        //        claims,
        //        expires: expires,
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);


        //}
        public string GenerateJwtToken(AspNetUser user)
        {
            var userRole = _context.AspNetUserRoles.FirstOrDefault(u=>u.UserId==user.Id);
            var roleId = userRole.RoleId;
            var claims = new List<Claim>()
            {
                 new Claim(ClaimTypes.Email, user.Email),
                 new Claim(ClaimTypes.Role, roleId),
                 new Claim("userId",user.Id),
              
             };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires =
                DateTime.Now.AddMinutes(20);

            var token = new JwtSecurityToken(
               configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);


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

