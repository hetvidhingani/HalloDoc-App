using HalloDoc.Entities.DataModels;
using HalloDoc.Services.IServices;
using HalloDoc.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class CustomController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IPatientService _patient;
        private IAdminService _admin;
        private IJwtService _jwtService;


        public CustomController(ApplicationDbContext context, IPatientService patient, IAdminService admin, IJwtService jwtService)
        {
            _context = context;
            _patient = patient;
            _admin = admin;
            _jwtService = jwtService;

        }
        #region Admin Login
        public IActionResult AdminLogin()
        {
            
            return View("~/Views/Admin/AdminLogin.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> AdminLogin(AspNetUser user)
        {
            AspNetUser myUser = await _admin.checkEmailPassword(user);

            if (myUser != null)
            {
                Admin userID = await _admin.GetAdmin(myUser.Email);
                var jwtToken = _jwtService.GenerateJwtToken(myUser.Email, myUser.Id, userID.FirstName, userID.LastName, Convert.ToString(myUser.Id), true, false, false);
                Response.Cookies.Append("jwt", jwtToken);
                HttpContext.Session.SetString("UserName", myUser.UserName);

                HttpContext.Session.SetInt32("AdminSession", userID.AdminId);
                return RedirectToAction("Dashboard","Admin");
            }
            else
            {
                ViewBag.Message = "Login Failed!";
            }
            return View("~/Views/Admin/AdminLogin.cshtml");
        }
        #endregion

    }
}
