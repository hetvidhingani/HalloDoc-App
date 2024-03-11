using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using HalloDoc.Services.IServices;
using HalloDoc.Services.Services;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Security.Cryptography;

namespace HalloDoc.Controllers
{
    public class CustomController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IPatientService _patient;
        private IAdminService _admin;
        private IJwtService _jwtService;

        #region constructor
        public CustomController(ApplicationDbContext context, IPatientService patient, IAdminService admin, IJwtService jwtService)
        {
            _context = context;
            _patient = patient;
            _admin = admin;
            _jwtService = jwtService;

        }
        public IActionResult PatientSite()
        {
            return View();
        }
       
        public IActionResult ResetPassword()
        {
            return View();
        }
        public IActionResult PatientSubmitRequest()
        {
            return View();
        }
        public async Task<IActionResult> BusinessRequest()
        {
            var result = await _patient.RegionList();
            return View(result);
        }

        public IActionResult CreateAccountPatient()
        {
            return View();
        }

        public async Task<IActionResult> FamilyFriendRequest()
        {
            var result = await _patient.RegionList();
            return View(result);
        }
        public async Task<IActionResult> ConciergeRequest()
        {
            var result = await _patient.RegionList();
            return View(result);
        }
        #endregion

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
                var jwtToken = _jwtService.GenerateJwtToken(myUser);
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

        #region EmailSending
        public async Task SendEmailfgpasswordAsync(string toEmail, string subject, string body)
        {
            await _admin.SendMail(toEmail, subject, body);
            
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetpasswordRequest(CreateAccountViewModel req)
        {
            if (req.Email == null)
            {
                ViewBag.emailEnter = "Please enter Email";
                return RedirectToAction("PatientForgotPassword", "Patient");

            }
           
            var resetLink = "<a href=" + Url.Action("PatientForgotPassword", "Patient", new { email = req.Email}, "http") + ">Reset Password</a>";
            var subject = "Password Reset Request";
            var body = "<b>Please find the Password Reset Link.</b><br/>" + resetLink;

            await SendEmailfgpasswordAsync(req.Email, subject, body);
            TempData["sent"] = "Reset Password link is Successfully sent to your email!";
            return RedirectToAction("ResetPassword", "Patient");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkToCreateAccount(CreateAccountViewModel req)
        {
            if (req.Email == null)
            {
                TempData["emptyemail"] = "Please enter Email";
                return RedirectToAction("CreateAccountRequest", "Patient");

            }
           
            var resetLink = "<a href=" + Url.Action("CreateAccountPatient", "Patient", new { email = req.Email }, "http") + ">Create account</a>";
            var subject = "Create Patient Account";
            var body = "<b>Please Create Account Link.</b><br/>" + resetLink;

            await SendEmailfgpasswordAsync(req.Email, subject, body);
            TempData["emailsend"] = "Email is sent successfully to your email account";
            return RedirectToAction("PatientSite", "Patient");
        }


        #endregion

        #region check email
        [Route("/Patient/checkemail/{email}")]
        [HttpGet]
        public IActionResult CheckEmail(string email)
        {

            return Json(new { exists = _patient.CheckEmail(email) });
        }
        //public IActionResult CheckEmail(string email)
        //{
        //    var emailExists = _context.AspNetUsers.Any(u => u.Email == email);
        //    return Json(new { exists = emailExists });
        //}
        #endregion

        #region Login

        public IActionResult RegisterdPatientLogin()
        {
            if (HttpContext.Session.GetInt32("UserSession") != null)
            {

                return RedirectToAction("DashBoard","Patient");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterdPatientLogin(LoginViewModel viewModel)
        {
     
            var myUser = await _patient.checkEmailPassword(viewModel.Email,viewModel.PasswordHash);
           
            if (myUser != null)
            {
               
                var jwtToken = _jwtService.GenerateJwtToken(myUser);
                Response.Cookies.Append("jwt", jwtToken);

                User userID = await _patient.GetUser(myUser.Email);
                HttpContext.Session.SetString("UserName", myUser.UserName);
                HttpContext.Session.SetInt32("UserSession", userID.UserId);

                return RedirectToAction("DashBoard","Patient");
            }
            else
            {
                ViewBag.Message = "Login Failed!";
            }

            return View();
        }

        #endregion

        #region Forgot Password
        public IActionResult PatientForgotPassword()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> PatientForgotPassword(CreateAccountViewModel createAccountViewModel)
        {
            var result = await _patient.PatientForgotPassword(createAccountViewModel);
            string Message = await _patient.GetTempData<string>("Message");
            ViewBag.Message = Message;
            return View(result);
        }
        #endregion

        #region PatientRequest
        [HttpGet]
        public async Task<IActionResult> PatientRequest()
        {
            int? userId = HttpContext.Session.GetInt32("UserSession");
            var result = await _patient.PatientRequest(userId);
            if (result == "")
            {
                return View("PatientRequest");
            }
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> PatientRequest(PatientRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _patient.PatientRequest(viewModel);
                return RedirectToAction(result);
            }
            return View(viewModel);
        }

        #endregion

        #region Family Freind Request
        [HttpPost]
        public async Task<IActionResult> FamilyFriendRequest(OtherRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _patient.FamilyFriendRequest(viewModel);
                if (result == "")
                {
                    await LinkToCreateAccount(new CreateAccountViewModel
                    {
                        Email = viewModel.Email
                    });
                    return View("PatientSite");
                }
            }
          


            return View("PatientSite");
        }

        #endregion

        #region Business Request
        [HttpPost]
        public async Task<IActionResult> BusinessRequest(OtherRequestViewModel viewModel)
        {


            var result = await _patient.BusinessRequest(viewModel);
            if (result == "")
            {
                await LinkToCreateAccount(new CreateAccountViewModel
                {
                    Email = viewModel.Email
                });
                return View("PatientSite");
            }



            return View(result);
        }


        #endregion

        #region Concierge Request
        [HttpPost]
        public async Task<IActionResult> ConciergeRequest(OtherRequestViewModel viewModel)
        {
           

                var result = await _patient.ConciergeRequest(viewModel);
                if (result == "")
                {
                    await LinkToCreateAccount(new CreateAccountViewModel
                    {
                        Email = viewModel.Email
                    });
                    return View("PatientSite");
                }


            return View(result);
        }



        #endregion

        #region Create Account

        [HttpPost]
        public async Task<IActionResult> CreateAccountRequest(CreateAccountViewModel createAccountViewModel)
        {

            var result = await _patient.CreateAccountRequest(createAccountViewModel);
            string Message = await _patient.GetTempData<string>("errormsg");
            TempData["errormsg"] = Message;
            return View(result);

        }



        #endregion

    }
}
