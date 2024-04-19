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
using System.Drawing;
using System.Globalization;
using Twilio.Types;
using Twilio;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Twilio.Rest.Api.V2010.Account;

namespace HalloDoc.Controllers
{
    [CustomAuthorize("1")]
    public class AdminController : Controller
    {
        private IAdminService _admin;
        private IJwtService _jwtService;
        private ICustomService _customService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public AdminController(IAdminService admin, IJwtService jwtService, ICustomService customService, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {

            _admin = admin;
            _jwtService = jwtService;
            _customService = customService;
            _hostingEnvironment = hostingEnvironment;
        }

        #region Logout
        public IActionResult Logout()
        {


            HttpContext.Session.Clear();
            Response.Cookies.Delete("jwt");
            Response.Cookies.Delete("RoleMenu");
            Response.Cookies.Delete("AdminID");
            Response.Cookies.Delete("UserNameAdmin");
            Response.Cookies.Delete("AspNetIdAdmin");

            return RedirectToAction("AdminLogin", "Custom");
        }


        #endregion

        #region Dashboard
        [RoleAuthorize(9)]
        public async Task<IActionResult> Dashboard()
        {

            var viewModel = new AdminDashboardViewModel()
            {
                NewCount = await _customService.GetCount(1),
                PendingCount = await _customService.GetCount(2),
                ActiveCount = await _customService.GetCount(4),
                ConcludeCount = await _customService.GetCount(6),
                ToCloseCount = await _customService.GetCount(3) + await _customService.GetCount(7) + await _customService.GetCount(8),
                UnpaidCount = await _customService.GetCount(9),

            };
            viewModel.State = _admin.getstateDropdown();

            return View(viewModel);
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
                        var result = _admin.Admintbl(state, list, 1);
                        var paging = _admin.Pagination(state, CurrentPage, PatientName, ReqType, RegionId, result);
                        return PartialView("_TablePartialView", paging);

                    case "Pending":
                        var result2 = _admin.Admintbl(state, list, 2);
                        var paging2 = _admin.Pagination(state, CurrentPage, PatientName, ReqType, RegionId, result2);

                        return PartialView("_TablePartialView", paging2);

                    case "Active":
                        var result3 = _admin.Admintbl(state, list, 4);
                        var paging3 = _admin.Pagination(state, CurrentPage, PatientName, ReqType, RegionId, result3);

                        return PartialView("_TablePartialView", paging3);

                    case "Conclude":
                        var result4 = _admin.Admintbl(state, list, 6);
                        var paging4 = _admin.Pagination(state, CurrentPage, PatientName, ReqType, RegionId, result4);

                        return PartialView("_TablePartialView", paging4);

                    case "Toclose":

                        var result5 = _admin.Admintbl(state, list, 3);
                        result5.AddRange(_admin.Admintbl(state, list, 7));
                        result5.AddRange(_admin.Admintbl(state, list, 8));

                        var paging5 = _admin.Pagination(state, CurrentPage, PatientName, ReqType, RegionId, result5);

                        return PartialView("_TablePartialView", paging5);

                    case "Unpaid":
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

        public IActionResult RoleAuthorization()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExportFilterWise(string state, int CurrentPage, string? PatientName, int? ReqType, int? RegionId)
        {
            int data;
            List<AdminDashboardViewModel> list = new List<AdminDashboardViewModel>();

            switch (state)
            {
                case "New":
                    data = 1;
                    break;

                case "Pending":
                    data = 2;
                    break;
                case "Active":
                    data = 4;
                    break;

                case "Conclude":
                    data = 6;
                    break;

                case "Toclose":
                    data = 3;
                    break;

                case "Unpaid":
                    data = 9;
                    break;

                default:
                    data = 1;
                    break;
            }
            var result1 = _admin.Admintbl(state, list, data);
            var paging6 = _admin.Pagination(state, 0, PatientName, ReqType, RegionId, result1).PagingData;
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
            {


                csvWriter.WriteRecords(paging6);
                writer.Flush();
                var result = memoryStream.ToArray();
                var fileName = "HalloDoc_exportData_" + Guid.NewGuid().ToString() + ".csv";
                var webRootPath = _hostingEnvironment.WebRootPath;
                var filePath = Path.Combine(webRootPath, "uploads", fileName);

                System.IO.File.WriteAllBytes(filePath, result);

                var fileUrl = Url.Content("~/uploads/" + fileName);
                return Ok(fileUrl);
            }
        }

        [HttpGet]
        public IActionResult ExportAll()
        {
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(_admin.ExportAllData());
                writer.Flush();
                var result = memoryStream.ToArray();
                var fileName = "HalloDoc_ExportALLData_" + Guid.NewGuid().ToString() + ".csv";
                var webRootPath = _hostingEnvironment.WebRootPath;
                var filePath = Path.Combine(webRootPath, "uploads", fileName);

                System.IO.File.WriteAllBytes(filePath, result);

                var fileUrl = Url.Content("~/uploads/" + fileName);
                return Ok(fileUrl);
            }
        }

        [HttpPost]
        public IActionResult sendMailToAllPhysicians(string notess)
        {
            var result = _admin.UnscheduledPhysicians(notess);
            if (result == null)
            {
                TempData["success"] = "Email is sent successfully to Unscheduled Physicians";
            }
            else
            {
                TempData["error"] = "No Physicians available to provide service.";
            }

            return Json("success");


        }
        public IActionResult sendLink()
        {
            return PartialView("_SendLink");
        }
        [HttpPost]
        public IActionResult SendLinkPatient(string email)
        {
            if (email != null)
            {
                var link = Request.Scheme + "://" + Request.Host + "/Custom/PatientRequest/" + email;
                var subject = "Create Request";
                var body = "Click here " + "<a href=" + link + ">Create New Request</a>" + ".";
                _customService.SendEmail(email, link, subject, body, 0, 0,0, null);

                TempData["success"] = "Email is sent successfully to your email account";
            }

            return RedirectToAction("Dashboard");
        }
        #endregion

        #region Create Admin
        public async Task<IActionResult> CreateAdmin()
        {
            var result = await _admin.CreateAdmin();
            return View(result);
        }
        [HttpPost]
        public IActionResult CreateAdmin(AdminMyProfileViewModel model)
        {
            var result = _admin.CreateAdmin(model);
            return RedirectToAction("UserAccess", result);
        }
        #endregion

        #region View Case
        [HttpGet]
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
        public async Task<IActionResult> ViewNotes(string additionalNotes, int id)
        {
            if (ModelState.IsValid)
            {
                var request = HttpContext.Request;
                string AdminID = request.Cookies["AspNetIdAdmin"];

                await _admin.AddNotes(additionalNotes, id, AdminID);
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
                var request = HttpContext.Request;
                int AdminID = Int32.Parse(request.Cookies["AdminID"]);

                await _admin.ConfirmCancelCase(viewModel, id, AdminID);

                return Json(new { success = true });
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
                var request = HttpContext.Request;
                int AdminID = Int32.Parse(request.Cookies["AdminID"]);
                await _admin.AssignRequest(viewModel, id, AdminID);

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
                var request = HttpContext.Request;
                int AdminID = Int32.Parse(request.Cookies["AdminID"]);
                await _admin.BlockCaseRequest(viewModel, id, AdminID);

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
                var request = HttpContext.Request;
                int AdminID = Int32.Parse(request.Cookies["AdminID"]);
                await _admin.TransferRequest(viewModel, id, AdminID);
                return Json(new { success = true });

            }
            return View();
        }

        #endregion

        #region Clear Case
        public async Task<IActionResult> ClearCase(int id)
        {
            ViewBag.id = id;

            return PartialView("_ClearCasePartialView");
        }
        public async Task<IActionResult> ClearRequest(int id)
        {
            if (ModelState.IsValid)
            {
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
                var request = HttpContext.Request;
                int AdminID = Int32.Parse(request.Cookies["AdminID"]);
                var result = await _admin.ConfirmCloseCase(id, AdminID);
                TempData["success"] = "Request Moved to UnPaid";
                return Json(new { success = true });
            }
            return RedirectToAction("Dashboard");
        }
        #endregion

        #region View Uploads

        public async Task<IActionResult> ViewUploads(int Id)
        {
            var request = HttpContext.Request;
            ViewBag.MySession = request.Cookies["UserNameAdmin"];
            var result = await _customService.ViewDocument(Id);
            return View(result);
        }

        [HttpPost]

        public async Task<IActionResult> ViewUploads(IFormFile a, int Id)
        {
            var request = HttpContext.Request;
            int AdminID = Int32.Parse(request.Cookies["AdminID"]);
            await _customService.ViewDocument(a, Id, AdminID, 0);
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

        public async Task<IActionResult> DeleteFile(int fileID, int RequstId)
        {
            await _admin.DeleteFile(fileID, RequstId);
            TempData["success"] = "Deleted Successfully!";
            return RedirectToAction("ViewUploads", new { Id = RequstId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFileByChecked(List<int> documentValues, int requestID)
        {
            if (documentValues.Count() > 0)
            {
                foreach (var items in documentValues)
                {
                    await _admin.DeleteFile(items, requestID);
                }
            }
            else
            {
                await _admin.DeleteFile(0, requestID);
            }
            return Json(documentValues);
        }
        #endregion

        #region Send Order
        [RoleAuthorize(13)]
        [HttpGet]
        public async Task<IActionResult> SendOrder(int Id)
        {
            var result = await _customService.SendOrder(Id);
            return View(result);
        }
        [HttpGet]

        public async Task<IActionResult> GetBusinessByProfession(int ProfessionID)
        {
            var physicians = await _customService.GetBusinessByProfession(ProfessionID);
            return Json(physicians);
        }

        [HttpGet]

        public async Task<IActionResult> GetBusinessDetails(int BusinessId)
        {
            var result = await _customService.GetBusinessDetails(BusinessId);
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
                var request = HttpContext.Request;
                string AdminID = request.Cookies["AspNetIdAdmin"];


                await _customService.SendOrderDetails(viewModel, Id, AdminID);

            }
            return Json(new { success = true });

        }
        #endregion

        #region send agreement
        public async Task<IActionResult> SendAgreement(int id)
        {
            ViewBag.AgreementIdToClient = id;
            var result = await _customService.sendAgreement(id);
            return PartialView("_SendAgreementPartialView", result);
        }
        public async Task<IActionResult> SendAgreementLink(ViewCaseViewModel viewModel)
        {
            var request = HttpContext.Request;
            int AdminId = Int32.Parse(request.Cookies["AdminID"]);

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
                _customService.SendEmail(viewModel.Email, link, subject, body, viewModel.requestclientID, AdminId,0, attachmentFilePaths);


                TempData["emailsend"] = "Email is sent successfully to your email account";
                return RedirectToAction("Dashboard", "Admin");
            }
            return View(viewModel);
        }
        #endregion

        #region Admin MyProfile
        [RoleAuthorize(3)]
        public async Task<IActionResult> AdminMyProfile()
        {
            var request = HttpContext.Request;
            int AdminID = Int32.Parse(request.Cookies["AdminID"]);
            var result = await _admin.AdminMyProfile(AdminID);
            ViewBag.PageHeader = "My Profile";

            return View(result);
        }
        public async Task<IActionResult> ResetAdminPassword(AdminMyProfileViewModel model)
        {
            await _admin.ResetPasswordAdmin(model);
            TempData["success"] = "Password Updated Successfully.";
            return RedirectToAction("AdminMyProfile");
        }
        public async Task<IActionResult> SaveAdminInfo(AdminMyProfileViewModel model, List<int> notification)
        {
            await _admin.SaveAdminInfo(model, notification);
            return Json(new { success = true });
        }
        public async Task<IActionResult> SaveBillingInfo(AdminMyProfileViewModel model)
        {
            await _admin.SaveBillingInfo(model);
            return Json(new { success = true });

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
                TempData["success"] = "Data Saved Successfully.";
                return RedirectToAction("EncounterForm", result);
            }
            return View(model);
        }
        #endregion

        #region send Mail Attachment
        public async Task<IActionResult> SendEmailWithAttachments(List<int> selectedFilesIds, string userEmail)
        {
            var selectedFiles = await _customService.GetFilesSelectedByFileID(selectedFilesIds);

            if (selectedFiles.Any())
            {
                var subject = "HalloDoc Files";

                var message = "Please Find Attachments";
                var link = "";
                var attachmentFilePaths = selectedFiles.Select(file => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName)).ToList();
                _customService.SendEmail(userEmail, link, subject, message, 0, 0,0, attachmentFilePaths);
            }

            return RedirectToAction("Dashboard");
        }
        #endregion

        #region provider info
        [HttpGet]
        public IActionResult ProviderInformation()
        {
            var result = _admin.RegionList();
            return View(result);
        }
        [HttpPost]
        public IActionResult ProviderInformation(int RegionId, int CurrentPage = 1)
        {
            var result = _admin.ProviderInformation(RegionId, CurrentPage);
            return PartialView("_ProviderTable", result);
        }

        public IActionResult ContactProvider(int id)
        {
            var result = _admin.ContectProviderModel(id);
            return PartialView("_ContactProviderPartialView", result);
        }


        [HttpPost]
        public IActionResult ContectProvider2(ContactProviderViewModel model, int flexRadioDefault)
        {
            var request = HttpContext.Request;
            int AdminID = Int32.Parse(request.Cookies["AdminID"]);
            if (flexRadioDefault == 2)
            {
                // var result = _admin.ContectProvider(model.physicianId);

                var link = "";
                var subject = "Admin is trying to connect with you";
                var body = model.message;
                List<string> attachmentFilePaths = null;

                _customService.SendEmail(model.email, link, subject, body, 0, AdminID,0, attachmentFilePaths);

                return Json("success");
            }
            else if (flexRadioDefault == 1)
            {
                _admin.sendSMS(model, AdminID);
                return Json("success");
            }
            else
            {
                // var result = _admin.ContectProvider(model.physicianId);

                var link = "";
                var subject = "Admin is trying to connect with you";
                var body = model.message;
                List<string> attachmentFilePaths = null;

                _customService.SendEmail(model.email, link, subject, body, 0, AdminID,0, attachmentFilePaths);
                _admin.sendSMS(model, AdminID);

                return Json("success");

            }
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
            return RedirectToAction("ProviderInformation");
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
            TempData["success"] = "Provider Deleted Successfully.";
            return RedirectToAction("ProviderInformation");
        }
        #endregion

        #region save provider
        public async Task<IActionResult> changeRoleStatus(ProviderViewModel model, int id)
        {
            await _admin.resetRoleStatus(model, id);
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordProvider(int physicianId, string newPassword)
        {
            await _admin.resetPasswordProvider(physicianId, newPassword);
            return Json("success");
        }
        public IActionResult savePhysicianInformation(ProviderViewModel model, int id, List<int> notification)
        {
            var result = _admin.savePhysicianInformation(model, id, notification);
            model.PhysicianId = result.PhysicianId;
            return Json(new { success = true });
        }
        public IActionResult saveBillingInformation(ProviderViewModel model, int id)
        {
            var result = _admin.saveBillingInformation(model, id);
            return Json(new { success = true });

        }
        public IActionResult providerProfile(ProviderViewModel model, int id)
        {
            var result = _admin.providerProfile(model, id);
            return Json(new { success = true });
        }
        public IActionResult documentsProvider(ProviderViewModel model)
        {
            var result = _admin.documentsProvider(model);
            return Json(new { success = true });

        }
        #endregion

        #region Account Access

        public async Task<IActionResult> AccountAccess()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AccountAccess(int CurrentPage = 1)
        {
            var result = await _admin.AccountAccessTable(CurrentPage);
            return PartialView("_AccountAccessPartialView", result);
        }
        #endregion

        #region Create Role
        public IActionResult CreateRole()
        {
            var request = HttpContext.Request;
            int AdminID = Int32.Parse(request.Cookies["AdminID"]);
            var data = _admin.CreateRole((int)AdminID);
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
            var request = HttpContext.Request;
            string adminid = request.Cookies["AspNetIdAdmin"];
            await _admin.CreateAccess(model, adminid);
            return RedirectToAction("CreateRole");

        }
        #endregion

        #region Edit Role
        public IActionResult EditAccountAccess(int id)
        {
            var request = HttpContext.Request;
            int AdminId = Int32.Parse(request.Cookies["AdminID"]);
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

        public IActionResult UserAccessTable(int AaccountTypeId, int CurrentPage = 1)
        {
            var result = _admin.UserAccess(AaccountTypeId, CurrentPage);
            return PartialView("_UserAccessTable", result);
        }

        public async Task<IActionResult> EditAdminAccount(int id)
        {

            var result = await _admin.AdminMyProfile(id);
            ViewBag.PageHeader = "Edit Admin Account";
            return View("AdminMyProfile", result);
        }

        #endregion

        #region Vendors
        [RoleAuthorize(12)]
        public IActionResult VendorsDetails()
        {
            var result = _admin.VendorDetail();
            return View(result);
        }
        public IActionResult VendorTable(int VendorProfessionTypeId, string VendorName, int CurrentPage = 1)
        {
            var result = _admin.VendorTable(VendorProfessionTypeId, VendorName, CurrentPage);
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

        public IActionResult DeleteVendor(int id)
        {
            _admin.DeleteVendor(id);

            return RedirectToAction("VendorsDetails");
        }
        #endregion

        #region Email Logs
        [RoleAuthorize(14)]
        public IActionResult EmailLogs()
        {
            var result = _admin.Logs();
            return View(result);
        }
        public IActionResult LogTable(int RoleID, string ReciverName, string email, string phoneNo, DateTime? CreatedDate, DateTime? SentDate, int type, int CurrPage = 1)
        {
            var result = _admin.LogTable(RoleID, ReciverName, email, phoneNo, CreatedDate, SentDate, type, CurrPage);
            return PartialView("_EmailLogTable", result);
        }
        #endregion

        #region SMS Logs
        [RoleAuthorize(15)]
        public IActionResult SMSLogs()
        {
            var result = _admin.Logs();
            return View(result);
        }
        #endregion

        #region Patient History
        [RoleAuthorize(2)]
        public IActionResult PatientHistory()
        {

            return View();
        }
        public IActionResult PatientHistoryTable(string FirstName, string LastName, string Email, string PhoneNumber, int CurrentPage = 1)
        {
            var result = _admin.PatientHistory(FirstName, LastName, Email, PhoneNumber, CurrentPage);
            return PartialView("_PatientHistory", result);
        }
        #endregion

        #region Patient Records
        public IActionResult PatientRecord(int id)
        {
            PatientRecordViewModel viewModel = new PatientRecordViewModel();
            viewModel.userID = id;
            return View(viewModel);
        }
        public IActionResult PatientRecordTable(int id, int CurrentPage = 1)
        {
            var result = _admin.PatientRecordTable(id, CurrentPage);
            return PartialView("_PatientRecord", result);
        }
        #endregion

        #region Block History
        [RoleAuthorize(7)]
        public IActionResult BlockHistory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BlockHistoryTable(string FirstName, DateTime? Date, string Email, string PhoneNumber, int CurrPage = 1)
        {
            var result = _admin.BlockHistoryTable(FirstName, Date, Email, PhoneNumber, CurrPage);
            return PartialView("_BlockHistoryTable", result);
        }
        public IActionResult UnblockRequest(int id)
        {
            _admin.UnblockRequest(id);
            return RedirectToAction("BlockHistory");
        }
        #endregion

        #region Search Record
        [RoleAuthorize(8)]
        public IActionResult SearchRecord()
        {

            return View();
        }
        [HttpPost]
        public IActionResult SearchRecordTable(int statusOfRequest, string Name, int requestType, DateTime? DateOfService, DateTime? ToDateOfService, string physician, string Email, string PhoneNumber, int CurrPage = 1)
        {
            var result = _admin.SearchRecordTable(statusOfRequest, Name, requestType, DateOfService, ToDateOfService, physician, Email, PhoneNumber, CurrPage);
            return PartialView("_SearchRecordTable", result);
        }
        public IActionResult DeleteRequest(int id)
        {
            _admin.DeleteRequest(id);
            return RedirectToAction("SearchRecord");
        }
        #endregion

        #region Create Request Admin
        public IActionResult CreateRequestByAdmin()
        {
            PatientRequestViewModel model = new PatientRequestViewModel();
            model.State = _admin.getstateDropdown();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRequest(PatientRequestViewModel model)
        {
            var request = HttpContext.Request;
            int AdminId = Int32.Parse(request.Cookies["AdminID"]);
            await _admin.CreateRequestByAdmin(model, (int)AdminId);
            //await LinkToCreateAccount(new CreateAccountViewModel
            //{
            //    Email = viewModel.Email
            //});
            TempData["success"] = "Request Created Successfully.";

            return RedirectToAction("Dashboard");
        }
        #endregion

        #region Scheduling
        [RoleAuthorize(10)]
        public IActionResult Scheduling()
        {
            SchedulingModel model = new()
            {
                regions = _admin.getstateDropdown(),
            };
            return View(model);
        }
        public IActionResult getCal(int regionId)
        {
            var result = _admin.Scheduling(regionId);
            return Json(result);
        }
        #endregion

        #region Create Shift
        public IActionResult CreateShift()
        {

            CreateShiftViewModel model = new()
            {
                regions = _admin.getstateDropdown(),
            };
            return PartialView("_createShift", model);
        }
        [HttpPost]
        public IActionResult AddShift(CreateShiftViewModel model, List<DayOfWeek> WeekDays)
        {
            var request = HttpContext.Request;
            string AdminID = request.Cookies["AspNetIdAdmin"];
            _admin.AddShift(model, WeekDays, AdminID);
            return RedirectToAction("Scheduling");
        }
        #endregion

        #region EditShift
        [HttpGet]
        public IActionResult GetShiftDetailsById(int shiftDetailsId)
        {
            return Json(new { data = _admin.GetShiftDetailsById(shiftDetailsId) });
        }
        [HttpPost]
        public IActionResult EditShiftData(CreateShiftViewModel shiftData)
        {
            _admin.EditShiftData(shiftData);
            return Json("success");
        }
        public IActionResult returnShift(int id)
        {
            _admin.returnShift(id);

            return Json("success");

        }

        public IActionResult deleteShift(int id)
        {
            _admin.deleteShift(id);

            return Json("success");
        }
        #endregion

        #region Requested Shift
        public IActionResult RequestedShift()
        {
            RequestedShiftViewModel model = new()
            {
                regions = _admin.getstateDropdown(),
            };

            return View(model);
        }
        public IActionResult RequestedShiftTable(int month, int regionId, int CurrentPage = 1)
        {

            var result = _admin.RequestedShift(month, regionId, CurrentPage);
            return PartialView("_RequestedShiftTable", result);
        }
        public IActionResult ApproveShift(List<int> documentValues)
        {
            _admin.ApproveShift(documentValues);
            return RedirectToAction("RequestedShift");
        }
        public IActionResult DeleteSelectedShift(List<int> documentValues)
        {
            _admin.deleteshifts(documentValues);
            return RedirectToAction("RequestedShift");
        }
        #endregion

        #region Provider Location
        [RoleAuthorize(5)]
        public IActionResult ProviderLocation()
        {
            ViewBag.Locations = _admin.ProviderLocation();
            return View();
        }
        #endregion

        #region MD's On call
        public IActionResult MdOnCall()
        {

            ProvidersOnCallViewModel model = new()
            {
                regions = _admin.getstateDropdown(),
            };
            return View(model);
        }
        public IActionResult GetProvidersOnCall(int regionId = 0)
        {
            var result = _admin.GetProvidersOnCall(regionId);
            return PartialView("_MDonCallPartialView", result);
        }
        #endregion

    }
}