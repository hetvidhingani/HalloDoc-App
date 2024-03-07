﻿using HalloDoc.Entities.DataModels;
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
        public IActionResult BusinessRequest()
        {
            return View();
        }

        public IActionResult CreateAccountPatient()
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
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("HalloDoc", "t12281554@gmail.com"));
            message.To.Add(new MailboxAddress("HalloDocMember", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;


            message.Body = bodyBuilder.ToMessageBody();
            ViewBag.emailsend = "Email is sent successfully to your email account";

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("t12281554@gmail.com", "vbdhvlywjczuttbh");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        private const int TokenExpirationHours = 24;

        public string GenerateToken()
        {
            byte[] tokenBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }

            string token = Convert.ToBase64String(tokenBytes);

            return token;
        }

        public DateTime GetTokenExpiration()
        {
            return DateTime.UtcNow.AddHours(TokenExpirationHours);
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
            var resetToken = GenerateToken();
            var resetLink = "<a href=" + Url.Action("PatientForgotPassword", "Patient", new { email = req.Email, code = resetToken }, "http") + ">Reset Password</a>";
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
            var resetToken = GenerateToken();
            var resetLink = "<a href=" + Url.Action("CreateAccountPatient", "Patient", new { email = req.Email, code = resetToken }, "http") + ">Create account</a>";
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
        public async Task<IActionResult> RegisterdPatientLogin(AspNetUser user)
        {
            AspNetUser myUser = await _patient.checkEmailPassword(user);

            if (myUser != null)
            {
                User userID = await _patient.GetUser(myUser.Email);
                var jwtToken = _jwtService.GenerateJwtToken(myUser);
                Response.Cookies.Append("jwt", jwtToken);
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
            var result = await _patient.FamilyFriendRequest(viewModel);
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
            if (ModelState.IsValid)
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

            }
            return View("RegisterdPatientLogin");
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