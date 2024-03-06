using HalloDoc.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class CustomAuthorize : Attribute, IAuthorizationFilter
    {
        private readonly string _role;
        public CustomAuthorize(string role ="")
        {
            _role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var jwtService = context.HttpContext.RequestServices.GetService<Services.IServices.IJwtService>();
            if (jwtService == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Custom", action = "AdminLogin" }));
                return;
            }
            var request = context.HttpContext.Request;
            var token = request.Cookies["jwt"];
            if (token == null || !jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Custom", action = "AdminLogin" }));
                return;
            }
            var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);
            if (roleClaim == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Custom", action = "AdminLogin" }));
                return;
            }

            if (string.IsNullOrWhiteSpace(_role) || roleClaim.Value != _role)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Custom", action = "AdminLogin" }));
                return;
            }
        }
    }
}
