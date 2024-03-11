using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using HalloDoc.Repository.Repository;
using HalloDoc.Services.IServices;
using HalloDoc.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using System.Diagnostics.Contracts;

namespace HalloDoc.Controllers
{
    [CustomAuthorize("1")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IAdminService _admin;
        private IJwtService _jwtService;


        public AdminController(ApplicationDbContext context, IAdminService admin, IJwtService jwtService)
        {
            _context = context;
            _admin = admin;
            _jwtService = jwtService;
        }

      
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

        public IActionResult New()
        {
            List<AdminDashboardViewModel> list = _admin.New();
            return PartialView("_NewPartialView", list);
        }

        public IActionResult Pending()
        {
            List<AdminDashboardViewModel> list = _admin.Pending();

            return PartialView("_PendingPartialView", list);
        }

        public IActionResult Active()
        {
            List<AdminDashboardViewModel> list = _admin.Active();

            return PartialView("_ActivePartialView", list);
        }

        public IActionResult Conclude()
        {
            List<AdminDashboardViewModel> list = _admin.Conclude();

            return PartialView("_ConcludePartialView", list);
        }

        public IActionResult ToClose()
        {
            List<AdminDashboardViewModel> list = _admin.ToClose();

            return PartialView("_ToClosePartialView", list);
        }

        public IActionResult Unpaid()
        {
            List<AdminDashboardViewModel> list = _admin.Unpaid();

            return PartialView("_UnpaidPartialView", list);
        }

        #endregion

        #region View Case

        public async Task<IActionResult> ViewCase(int Id)
        {
            var result = await _admin.ViewCase(Id);
            return View(result);
        }
        [HttpPost]

        public async Task<IActionResult> ViewCase(ViewCaseViewModel viewmodel, int Id)
        {
            var result = await _admin.EditNewRequest(viewmodel, Id);
            return View(result);
        }

        #endregion

        #region View Notes

        public async Task<IActionResult> ViewNotes(AdminDashboardViewModel viewModel, int Id)
        {
            var result = await _admin.ViewNotes(viewModel, Id);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> ViewNotes(AdminDashboardViewModel viewModel, int Id, int x)
        {
            var result = await _admin.AddNotes(viewModel, Id);
            return View(result);
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
            var result = await _admin.ConfirmCancelCase(viewModel, id);
            return RedirectToAction("Dashboard");
        }
        #endregion

        #region Assign Case

        public async Task<IActionResult> AssignCase(AssignCaseViewModel viewModel, int id)
        {
            var result = await _admin.AssignCase(viewModel, id);
            return PartialView("_AssignRequestPartialView", result);
        }

        public async Task<IActionResult> AssignRequest(AssignCaseViewModel viewModel, int id)
        {
            await _admin.AssignRequest(viewModel, id);
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
            var result = await _admin.BlockCaseRequest(viewModel, id);
            return RedirectToAction("Dashboard");
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
            await _admin.TransferRequest(viewModel, id);
            return RedirectToAction("Dashboard");
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
            var id = HttpContext.Session.GetInt32("requestID");

            await _admin.ClearRequest(id);
            return RedirectToAction("Dashboard");

        }
        #endregion

        #region View Uploads

        public async Task<IActionResult> ViewUploads(int Id)
        {
            HttpContext.Session.SetInt32("reqID", Id);
            ViewBag.MySession = HttpContext.Session.GetString("UserName");
            var result = await _admin.ViewDocument(Id);
            return View(result);
        }

        [HttpPost]

        public async Task<IActionResult> ViewUploads(IFormFile a, int Id)
        {
            await _admin.ViewDocument(a, Id);
            return RedirectToAction("ViewUploads");
        }

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

        public async Task<IActionResult> DownloadAll(IEnumerable<int> documentValues)
        {
            if (documentValues.Count() > 0)
            {
                byte[] fileBytes = await _admin.DownloadAllByChecked(documentValues);
                string zipFileData = Convert.ToBase64String(fileBytes);
                return Json(new { success = true, zipFileData });

            }
            else
            {
                int? requestid = HttpContext.Session.GetInt32("reqID");

                byte[] fileBytes = await _admin.DownloadAll(documentValues, requestid);
                return File(fileBytes, "application/zip", "download.zip");
            }
        }

        public async Task<IActionResult> DeleteFile(int fileID)
        {
            int? requstId = HttpContext.Session.GetInt32("reqID");
            await _admin.DeleteFile(fileID);

            return RedirectToAction("ViewUploads", new { Id = requstId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFileByChecked(List<int> documentValues)
        {
            foreach (var items in documentValues)
            {

                await _admin.DeleteFile(items);
            }

            return Json(documentValues);
        }
        #endregion

        #region Send Order

        public async Task<IActionResult> SendOrder(SendOrderViewModel viewModel, int Id)
        {
            try
            {
                var result = await _admin.SendOrder(viewModel, Id);
                return View(result);
            }
            catch (Exception ex)
            {
                return View(ex);
            }
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

            await _admin.SendOrderDetails(viewModel, Id);
            return RedirectToAction("Dashboard");

        }
        #endregion

        #region send agreement
        public async Task<IActionResult> SendAgreement(ViewCaseViewModel viewModel, int id)
        {
            ViewBag.AgreementIdToClient = id;   
            var result = await _admin.sendAgreement(viewModel, id);
            return PartialView("_SendAgreementPartialView", result);
        }


        public async Task<IActionResult> SendAgreementLink(ViewCaseViewModel viewModel)
        {
            if (viewModel.Email == null)
            {
                TempData["emptyemail"] = "Please enter Email";
                return RedirectToAction("SendAgreement", "Admin");
            }

            var link = Request.Scheme + "://" + Request.Host + "/Custom/ReviewAgreement?requestClinetID=" +viewModel.requestclientID;
            var msg = _admin.SendEmailAgreement(viewModel.Email, link);

            TempData["emailsend"] = "Email is sent successfully to your email account";
            return RedirectToAction("Dashboard", "Admin");
        }

       
        #endregion
    }
}
