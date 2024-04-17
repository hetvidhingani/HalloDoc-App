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
using HalloDoc.Repository.Repository;
using System.Linq;
using Org.BouncyCastle.Asn1.Ocsp;

namespace HalloDoc.Controllers
{
    [CustomAuthorize("2")]

    public class PatientController : Controller
    {
        public IPatientService _patient;
        public ICustomService _customService;
        public PatientController( IPatientService patient, ICustomService customService)
        {
            _patient = patient;
            _customService = customService;
        }

        #region view

        public IActionResult ReviewAgreement()
        {
            return View();
        }


        public async Task<IActionResult> SubmitInformationSomeoneElse()
        {
            PatientRequestViewModel viewModel = new PatientRequestViewModel();

            var result = await _patient.RegionListUser(viewModel);
            return View(result);

        }
        #endregion

        #region Dashboard
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> GetPatientDashboardData(int CurrentPage = 1)
        {
            var request = HttpContext.Request;
            int userID = Int32.Parse(request.Cookies["UserID"]);
            var result = await _patient.Dashboard(userID, CurrentPage);
            return PartialView("_PatientDashboardPartialView", result);
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
        [HttpGet]
        public async Task<IActionResult> submitInfoMe()
        {
            var request = HttpContext.Request;
            int? userId = Int32.Parse(request.Cookies["UserID"]);
            var result = await _patient.submitInfoMe(userId);
           
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> submitInfoMe(PatientRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _patient.PatientRequest(viewModel);
                TempData["success"] = "Form Saved Successfully.";

                return RedirectToAction("Dashboard");
            }
            else
            {
                TempData["error"] = "Error Saving Data";
                return View(viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitInformationSomeoneElse(PatientRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = HttpContext.Request;
                int userId = Int32.Parse(request.Cookies["UserID"]);
                var result = await _patient.SubmitInformationSomeoneElse(model, userId);
                TempData["success"] = "Form Saved Successfully.";

                return RedirectToAction(result);
            }
            else
            {
                TempData["error"] = "Error Saving Data";
            }
            return View();
        }

        #endregion

        #region View Document

        public async Task<IActionResult> ViewDocument(int Id)
        {

            ViewBag.MySession = HttpContext.Session.GetString("UserName");
            var result = await _customService.ViewDocument(Id);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> ViewDocument(IFormFile a, int Id)
        {
            var request = HttpContext.Request;
            int userID = Int32.Parse(request.Cookies["UserID"]);
            await _customService.ViewDocument(a, Id, 0, userID);
            return RedirectToAction("ViewDocument");
        }

        public async Task<FileResult> DownloadFile(int fileId)
        {
            string filename = "";
            RequestWiseFile reqw = await _customService.DownloadFile(fileId);
            if (reqw != null)
            {
                filename = reqw.FileName;
            }

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads", filename);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        public async Task<IActionResult> DownloadAll(IEnumerable<int> documentValues, int ReqID)
        {
            if (documentValues.Count() > 0)
            {
                byte[] fileBytes = await _customService.DownloadAllByChecked(documentValues);
                string zipFileData = Convert.ToBase64String(fileBytes);
                return Json(new { success = true, zipFileData });
            }
            else
            {
                byte[] fileBytes = await _customService.DownloadAll(documentValues, ReqID);
                string zipFileData = Convert.ToBase64String(fileBytes);
                return Json(new { success = true, zipFileData });
            }
        }

        #endregion

        #region Patient My Profile
        public async Task<IActionResult> Profile()
        {
            var request = HttpContext.Request;
            var userID = Int32.Parse(request.Cookies["UserID"]);
            var result = await _patient.Profile(userID);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserMyProfileViewModel patientRequestViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _patient.EditUser(patientRequestViewModel);
                HttpContext.Response.Cookies.Append("UserNameUser", result.FirstName + " " + result.LastName);
                TempData["success"] = "Form Saved Successfully.";

                return RedirectToAction("Profile", result);

            }
            else
            {
                TempData["error"] = "Error Saving Data";

                return RedirectToAction("Profile");
            }
        }

        #endregion

        #region Logout
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                HttpContext.Session.Clear();
                Response.Cookies.Delete("jwt");
                Response.Cookies.Delete("UserID");
                Response.Cookies.Delete("UserNameUser");
                Response.Cookies.Delete("AspNetIdUser");
                return RedirectToAction("RegisterdPatientLogin", "Custom");
            }
            return RedirectToAction("RegisterdPatientLogin", "Custom");
        }


        #endregion

    }
}
