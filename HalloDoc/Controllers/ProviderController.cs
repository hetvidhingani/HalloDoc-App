using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using HalloDoc.Repository.Repository;
using HalloDoc.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace HalloDoc.Controllers
{
    [CustomAuthorize("3")]

    public class ProviderController : Controller
    {
        #region constructor
        private IProviderService _provider;
        private IJwtService _jwtService;
        private ICustomService _customService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public ProviderController(IProviderService provider, IJwtService jwtService, ICustomService customService, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _provider = provider;
            _jwtService = jwtService;
            _customService = customService;
            _hostingEnvironment = hostingEnvironment;
        }
        #endregion

        #region Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("jwt");
            Response.Cookies.Delete("RoleMenu");
            Response.Cookies.Delete("ProviderID");
            Response.Cookies.Delete("UserNameProvider");
            Response.Cookies.Delete("AspNetIdProvider");
            return RedirectToAction("AdminLogin", "Custom");
        }
        #endregion

        #region Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var request = HttpContext.Request;
            int providerId = Int32.Parse(request.Cookies["ProviderID"]);

            var viewModel = new ProviderDashboardViewModel()
            {
                NewCount = await _provider.GetCount(1, providerId),
                PendingCount = await _provider.GetCount(2, providerId),
                ActiveCount = await _provider.GetCount(4, providerId) + await _provider.GetCount(5, providerId),
                ConcludeCount = await _provider.GetCount(6, providerId),
                PhysicianId = providerId,
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult GetTable(string state, int CurrentPage, string? PatientName, int? ReqType)
        {
            if (ModelState.IsValid)
            {
                List<ProviderDashboardViewModel> list = new List<ProviderDashboardViewModel>();
                var request = HttpContext.Request;
                int providerId = Int32.Parse(request.Cookies["ProviderID"]);
                switch (state)
                {
                    case "New":
                        var result = _provider.ProviderTable(providerId, state, list, 1);
                        var paging = _provider.Pagination(state, CurrentPage, PatientName, ReqType, result);

                        return PartialView("_ProviderDashboard", paging);

                    case "Pending":
                        var result2 = _provider.ProviderTable(providerId, state, list, 2);
                        var paging2 = _provider.Pagination(state, CurrentPage, PatientName, ReqType, result2);

                        return PartialView("_ProviderDashboard", paging2);

                    case "Active":
                        var result3 = _provider.ProviderTable(providerId, state, list, 4);
                        result3.AddRange(_provider.ProviderTable(providerId, state, list, 5));
                        var paging3 = _provider.Pagination(state, CurrentPage, PatientName, ReqType, result3);

                        return PartialView("_ProviderDashboard", paging3);

                    case "Conclude":
                        var result4 = _provider.ProviderTable(providerId, state, list, 6);
                        var paging4 = _provider.Pagination(state, CurrentPage, PatientName, ReqType, result4);

                        return PartialView("_ProviderDashboard", paging4);

                    default:
                        var result7 = _provider.ProviderTable(providerId, state, list, 1);
                        var paging7 = _provider.Pagination(state, CurrentPage, PatientName, ReqType, result7);

                        return PartialView("_ProviderDashboard", paging7);
                }
            }
            return View();
        }

        public IActionResult RoleAuthorization()
        {
            return View();
        }
        #endregion

        #region View Case
        [HttpGet]
        public async Task<IActionResult> ViewCase(int Id)
        {
            if (ModelState.IsValid)
            {
                var result = await _provider.ViewCase(Id);
                return View(result);
            }
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> ViewCase(ViewCaseViewModel viewmodel, int Id)
        {
            if (ModelState.IsValid)
            {
                await _provider.EditNewRequest(viewmodel, Id);
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
                var result = await _provider.ViewNotes(Id);
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
                string ProviderId = request.Cookies["AspNetIdProvider"];

                await _provider.AddNotes(additionalNotes, id, ProviderId);
                return Json("success");
            }
            return View();
        }
        #endregion

        #region Accept Case
        public async Task<IActionResult> AcceptCase(int id)
        {
            var request = HttpContext.Request;
            int providerId = Int32.Parse(request.Cookies["ProviderID"]);
            await _provider.AcceptCase(id, providerId);

            return Json(new { success = true });
        }
        #endregion

        #region Transfer Case
        public IActionResult TransferCase(int id)
        {
            ProviderDashboardViewModel model = new ProviderDashboardViewModel();
            model.requestClientId = id;
            return PartialView("_TransferCaseByProvider", model);
        }
        [HttpPost]
        public async Task<IActionResult> TransferRequest(int id, string note)
        {
            var request = HttpContext.Request;
            int providerId = Int32.Parse(request.Cookies["ProviderID"]);
            await _provider.TransferCase(id, providerId, note);
            return Json(new { success = true });
        }
        #endregion

        #region View Uploads

        public async Task<IActionResult> ViewUploads(int Id)
        {
            var request = HttpContext.Request;
            ViewBag.MySession = request.Cookies["UserNameProvider"];
            var result = await _customService.ViewDocument(Id);
            return View(result);
        }

        [HttpPost]

        public async Task<IActionResult> ViewUploads(IFormFile a, int Id)
        {
            var request = HttpContext.Request;
            int AdminID = Int32.Parse(request.Cookies["ProviderID"]);
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
            await _provider.DeleteFile(fileID, RequstId);
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
                    await _provider.DeleteFile(items, requestID);
                }
            }
            else
            {
                await _provider.DeleteFile(0, requestID);
            }
            return Json(documentValues);
        }
        #endregion

        #region send Mail Attachment
        public async Task<IActionResult> SendEmailWithAttachments(List<int> selectedFilesIds, string userEmail)
        {
            var request = HttpContext.Request;
            int providerId = Int32.Parse(request.Cookies["ProviderID"]);
            var selectedFiles = await _customService.GetFilesSelectedByFileID(selectedFilesIds);

            if (selectedFiles.Any())
            {
                var subject = "HalloDoc Files";

                var message = "Please Find Attachments";
                var link = "";
                var attachmentFilePaths = selectedFiles.Select(file => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName)).ToList();
                _customService.SendEmail(userEmail, link, subject, message, 0, 0, providerId, attachmentFilePaths);
            }

            return RedirectToAction("Dashboard");
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
            int providerId = Int32.Parse(request.Cookies["ProviderID"]);

            if (ModelState.IsValid)
            {
                if (viewModel.Email == null)
                {
                    TempData["emptyemail"] = "Please enter Email";
                    return RedirectToAction("SendAgreement", "Provider");
                }

                var link = Request.Scheme + "://" + Request.Host + "/Custom/ReviewAgreement?requestClinetID=" + viewModel.requestclientID;
                var subject = "Review Agreement";
                var body = "Click here " + "<a href=" + link + ">Agreement</a>" + " to Review Agreement!!!";
                List<string> attachmentFilePaths = null;
                _customService.SendEmail(viewModel.Email, link, subject, body, viewModel.requestclientID, 0, providerId, attachmentFilePaths);


                TempData["emailsend"] = "Email is sent successfully to your email account";
                return RedirectToAction("Dashboard", "Provider");
            }
            return View(viewModel);
        }
        #endregion

        #region Send Order

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
                string providerID = request.Cookies["AspNetIdProvider"];


                await _customService.SendOrderDetails(viewModel, Id, providerID);

            }
            return Json(new { success = true });

        }
        #endregion

        #region Type of Care
        public IActionResult HouseCallOrCounsult(int id, int requestId)
        {
            _provider.HouseCallOrCounsult(id, requestId);
            return Json(new { success = true });
        }
        #endregion

        #region conclude care
        public async Task<IActionResult> ConcludeCare(int id)
        {
            var request = HttpContext.Request;
            ViewBag.MySession = request.Cookies["UserNameProvider"];
            var result = await _customService.ViewDocument(id);
            return View(result);
        }
        public IActionResult ConcludeRequest(int id,string? note)
        {
            var request = HttpContext.Request;
            string ProviderId = request.Cookies["AspNetIdProvider"];
            _provider.ConcludeRequest(id,note, ProviderId);
            TempData["success"] = "Request Concluded successfully.";
            return RedirectToAction("Dashboard", "Provider");
        }
        #endregion

        #region Encounter Form
        public async Task<IActionResult> EncounterForm(int RequestId)
        {
            var result = await _provider.EncounterForm(RequestId);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> EncounterFormSaveChanges(EncounterViewModel model)
        {

            await _provider.EncounterFormSaveChanges(model);
            TempData["success"] = "Data saved successfully.";
            return Json(new { success = true });

        }
        public IActionResult FinalizeReport(int id)
        {
            _provider.FinalizeReport(id);
            TempData["success"] = "Medical Report is Finalized.";
            return Json(new { success = true });
        }
        #endregion

        #region My Profile
        public async Task<IActionResult> ResetPasswordProvider(int physicianId, string newPassword)
        {
            await _provider.resetPasswordProvider(physicianId, newPassword);
            return Json("success");
        }
        public async Task<IActionResult> MyProfile()
        {
            var request = HttpContext.Request;
            int id = Int32.Parse(request.Cookies["ProviderID"]);
            var result = await _provider.MyProfile(id);
            return View(result);
        }
        public IActionResult RequestAdminToEditProfile(ProviderViewModel model)
        {
            return Json("success");

        }
        #endregion
    }
}
