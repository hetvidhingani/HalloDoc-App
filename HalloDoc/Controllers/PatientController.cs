using HalloDoc.Entities.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.IO.Compression;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using MimeKit;
using HalloDoc.Repository.IRepository;
using HalloDoc.Services.IServices;
using HalloDoc.Entities.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using HalloDoc.Services.Services;

namespace HalloDoc.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IPatientService _patient;

        public PatientController(ApplicationDbContext context, IPatientService patient)
        {
            _context = context;
            _patient = patient;
        }
       
        #region view
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
        public IActionResult ReviewAgreement()
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
        public IActionResult SubmitInformationSomeoneElse()
        {
            return View();

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

                return RedirectToAction("DashBoard");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterdPatientLogin(AspNetUser user)
        {
            AspNetUser myUser = await _patient.checkEmailPassword(user);

            if (myUser != null)
            {
                HttpContext.Session.SetString("UserName", myUser.UserName);
                User userID = await _patient.GetUser(myUser.Email);
                HttpContext.Session.SetInt32("UserSession", userID.UserId);


                return RedirectToAction("DashBoard");
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
        
        #region Dashboard
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.MySession = HttpContext.Session.GetString("UserName");
            int? userID = HttpContext.Session.GetInt32("UserSession");
            var result = await _patient.Dashboard(userID);
            return View(result);

        }
        [HttpPost]
        public IActionResult DashBoard()
        {
            if (HttpContext.Session.GetInt32("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetInt32("UserSession");
            }
            else
            {
                return RedirectToAction("RegisterdPatientLogin");
            }
            return View();
        }


        #endregion
        
        #region create new request

        [HttpPost]
        public async Task<IActionResult> SubmitInformationSomeoneElse(PatientRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                int? userId = HttpContext.Session.GetInt32("UserSession");

                var result = await _patient.SubmitInformationSomeoneElse(viewModel, userId);
                return RedirectToAction(result, viewModel);
            }

            return View();
        }

        #endregion
        
        #region View Document
        
        public async Task<IActionResult> ViewDocument(int Id)
        {
            HttpContext.Session.SetInt32("reqID", Id);
            ViewBag.MySession = HttpContext.Session.GetString("UserName");
            var result =  _patient.ViewDocument(Id);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> ViewDocument(IFormFile a, int Id)
        {
            await _patient.ViewDocument(a, Id);
            return RedirectToAction("ViewDocument");
        }

        public async Task<FileResult> DownloadFile(string name, string filename)
        {
            
            RequestWiseFile reqw = await _patient.DownloadFile(name);
            if (reqw != null)
            {
                filename = reqw.FileName;
            }

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads", filename);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        public async Task<FileResult> DownloadAll(IEnumerable<string> selectedFiles)
        {
            if (selectedFiles.Count() > 0)
            {
                byte[] fileBytes = await _patient.DownloadAllByChecked(selectedFiles);
                return File(fileBytes, "application/zip", "download.zip");

            }
            else
            {
                int? requestid = HttpContext.Session.GetInt32("reqID");
                byte[] fileBytes = await _patient.DownloadAll(selectedFiles, requestid);
                return File(fileBytes, "application/zip", "download.zip");
            }
        }

        #endregion
        
        #region Patient My Profile
        public async Task<IActionResult> Profile(PatientRequestViewModel requestViewModel)
        {
            int? userId = HttpContext.Session.GetInt32("UserSession");
            var result = await _patient.Profile(requestViewModel, userId);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(PatientRequestViewModel patientRequestViewModel)
        {
            int? userId = HttpContext.Session.GetInt32("UserSession");
            var result = await _patient.EditUser(patientRequestViewModel, userId);

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
       
        #region Logout
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                HttpContext.Session.Clear();
                return RedirectToAction("RegisterdPatientLogin");
            }
            return View();
        }


        #endregion

    }
}
