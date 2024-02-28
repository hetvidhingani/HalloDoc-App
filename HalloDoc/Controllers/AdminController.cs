using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using HalloDoc.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace HalloDoc.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IAdminService _admin;

        public AdminController(ApplicationDbContext context,IAdminService admin)
        {
            _context = context;
            _admin = admin;
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
        public async Task<IActionResult> AdminLogin(AspNetUser user)
        {
            AspNetUser myUser = await _admin.checkEmailPassword(user);

            if (myUser != null)
            {
                HttpContext.Session.SetString("UserName", myUser.UserName);
                User userID = await _admin.GetUser(myUser.Email);
                HttpContext.Session.SetInt32("AdminSession", userID.UserId);
                return RedirectToAction("Dashboard");
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

        #region Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var viewModel = new AdminDashboardViewModel
            {
                NewCount = await _admin.GetCount(1),
                PendingCount = await _admin.GetCount(1),
                ActiveCount = await _admin.GetCount(2),
                ConcludeCount = await _admin.GetCount(3),
                ToCloseCount = await _admin.GetCount(4),
                UnpaidCount = await _admin.GetCount(5)
            };

            return View(viewModel);
        }
        public IActionResult New()
        {
            List<AdminDashboardViewModel> list = _admin.New();
            return PartialView("_NewPartialView", list);
        }
        public IActionResult Pending()
        {
            List<AdminDashboardViewModel> list = _admin.Pending();

            return PartialView("_PendingPartialView", list);
        }
        public IActionResult Active()
        {
            List<AdminDashboardViewModel> list = _admin.Active();

            return PartialView("_ActivePartialView", list);
        }
        public IActionResult Conclude()
        {
            List<AdminDashboardViewModel> list = _admin.Conclude();

            return PartialView("_ConcludePartialView", list);
        }
        public IActionResult ToClose()
        {
            List<AdminDashboardViewModel> list = _admin.ToClose();

            return PartialView("_ToClosePartialView", list);
        }
        public IActionResult Unpaid()
        {
            List<AdminDashboardViewModel> list = _admin.Unpaid();

            return PartialView("_UnpaidPartialView", list);
        }

        #endregion

        #region View Case

        public async Task<IActionResult> ViewCase(int Id)
        {
           // int user = await _admin.GetUserByRequestClientID(Id);
            var result = await _admin.ViewCase(Id);
            return View(result);
        }
        #endregion
        #region View Notes

        public async Task<IActionResult> ViewNotes()
        {
           
            return View();
        }
        #endregion

    }
}
