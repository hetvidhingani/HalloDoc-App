using HalloDoc.DataContext;
using HalloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using HalloDoc.Models;

namespace HalloDoc.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult PatientSite()
        {
            return View();
        }
        
        //public IActionResult RegisterdPatientLogin()
        //{
        //    return View();
        //}
        public IActionResult PatientForgotPassword()
        {
            return View();
        }
        public IActionResult PatientSubmitRequest()
        {
            return View();
        }
        public IActionResult PatientRequest()
        {
            return View();
        }
        public IActionResult FamilyFriendRequest()
        {
            return View();
        }
        public IActionResult ConciergeRequest()
        {
            return View();
        }
        public IActionResult BusinessRequest()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }


      
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PatientRequest(PatientRequestViewModel request)
        {
            if (ModelState.IsValid)
            {
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }


        public IActionResult RegisterdPatientLogin()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("DashBoard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult RegisterdPatientLogin(AspNetUser user)
        {
            var myUser = _context.AspNetUsers.Where(x => x.Email == user.Email && x.PasswordHash == user.PasswordHash).FirstOrDefault();
            if (myUser != null)
            {
                HttpContext.Session.SetString("UserSession", myUser.Email);

                return RedirectToAction("DashBoard");
            }
            else
            {
                ViewBag.Message = "Login Failed!";
            }
          
            return View();
        }


        [HttpPost]
        public IActionResult DashBoard()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("RegisterdPatientLogin");
            }
            return View();
        }
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("RegisterdPatientLogin");
            }
            return View();
        }

    }
}
