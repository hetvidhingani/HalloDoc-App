using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult PatientSite()
        {
            return View();
        }
        
        public IActionResult RegisterdPatientLogin()
        {
            return View();
        }
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

    }
}
