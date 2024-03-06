﻿using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using HalloDoc.Repository.Repository;
using HalloDoc.Services.IServices;
using HalloDoc.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace HalloDoc.Controllers
{
  
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IAdminService _admin;
        private IJwtService _jwtService;


        public AdminController(ApplicationDbContext context,IAdminService admin,IJwtService jwtService)
        {
            _context = context;
            _admin = admin;
            _jwtService = jwtService;
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
                var jwtToken = _jwtService.GenerateJwtToken(myUser.Email,myUser.Id,Convert.ToString(myUser.Id),true,false,false);
                Response.Cookies.Append("jwt", jwtToken);
                //HttpContext.Session.SetString("UserName", myUser.UserName);
                //Admin userID = await _admin.GetAdmin(myUser.Email);
                //HttpContext.Session.SetInt32("AdminSession", userID.AdminId);
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
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> Dashboard()
        {
            var viewModel = new AdminDashboardViewModel
            {
                NewCount = await _admin.GetCount(1),
                PendingCount = await _admin.GetCount(2),
                ActiveCount = await _admin.GetCount(4),
                ConcludeCount = await _admin.GetCount(6),
                ToCloseCount = await _admin.GetCount(3),
                UnpaidCount = await _admin.GetCount(9)
            };

            return View(viewModel);
        }
        [CustomAuthorize("Admin")]
        public IActionResult New()
        {
            List<AdminDashboardViewModel> list = _admin.New();
            return PartialView("_NewPartialView", list);
        }
        [CustomAuthorize("Admin")]
        public IActionResult Pending()
        {
            List<AdminDashboardViewModel> list = _admin.Pending();

            return PartialView("_PendingPartialView", list);
        }
        [CustomAuthorize("Admin")]
        public IActionResult Active()
        {
            List<AdminDashboardViewModel> list = _admin.Active();

            return PartialView("_ActivePartialView", list);
        }
        [CustomAuthorize("Admin")]
        public IActionResult Conclude()
        {
            List<AdminDashboardViewModel> list = _admin.Conclude();

            return PartialView("_ConcludePartialView", list);
        }
        [CustomAuthorize("Admin")]
        public IActionResult ToClose()
        {
            List<AdminDashboardViewModel> list = _admin.ToClose();

            return PartialView("_ToClosePartialView", list);
        }
        [CustomAuthorize("Admin")]
        public IActionResult Unpaid()
        {
            List<AdminDashboardViewModel> list = _admin.Unpaid();

            return PartialView("_UnpaidPartialView", list);
        }

        #endregion

        #region View Case
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> ViewCase(int Id)
        {
            var result = await _admin.ViewCase(Id);
            return View(result);
        }
        [HttpPost]
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> ViewCase(ViewCaseViewModel viewmodel, int Id)
        {
            var result = await _admin.EditNewRequest(viewmodel,Id);
            return View(result);
        }

        #endregion

        #region View Notes
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> ViewNotes(AdminDashboardViewModel viewModel,int Id)
        {
            var result = await _admin.ViewNotes(viewModel, Id);
            return View(result);
        }
        [CustomAuthorize("Admin")]
        [HttpPost]
        public async Task<IActionResult> ViewNotes(AdminDashboardViewModel viewModel,int Id,int x)
        {
         var result =  await _admin.AddNotes(viewModel, Id);
            return View(result);
        }
        #endregion

        #region Cancel Case
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> CancelCase(CancelCaseViewModel viewModel, int id)
        {
            var result = await _admin.CancelCase(viewModel,id);
            return PartialView("_CancelCasePartialView", result);
        }
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> ConfirmCancelCase(CancelCaseViewModel viewModel ,int id)
        {
            var result = await _admin.ConfirmCancelCase(viewModel, id);
            return RedirectToAction("Dashboard" );
        }
        #endregion

        #region Assign Case
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> AssignCase(AssignCaseViewModel viewModel, int id)
        {
            var result = await _admin.AssignCase(viewModel, id);
            return PartialView("_AssignRequestPartialView", result);
        }
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> AssignRequest(AssignCaseViewModel viewModel, int id)
        {
            var result = await _admin.AssignRequest(viewModel, id);
            return RedirectToAction("Dashboard");
        }
     
    
        [HttpGet]
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> GetPhysiciansByRegion(int regionId)
        {
            var physicians = await _admin.GetPhysiciansByRegion(regionId);
           
            return Json(physicians);
        }
        #endregion

        #region Block Case
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> BlockCase(CancelCaseViewModel viewModel, int id)
        {
            var result = await _admin.BlockCase(viewModel, id);
            return PartialView("_BlockCasePartialView", result);
        }
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> BlockCaseRequest(CancelCaseViewModel viewModel, int id)
        {
            var result = await _admin.BlockCaseRequest(viewModel, id);
            return RedirectToAction("Dashboard");
        }
        #endregion

        #region View Uploads
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> ViewUploads(int Id)
        {
            var result =await _admin.ViewDocument(Id);
            return View(result);
        }
       
        [HttpPost]
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> ViewUploads(IFormFile a, int Id)
        {
            await _admin.ViewDocument(a, Id);
            return RedirectToAction("ViewUploads");
        }
        [CustomAuthorize("Admin")]
        public async Task<FileResult> DownloadFile(string name, string filename)
        {

            RequestWiseFile reqw = await _admin.DownloadFile(name);
            if (reqw != null)
            {
                filename = reqw.FileName;
            }

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads", filename);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }
        [CustomAuthorize("Admin")]
        public async Task<FileResult> DownloadAll(IEnumerable<string> selectedFiles)
        {
            if (selectedFiles.Count() > 0)
            {
                byte[] fileBytes = await _admin.DownloadAllByChecked(selectedFiles);
                return File(fileBytes, "application/zip", "download.zip");

            }
            else
            {
                int? requestid = HttpContext.Session.GetInt32("reqID");
                byte[] fileBytes = await _admin.DownloadAll(selectedFiles, requestid);
                return File(fileBytes, "application/zip", "download.zip");
            }
        }
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> DeleteFile(int fileID, int RequstId)
        {
             await _admin.DeleteFile(fileID);
          
            return RedirectToAction("ViewUploads", new { Id = RequstId });
        }
        [HttpPost]
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> DeleteFileByChecked(List<int> documentValues)
        {
            foreach(var items in  documentValues)
            {

                await _admin.DeleteFile(items);
            }

            return Json(documentValues);
        }
        #endregion

        #region Send Order
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> SendOrder(SendOrderViewModel viewModel, int Id)
        {
            try
            {
                var result =await _admin.SendOrder(viewModel, Id);
                return View(result);
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }
        [HttpGet]
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> GetBusinessByProfession(int ProfessionID)
        {
            var physicians = await _admin.GetBusinessByProfession(ProfessionID);

            return Json(physicians);
        }

        [HttpGet]
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> GetBusinessDetails(int BusinessId)
        {
            var result = await _admin.GetBusinessDetails(BusinessId);
            return Json(result);
        }
        [HttpPost]
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> SendOrderDetails(SendOrderViewModel viewModel, int Id)
        {
           
                 await _admin.SendOrderDetails(viewModel, Id);
                return RedirectToAction("Dashboard");
            
        }
        #endregion
    }
}
