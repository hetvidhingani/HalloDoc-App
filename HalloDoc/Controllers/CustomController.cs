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
        private ICustomService _customService;

        #region constructor
        public CustomController(ApplicationDbContext context, IPatientService patient, IAdminService admin, IJwtService jwtService, ICustomService customService)
        {
            _context = context;
            _patient = patient;
            _admin = admin;
            _jwtService = jwtService;
            _customService = customService;

        }
        public IActionResult PatientSite()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return View();
        }
        public IActionResult AdminForgotPassword()
        {
            return View("~/Views/Admin/AdminForgotPassword.cshtml");
        }
        public IActionResult AdminResetPassword()
        {
            return View("~/Views/Admin/AdminResetPassword.cshtml");
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

        #region Admin / Provider Login
        public IActionResult AdminLogin()
        {

            return View("~/Views/Admin/AdminLogin.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(LoginViewModel user)
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            AspNetUser myUser = _customService.checkEmailPassword(user.Email, user.PasswordHash);

            if (myUser != null)
            {
                AspNetUserRole role = _customService.checkIfRoleExist(myUser.Id);
                if (role.RoleId == "1")
                {
                    Admin userID = await _admin.GetAdmin(myUser.Email);
                    var jwtToken = _jwtService.GenerateJwtToken(myUser);

                    Response.Cookies.Append("jwt", jwtToken);
                    Response.Cookies.Append("RoleMenu", userID.RoleId.ToString());
                    Response.Cookies.Append("AdminID", userID.AdminId.ToString());
                    Response.Cookies.Append("UserNameAdmin", userID.FirstName + " " + userID.LastName);
                    Response.Cookies.Append("AspNetIdAdmin", userID.AspNetUserId.ToString());

                    return RedirectToAction("Dashboard", "Admin");
                }
                else if (role.RoleId == "3")
                {
                    Physician userID = _customService.GetPhysician(myUser.Email);

                    var jwtToken = _jwtService.GenerateJwtToken(myUser);

                    Response.Cookies.Append("jwt", jwtToken);
                    Response.Cookies.Append("RoleMenu", userID.RoleId.ToString());
                    Response.Cookies.Append("ProviderID", userID.PhysicianId.ToString());
                    Response.Cookies.Append("UserNameProvider", userID.FirstName + " " + userID.LastName);
                    Response.Cookies.Append("AspNetIdProvider", userID.Id.ToString());
                    return RedirectToAction("Dashboard", "Provider");
                }
            }
            else
            {
                TempData["error"] = "Invalid UserName or Password";
            }
            return View("~/Views/Admin/AdminLogin.cshtml");
        }
        #endregion

        #region EmailSending

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPasswordRequest(string email)
        {

            var result = _customService.checkIfExist(email);
            if (result == null)
            {
                TempData["EnterEmail"] = "Please Provide Registerd Email.";
                return RedirectToAction("ResetPassword");

            }
            else
            {
                var link = Request.Scheme + "://" + Request.Host + "/Custom/PatientForgotPassword/" + email;
                var subject = "Reset Account Password";
                var body = "Click here " + "<a href=" + link + ">Reset Password</a>" + " to Update your password";
                _customService.SendEmail(email, link, subject, body, 0, 0, 0);
                TempData["EnterEmailSuccess"] = "Email is successfully Sent to your Registerd Email.";

                return RedirectToAction("ResetPassword");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminResetPasswordRequest(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { success = false, message = "Please enter an email address" });
            }

            var link = Request.Scheme + "://" + Request.Host + "/Custom/AdminResetPassword/" + email;
            var subject = "Reset Account Password";
            var body = "Click here " + "<a href=" + link + ">Reset Password</a>" + " to Update your password";
            _customService.SendEmail(email, link, subject, body, 0, 0, 0);
            TempData["success"] = "Link is successfully sent to your Registed Email Account";
            return RedirectToAction("AdminForgotPassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkToCreateAccount(CreateAccountViewModel req)
        {
            if (req.Email == null)
            {
                TempData["emptyemail"] = "Please enter Email";
                return RedirectToAction("CreateAccountRequest", "Custom");

            }

            var link = Request.Scheme + "://" + Request.Host + "/Custom/CreateAccountPatient/" + req.Email;
            var subject = "Create Account";
            var body = "Click here " + "<a href=" + link + ">Create Account</a>" + " to Create Account At HALLODOC Plateform!";
            _customService.SendEmail(req.Email, link, subject, body, 0, 0, 0);

            TempData["emailsend"] = "Email is sent successfully to your email account";
            return RedirectToAction("PatientSite", "Custom");
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

        #region Patient Login

        public IActionResult RegisterdPatientLogin()
        {
            if (HttpContext.Session.GetInt32("UserSession") != null)
            {
                return RedirectToAction("DashBoard", "Patient");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterdPatientLogin(LoginViewModel viewModel)
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            var myUser = _customService.checkEmailPassword(viewModel.Email, viewModel.PasswordHash);

            if (myUser != null)
            {

                User userID = await _patient.GetUser(myUser.Email);
                if (userID == null)
                {
                    ViewBag.Message = "Please enter Valid Email";
                }
                else
                {
                    var jwtToken = _jwtService.GenerateJwtToken(myUser);
                    Response.Cookies.Append("jwt", jwtToken);
                    Response.Cookies.Append("UserID", userID.UserId.ToString());
                    Response.Cookies.Append("UserNameUser", userID.FirstName + " " + userID.LastName);
                    Response.Cookies.Append("AspNetIdUser", userID.Id.ToString());
                    HttpContext.Session.SetString("UserName", myUser.UserName);
                    HttpContext.Session.SetInt32("UserSession", userID.UserId);
                }
                return RedirectToAction("DashBoard", "Patient");
            }
            else
            {
                ViewBag.Message = "Invalid User Name or Password";
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
            var data = _customService.checkIfExist(createAccountViewModel.Email);
            if (data != null)
            {
                await _customService.PatientForgotPassword(createAccountViewModel);
                string Message = await _patient.GetTempData<string>("Message");
                TempData["PwdUpdate"] = "Password Updated";
            }
            else
            {
                TempData["EnterEmail"] = "Use Your Registerd Email only!!!";

            }

            return View();
        }
        #endregion

        #region PatientRequest
        [HttpGet]
        public async Task<IActionResult> PatientRequest()
        {
            //var request = HttpContext.Request;
            //int? userId = Int32.Parse(request.Cookies["UserID"]);

            var result = await _patient.PatientRequest();
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
                TempData["success"] = "Form Saved Successfully.";

                return RedirectToAction(result);
            }
            else
            {
                TempData["error"] = "Error Saving Data";
                return View(viewModel);
            }
        }

        #endregion

        #region Family Freind Request
        [HttpPost]
        public async Task<IActionResult> FamilyFriendRequest(OtherRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _patient.FamilyFriendRequest(viewModel);

                await LinkToCreateAccount(new CreateAccountViewModel
                {
                    Email = viewModel.Email
                });
                return View("PatientSite");

            }
            return View("PatientSite");
        }

        #endregion

        #region Business Request
        [HttpPost]
        public async Task<IActionResult> BusinessRequest(OtherRequestViewModel viewModel)
        {

            await _patient.BusinessRequest(viewModel);

            await LinkToCreateAccount(new CreateAccountViewModel
            {
                Email = viewModel.Email
            });
            return View("PatientSite");
        }

        #endregion

        #region Concierge Request
        [HttpPost]
        public async Task<IActionResult> ConciergeRequest(OtherRequestViewModel viewModel)
        {

            await _patient.ConciergeRequest(viewModel);

            await LinkToCreateAccount(new CreateAccountViewModel
            {
                Email = viewModel.Email
            });
            return View("PatientSite");

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

        #region send Agreement
        [HttpGet]
        public async Task<IActionResult> ReviewAgreement()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AcceptAgreement(int id)
        {

            await _customService.AcceptAgreement(id);
            return Json("success");
        }

        [HttpGet]
        public async Task<IActionResult> CancelAgreement(int id)
        {
            var result = await _customService.GetRequestClientByID(id);
            var name = result.FirstName + " " + result.LastName;
            return Json(name);
        }
        [HttpPost]
        public async Task<string> ConfirmCancelAgreement(int id, string note)
        {
            await _customService.ConfirmCancelAgreement(id, note);
            return "";
        }
        #endregion
    }
}
