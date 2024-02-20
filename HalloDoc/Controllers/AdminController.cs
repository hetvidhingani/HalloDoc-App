using HalloDoc.Entities.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }

        #region Login
        public IActionResult AdminLogin()
        {
            if (HttpContext.Session.GetInt32("AdminSession") != null)
            {

                return RedirectToAction("DashBoard");
            }
            return View();
        }
        [HttpPost]
        public IActionResult AdminLogin(AspNetUser user)
        {
            var myUser = _context.AspNetUsers.Where(x => x.Email == user.Email && x.PasswordHash == user.PasswordHash).FirstOrDefault();

            if (myUser != null)
            {
                HttpContext.Session.SetString("UserName", myUser.UserName);
                User uID = _context.Users.Where(x => x.Email == myUser.Email).FirstOrDefault();
                HttpContext.Session.SetInt32("AdminSession", uID.UserId);


                return RedirectToAction("DashBoard");
            }
            else
            {
                ViewBag.Message = "Login Failed!";
            }

            return View();
        }
        #endregion
        #region Logout
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                HttpContext.Session.Remove("AdminSession");
                return RedirectToAction("AdminLogin");
            }
            return View();
        }


        #endregion
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
