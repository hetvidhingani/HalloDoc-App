using HalloDoc.Entities.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class RoleAuthorize : Attribute, IAuthorizationFilter
    {
        private readonly int _menuId;
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        public RoleAuthorize(int menuId = 0)
        {
            _menuId = menuId;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;
            var roleCookie = request.Cookies["RoleMenu"];

            var Role = _context.RoleMenus.Where(u => u.RoleId == Int32.Parse(roleCookie!)).ToList();
            bool flag = false;

            if(Role.Any(u => u.MenuId == _menuId))
            {
                flag = true;
            }
            if(flag == false)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Admin", action = "RoleAuthorization" }));
                return;
            }
        }
    }
}
