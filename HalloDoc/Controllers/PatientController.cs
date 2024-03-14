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

namespace HalloDoc.Controllers
{
    [CustomAuthorize("2")]

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
       
        public IActionResult ReviewAgreement()
        {
            return View();
        }


        public async Task<IActionResult> SubmitInformationSomeoneElse()
        {
            var result = await _patient.RegionListUser();
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

            return RedirectToAction("Profile",result);
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
                return RedirectToAction("RegisterdPatientLogin", "Custom");
               
            }
            return View();
        }


        #endregion

    }
}
