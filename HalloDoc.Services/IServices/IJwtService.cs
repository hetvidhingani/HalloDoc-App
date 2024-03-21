using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Services.IServices
{
    public interface  IJwtService
    {
        string GenerateJwtToken(AspNetUser user);

        public dynamic GetTokenData(string token);
        bool ValidateToken(string token, out JwtSecurityToken jwtSecurityToken);
    }
}
