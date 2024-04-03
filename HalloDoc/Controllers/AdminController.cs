using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using HalloDoc.Repository.IRepository;
using HalloDoc.Repository.Repository;
using HalloDoc.Services.IServices;
using HalloDoc.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Org.BouncyCastle.Ocsp;
using System.Diagnostics.Contracts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HalloDoc.Controllers
{
    [CustomAuthorize("1")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IAdminService _admin;
        private IJwtService _jwtService;
        private ICustomService _customService;


        public AdminController(ApplicationDbContext context, IAdminService admin, IJwtService jwtService, ICustomService customService)
        {
            _context = context;
            _admin = admin;
            _jwtService = jwtService;
            _customService = customService;
        }
        #region admin
        #region Logout
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                HttpContext.Session.Remove("AdminSession");
                HttpContext.Session.Clear();
                Response.Cookies.Delete("jwt");
                return RedirectToAction("AdminLogin", "Custom");
            }
            return View();
        }


        #endregion

        #region Dashboard

        public async Task<IActionResult> Dashboard()
        {
            //var cookie = Request.Cookies["jwt"];
            //var cookiedata = _jwtService.GetTokenData(cookie);
            var viewModel = new AdminDashboardViewModel
            {
                NewCount = await _admin.GetCount(1),
                PendingCount = await _admin.GetCount(2),
                ActiveCount = await _admin.GetCount(4),
                ConcludeCount = await _admin.GetCount(6),
                ToCloseCount = await _admin.GetCount(3),
                UnpaidCount = await _admin.GetCount(9),

            };
            var result = await _admin.DashboardRegions(viewModel);

            return View(result);
        }

        [HttpPost]
        public IActionResult GetTable(string state, int CurrentPage, string? PatientName, int? ReqType, int? RegionId)
        {
            if (ModelState.IsValid)
            {
                List<AdminDashboardViewModel> list = new List<AdminDashboardViewModel>();

                switch (state)
                {
                    case "New":
                        ViewBag.state = "New";
                        var result = _admin.Admintbl(state, list, 1);
                        var paging = _admin.Pagination(state, CurrentPage, PatientName, ReqType, RegionId, result);
                        return PartialView("_TablePartialView", paging);

                    case "Pending":
                        ViewBag.state = "Pending";
                        var result2 = _admin.Admintbl(state, list, 2);
                        var paging2 = _admin.Pagination(state, CurrentPage, PatientName, ReqType, RegionId, result2);

                        return PartialView("_TablePartialView", paging2);

                    case "Active":
                        ViewBag.state = "Active";
                        var result3 = _admin.Admintbl(state, list, 4);
                        var paging3 = _admin.Pagination(state, CurrentPage, PatientName, ReqType, RegionId, result3);

                        return PartialView("_TablePartialView", paging3);

                    case "Conclude":
                        ViewBag.state = "Conclude";
                        var result4 = _admin.Admintbl(state, list, 6);
                        var paging4 = _admin.Pagination(state, CurrentPage, PatientName, ReqType, RegionId, result4);

                        return PartialView("_TablePartialView", paging4);

                    case "Toclose":
                        ViewBag.state = "Toclose";
                        var result5 = _admin.Admintbl(state, list, 3);
                        var paging5 = _admin.Pagination(state, CurrentPage, PatientName, ReqType, RegionId, result5);

                        return PartialView("_TablePartialView", paging5);

                    case "Unpaid":
                        ViewBag.state = "Unpaid";
                        var result6 = _admin.Admintbl(state, list, 9);
                        var paging6 = _admin.Pagination(state, CurrentPage, PatientName, ReqType, RegionId, result6);

                        return PartialView("_TablePartialView", paging6);

                    default:
                        var result7 = _admin.Admintbl(state, list, 1);
                        var paging7 = _admin.Pagination(state, CurrentPage, PatientName, ReqType, RegionId, result7);

                        return PartialView("_TablePartialView", paging7);
                }
            }
            return View();
        }
        #endregion

        #region View Case

        public async Task<IActionResult> ViewCase(int Id)
        {
            if (ModelState.IsValid)
            {
                var result = await _admin.ViewCase(Id);
                return View(result);
            }
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> ViewCase(ViewCaseViewModel viewmodel, int Id)
        {
            if (ModelState.IsValid)
            {
                await _admin.EditNewRequest(viewmodel, Id);
                return RedirectToAction("ViewCase", new { Id = Id });
            }
            return View();
        }

        #endregion

        #region View Notes

        public async Task<IActionResult> ViewNotes(int Id)
        {
            if (ModelState.IsValid)
            {
                var result = await _admin.ViewNotes(Id);
                return View(result);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ViewNotes(string? additionalNotes, string? adminNotes, int id)
        {
            if (ModelState.IsValid)
            {
                await _admin.AddNotes(additionalNotes, adminNotes, id);
                return Json("success");
            }
            return View();
        }
        #endregion

        #region Cancel Case

        public async Task<IActionResult> CancelCase(CancelCaseViewModel viewModel, int id)
        {

            var result = await _admin.CancelCase(viewModel, id);
            return PartialView("_CancelCasePartialView", result);

        }

        public async Task<IActionResult> ConfirmCancelCase(CancelCaseViewModel viewModel, int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _admin.ConfirmCancelCase(viewModel, id);
                return RedirectToAction("Dashboard");
            }
            return RedirectToAction("Dashboard");

        }
        #endregion

        #region Assign Case

        public async Task<IActionResult> AssignCase(AssignCaseViewModel viewModel, int id)
        {
            var result = await _admin.AssignCase(viewModel, id);
            if (result != null)
            {
                return PartialView("_AssignRequestPartialView", result);
            }
            return PartialView("_AssignRequestPartialView", result);
        }

        public async Task<IActionResult> AssignRequest(AssignCaseViewModel viewModel, int id)
        {
            if (ModelState.IsValid)
            {
                await _admin.AssignRequest(viewModel, id);

                return Json(new { success = true });
            }
            return RedirectToAction("Dashboard");
        }


        [HttpGet]

        public async Task<IActionResult> GetPhysiciansByRegion(int regionId)
        {
            var physicians = await _admin.GetPhysiciansByRegion(regionId);

            return Json(physicians);
        }
        #endregion

        #region Block Case

        public async Task<IActionResult> BlockCase(CancelCaseViewModel viewModel, int id)
        {
            var result = await _admin.BlockCase(viewModel, id);
            return PartialView("_BlockCasePartialView", result);
        }

        public async Task<IActionResult> BlockCaseRequest(CancelCaseViewModel viewModel, int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _admin.BlockCaseRequest(viewModel, id);

                return Json(new { success = true });

            }
            return View();
        }
        #endregion

        #region Transfer Case


        public async Task<IActionResult> TransferCase(AssignCaseViewModel viewModel, int id)
        {
            var result = await _admin.TransferCase(viewModel, id);
            return PartialView("_TransferRequestPartialView", result);
        }

        public async Task<IActionResult> TransferRequest(AssignCaseViewModel viewModel, int id)
        {
            if (ModelState.IsValid)
            {
                await _admin.TransferRequest(viewModel, id);
                return Json(new { success = true });

            }
            return View();
        }

        #endregion

        #region Clear Case
        public async Task<IActionResult> ClearCase(int id)
        {
            HttpContext.Session.SetInt32("requestID", id);

            return PartialView("_ClearCasePartialView");
        }
        public async Task<IActionResult> ClearRequest()
        {
            if (ModelState.IsValid)
            {
                var id = HttpContext.Session.GetInt32("requestID");

                await _admin.ClearRequest(id);
                return RedirectToAction("Dashboard");
            }
            return View();

        }
        #endregion

        #region Close Case
        public async Task<IActionResult> CloseCase(int Id)
        {
            var result = await _admin.CloseCase(Id);
            return View(result);
        }
        public async Task<IActionResult> EditCloseCase(CloseCaseViewModel viewModel, int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _admin.EditClose(viewModel, id);
                return RedirectToAction("CloseCase", new { Id = result });
            }
            return View();
        }

        public async Task<IActionResult> ConfirmCloseCase(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _admin.ConfirmCloseCase(id);
                return RedirectToAction("Dashboard");
            }
            return RedirectToAction("Dashboard");
        }
        #endregion

        #region View Uploads

        public async Task<IActionResult> ViewUploads(int Id)
        {
            HttpContext.Session.SetInt32("reqID", Id);
            ViewBag.MySession = HttpContext.Session.GetString("UserName");
            var result = await _customService.ViewDocument(Id);
            return View(result);
        }

        [HttpPost]

        public async Task<IActionResult> ViewUploads(IFormFile a, int Id)
        {
            await _customService.ViewDocument(a, Id);
            return RedirectToAction("ViewUploads");
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

        public async Task<IActionResult> DownloadAll(IEnumerable<int> documentValues)
        {
            if (documentValues.Count() > 0)
            {
                byte[] fileBytes = await _customService.DownloadAllByChecked(documentValues);
                string zipFileData = Convert.ToBase64String(fileBytes);
                return Json(new { success = true, zipFileData });

            }
            else
            {
                int? requestid = HttpContext.Session.GetInt32("reqID");

                byte[] fileBytes = await _customService.DownloadAll(documentValues, requestid);
                string zipFileData = Convert.ToBase64String(fileBytes);
                return Json(new { success = true, zipFileData });
            }
        }

        public async Task<IActionResult> DeleteFile(int fileID)
        {
            int? requstId = HttpContext.Session.GetInt32("reqID");
            await _admin.DeleteFile(fileID, requstId);

            return RedirectToAction("ViewUploads", new { Id = requstId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFileByChecked(List<int> documentValues)
        {
            int? requestid = HttpContext.Session.GetInt32("reqID");
            if (documentValues.Count() > 0)
            {
                foreach (var items in documentValues)
                {

                    await _admin.DeleteFile(items, requestid);
                }
            }
            else
            {
                await _admin.DeleteFile(0, requestid);

            }


            return Json(documentValues);
        }
        #endregion

        #region Send Order
        [HttpGet]
        public async Task<IActionResult> SendOrder(int Id)
        {

            var result = await _admin.SendOrder(Id);
            return View(result);

        }
        [HttpGet]

        public async Task<IActionResult> GetBusinessByProfession(int ProfessionID)
        {
            var physicians = await _admin.GetBusinessByProfession(ProfessionID);

            return Json(physicians);
        }

        [HttpGet]

        public async Task<IActionResult> GetBusinessDetails(int BusinessId)
        {
            var result = await _admin.GetBusinessDetails(BusinessId);
            return Json(result);
        }
        [HttpPost]

        public async Task<IActionResult> SendOrderDetails(SendOrderViewModel viewModel, int Id)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors });
            }
            else
            {
                string? adminID = HttpContext.Session.GetString("AdminAspNetID");

                await _admin.SendOrderDetails(viewModel, Id, adminID);

            }
            return Json(new { success = true });

        }
        #endregion

        #region send agreement
        public async Task<IActionResult> SendAgreement(int id)
        {
            ViewBag.AgreementIdToClient = id;
            var result = await _admin.sendAgreement(id);
            return PartialView("_SendAgreementPartialView", result);
        }


        public async Task<IActionResult> SendAgreementLink(ViewCaseViewModel viewModel)
        {
            var AdminId = HttpContext.Session.GetInt32("AdminSession");

            if (ModelState.IsValid)
            {
                if (viewModel.Email == null)
                {
                    TempData["emptyemail"] = "Please enter Email";
                    return RedirectToAction("SendAgreement", "Admin");
                }

                var link = Request.Scheme + "://" + Request.Host + "/Custom/ReviewAgreement?requestClinetID=" + viewModel.requestclientID;
                var subject = "Review Agreement";
                var body = "Click here " + "<a href=" + link + ">Agreement</a>" + " to Review Agreement!!!";
                List<string> attachmentFilePaths = null;
                _customService.SendEmail(viewModel.Email, link, subject, body, attachmentFilePaths);
                _customService.EmailLog(viewModel.requestclientID, viewModel.Email,link, subject, body, (int)AdminId);

                TempData["emailsend"] = "Email is sent successfully to your email account";
                return RedirectToAction("Dashboard", "Admin");
            }
            return View(viewModel);
        }


        #endregion

        #region Admin MyProfile
        public async Task<IActionResult> AdminMyProfile()
        {
            var AdminId = HttpContext.Session.GetInt32("AdminSession");
            var result = await _admin.AdminMyProfile(AdminId);
            return View(result);
        }
        public async Task<IActionResult> ResetAdminPassword(AdminMyProfileViewModel model)
        {

            await _admin.ResetPasswordAdmin(model);
            return RedirectToAction("AdminMyProfile");


        }
        public async Task<IActionResult> SaveAdminInfo(AdminMyProfileViewModel model, List<int> notification)
        {

            await _admin.SaveAdminInfo(model, notification);
            return RedirectToAction("AdminMyProfile");

        }
        public async Task<IActionResult> SaveBillingInfo(AdminMyProfileViewModel model)
        {

            await _admin.SaveBillingInfo(model);
            return RedirectToAction("AdminMyProfile");

        }
        #endregion

        #region Encounter Form
        public async Task<IActionResult> EncounterForm(int RequestId)
        {
            var result = await _admin.EncounterForm(RequestId);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> EncounterFormSaveChanges(EncounterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _admin.EncounterFormSaveChanges(model);
                return RedirectToAction("EncounterForm", result);
            }
            return View(model);
        }
        #endregion

        #region send Mail Attachment
        public async Task<IActionResult> SendEmailWithAttachments(List<int> selectedFilesIds, string userEmail)
        {
            var selectedFiles = await _admin.GetFilesSelectedByFileID(selectedFilesIds);

            if (selectedFiles.Any())
            {
                var subject = "HalloDoc Files";

                var message = "Please Find Attachments";
                var link = "";
                var attachmentFilePaths = selectedFiles.Select(file => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName)).ToList();
                _customService.SendEmail(userEmail, link, subject, message, attachmentFilePaths);
            }

            return RedirectToAction("Dashboard");
        }
        #endregion
        #endregion

        #region provider

        #region provider info
        [HttpGet]
        public IActionResult ProviderInformation()
        {
            var result = _admin.RegionList();
            return View(result);
        }
        [HttpPost]
        public IActionResult ProviderInformation(int RegionId)
        {
            var result = _admin.ProviderInformation(RegionId);
            return PartialView("_ProviderTable", result);
        }

        public IActionResult ContactProvider(int id)
        {
            var result = _admin.ContectProviderModel(id);
            return PartialView("_ContactProviderPartialView", result);
        }


        [HttpPost]
        public IActionResult ContectProvider2(ContactProviderViewModel model)
        {
            var result = _admin.ContectProvider(model.physicianId);

            var link = "";
            var subject = "Admin is trying to connect with you";
            var body = model.message;
            List<string> attachmentFilePaths = null;

            _customService.SendEmail(result.Email, link, subject, body, attachmentFilePaths);

            return Json("success");
        }
        [HttpPost]
        public IActionResult StopNotificationPhysician(List<int> ids)
        {
            _admin.StopNotificationPhysician(ids);
            return Json("success");
        }
        #endregion

        #region Create Provider
        public async Task<IActionResult> CreateProvider()
        {
            var result = await _admin.Createprovider();
            return View(result);
        }
        [HttpPost]
        public IActionResult CreateProvider(ProviderViewModel model)
        {
            var result = _admin.CreateProvider(model);
            return RedirectToAction("Dashboard");
        }
        #endregion

        #region Create Admin
        public async Task<IActionResult> CreateAdmin()
        {
            var result  = await _admin.CreateAdmin();
            return View(result);
        }
        [HttpPost]
        public IActionResult CreateAdmin(AdminMyProfileViewModel model)
        {
            var result = _admin.CreateAdmin(model);
            return RedirectToAction("UserAccess",result);
        }
        #endregion

        #region edit provider details
        public async Task<IActionResult> EditProvider(int id)
        {
            var result = await _admin.EditProvider(id);
            return View(result);
        }
        public IActionResult DeleteProvider(int id)
        {
            _admin.DeleteProvider(id);
            return RedirectToAction("ProviderInformation");
        }
        #endregion

        #region save provider
        public async Task<IActionResult> changeRoleStatus(ProviderViewModel model, int id)
        {
            await _admin.resetRoleStatus(model, id);
            return Json("success");
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordProvider(int physicianId, string newPassword)
        {
            await _admin.resetPasswordProvider(physicianId, newPassword);
            return Json("success");
        }
        public IActionResult savePhysicianInformation(ProviderViewModel model, int id)
        {
            var result = _admin.savePhysicianInformation(model, id);
            model.PhysicianId = result.PhysicianId;
            return Json("success");
        }
        public IActionResult saveBillingInformation(ProviderViewModel model, int id)
        {
            var result = _admin.saveBillingInformation(model, id);
            return Json("success");
        }
        public IActionResult providerProfile(ProviderViewModel model, int id)
        {
            var result = _admin.providerProfile(model, id);
            return Json("success");
        }
        #endregion

        #region Account Access

        public async Task<IActionResult> AccountAccess()
        {
            var result = await _admin.AccountAccessTable();
            return View(result);
        }

        #endregion

        #region Create Role
        public IActionResult CreateRole()
        {
            var AdminId = HttpContext.Session.GetInt32("AdminSession");
            var data = _admin.CreateRole((int)AdminId);
            return View(data);
        }
        [HttpPost]
        public IActionResult MenuName(int accountTypeId, int typename, int id = 0)
        {
            var data = _admin.MenuName(accountTypeId, typename, id);
            return PartialView("_MenuPartialView", data);

        }
        public async Task<IActionResult> CreateAccess(AccountAccessViewModel model)
        {
            await _admin.CreateAccess(model);
            return RedirectToAction("CreateRole");

        }
        #endregion

        #region Edit Role
        public IActionResult EditAccountAccess(int id)
        {
            var AdminId = HttpContext.Session.GetInt32("AdminSession");
            var data = _admin.EditAccountAccess(id, (int)AdminId);
            return View(data);
        }
        public IActionResult submitEditAccess(AccountAccessViewModel viewModel)
        {
            var result = _admin.submitEditAccess(viewModel);
            return RedirectToAction("AccountAccess");
        }
        public IActionResult deleteAccess(int id)
        {
            var result = _admin.deleteAccountAccess(id);
            return RedirectToAction("AccountAccess");

        }
        #endregion

        #region User Access
        public IActionResult UserAccess()
        {
            return View();
        }
        public IActionResult UserAccessTable(int AaccountTypeId)
        {
            var result = _admin.UserAccess(AaccountTypeId);
            return PartialView("_UserAccessTable", result);
        }
        #endregion

        #region Vendors
        public IActionResult VendorsDetails()
        {
            var result = _admin.VendorDetail();
            return View(result);
        }
        public IActionResult VendorTable(int VendorProfessionTypeId,string VendorName)
        {
            var result = _admin.VendorTable(VendorProfessionTypeId,VendorName);
            return PartialView("_VendorsTable", result);
        }
        public IActionResult AddVendor()
        {
            var result = _admin.AddVendor();
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddVendor(VendorsViewModel model)
        {
            await _admin.AddVendor(model);
            return RedirectToAction("VendorsDetails");
        }
        public IActionResult EditVendor(int id)
        {
            var result = _admin.EditVendor(id);
            return View(result);
        }
        #endregion

        #region Email Logs
        public IActionResult EmailLogs()
        {
            var result = _admin.EmailLogs();
            return View(result);
        }
        public IActionResult EmailLogTable(int RoleID, string ReciverName, string email, DateTime? CreatedDate, DateTime? SentDate,int type)
        {
            var result = _admin.EmailLogTable(RoleID,ReciverName,email,CreatedDate,SentDate,type);
            return PartialView("_EmailLogTable",result);
        }
        #endregion

        #region SMS Logs
        public IActionResult SMSLogs()
        {
            var result = _admin.EmailLogs();
            return View(result);
        }
        #endregion

        #region Patient History
        public IActionResult PatientHistory()
        {
            
            return View();
        }
        public IActionResult PatientHistoryTable(string FirstName, string LastName, string Email, string PhoneNumber, int CurrentPage=1)
        {
            var result = _admin.PatientHistory(FirstName, LastName, Email, PhoneNumber, CurrentPage);
            return PartialView("_PatientHistory", result);
        }
        #endregion

        #endregion

    }
}