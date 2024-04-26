using HalloDoc.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using HalloDoc.Repository.IRepository;
using HalloDoc.Services.IServices;
using Microsoft.AspNetCore.Http;

using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using HalloDoc.Repository.IRepository;
using System.Data.Entity;
using System.Runtime.Intrinsics.X86;

namespace HalloDoc.Services.Services
{
    public class CustomService : ICustomService
    {
        #region constructor
        private readonly IAspNetUserRepository _aspnetuserRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestClientRepository _requestclientRepository;
        private readonly IRequestWiseFilesRepository _requestwisefileRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IConciergeRepository _conciergeRepository;
        private readonly IPhysicianRepository _physicianRepository;
        private readonly IRequestNotesRepository _requestNotesRepository;
        private readonly ICaseTagRepository _caseTagRepository;
        private readonly IRequestStatusLogRepository _requestStatusLogRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IHealthProfessionalsRepository _healthProfessionalsRepository;
        private readonly IHealthProfessionalTypeRepository _healthProfessionalTypeRepository;
        private readonly IBlockRequestRepository _blockRequestRepository;
        private readonly IEncounterRepository _encounterRepository;
        private readonly IRequestClosedRepository _requestClosedRepository;
        private readonly IAdminRegionRepository _adminRegionRepository;
        private readonly IAspNetUserRolesRepository _aspNetUserRolesRepository;
        private readonly IEmailLogsRepository _emailLogsRepository;

        public CustomService(IAspNetUserRepository aspnetuserRepository, IUserRepository userRepository,
                               IRequestRepository requestRepository, IRequestClientRepository requestclientRepository,
                               IRequestWiseFilesRepository requestwisefileRepository, IBusinessRepository businessRepository,
                               IConciergeRepository conciergeRepository,
                               IPhysicianRepository physicianRepository, IAdminRepository adminRepository, IRequestNotesRepository requestNotesRepository,
                               ICaseTagRepository caseTagRepository, IRequestStatusLogRepository requestStatusLogRepository, IRegionRepository regionRepository,
                               IOrderDetailsRepository orderDetailsRepository, IHealthProfessionalsRepository healthProfessionalsRepository,
                               IHealthProfessionalTypeRepository healthProfessionalTypeRepository, IBlockRequestRepository blockRequestRepository,
                               IAdminRegionRepository adminRegionRepository,
                               IEncounterRepository encounterRepository, IRequestClosedRepository requestClosedRepository, IAspNetUserRolesRepository aspNetUserRolesRepository,
                               IEmailLogsRepository emailLogsRepository)
        {
            _userRepository = userRepository;
            _aspnetuserRepository = aspnetuserRepository;
            _requestRepository = requestRepository;
            _requestclientRepository = requestclientRepository;
            _requestwisefileRepository = requestwisefileRepository;
            _businessRepository = businessRepository;
            _conciergeRepository = conciergeRepository;
            _physicianRepository = physicianRepository;
            _adminRepository = adminRepository;
            _requestNotesRepository = requestNotesRepository;
            _caseTagRepository = caseTagRepository;
            _requestStatusLogRepository = requestStatusLogRepository;
            _regionRepository = regionRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _healthProfessionalsRepository = healthProfessionalsRepository;
            _healthProfessionalTypeRepository = healthProfessionalTypeRepository;
            _blockRequestRepository = blockRequestRepository;
            _encounterRepository = encounterRepository;
            _requestClosedRepository = requestClosedRepository;
            _adminRegionRepository = adminRegionRepository;
            _aspNetUserRolesRepository = aspNetUserRolesRepository;
            _emailLogsRepository = emailLogsRepository;
        }
        #endregion

        #region check
        public AspNetUser checkEmailPassword(string email, string password)
        {
            AspNetUser user = _aspnetuserRepository.GetAll().Where(u => u.Email == email).FirstOrDefault();
            if (user != null)
            {

                var pwd = _aspnetuserRepository.DecodeFrom64(user.PasswordHash);
                var result = _aspnetuserRepository.GetAll().Where(u => u.Email == email && password == pwd).FirstOrDefault();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
        public AspNetUser checkIfExist(string email)
        {
            AspNetUser myUser = _aspnetuserRepository.GetAll().Where(x => x.Email == email).FirstOrDefault();
            return myUser;
        }

        public AspNetUserRole checkIfRoleExist(string id)
        {
            AspNetUserRole user = _aspNetUserRolesRepository.GetAll().Where(x => x.UserId == id).FirstOrDefault();
            return user;
        }
        public Physician GetPhysician(string email)
        {
            return _physicianRepository.GetPhysician(email);
        }
        #endregion

        #region Send Mail
        public async Task<List<RequestWiseFile>> GetFilesSelectedByFileID(List<int> selectedFilesIds)
        {
            return await _requestwisefileRepository.GetFilesSelectedByFileID(selectedFilesIds);
        }

        public string SendEmail(string email, string link, string subject, string body, int id, int AdminId, int PhysicianId, List<string> attachmentFilePath = null)
        {
            try
            {
                var senderMail = "tatva.dotnet.hetvidhingani@outlook.com";
                var senderPassword = "Hkd$9503";

                SmtpClient smtpClient = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(senderMail, senderPassword),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                };

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(senderMail),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = body,
                };

                if (attachmentFilePath != null && attachmentFilePath.Count > 0)
                {
                    foreach (var attachmentFilePaths in attachmentFilePath)
                    {
                        Attachment attachment = new Attachment(attachmentFilePaths);
                        mailMessage.Attachments.Add(attachment);
                    }
                }

                mailMessage.To.Add(email);
                smtpClient.Send(mailMessage);

                int? requestid = null;
                DateTime createdate = DateTime.Now;
                int? roleId = null;
                int? adminid = null;
                int? physicianId = null;
                if (id != 0)
                {
                    RequestClient req = _requestclientRepository.GetById(id);
                    Request request = _requestRepository.GetById(req.RequestId);
                    if (req != null)
                    {
                        requestid = request.RequestId;
                        createdate = request.CreatedDate;
                    }
                }
                if (AdminId != 0)
                {
                    Admin admin = _adminRepository.GetById(AdminId);
                    roleId = admin.RoleId;
                    adminid= admin.AdminId;
                }
                else if (PhysicianId != 0)
                {
                    Physician phy = _physicianRepository.GetById(PhysicianId);
                    roleId=phy.RoleId;
                    physicianId= phy.PhysicianId;
                }

                EmailLog log = new EmailLog();
                log.EmailTemplate = body;
                log.SubjectName = subject;
                log.EmailId = email;
                log.RoleId = roleId;
                log.RequestId = requestid;
                log.AdminId = adminid;
                log.PhysicianId = physicianId;
                log.SentDate = DateTime.Now;
                log.SentTries = 1;
                log.CreateDate = createdate;
                log.IsEmailSent = new BitArray(new bool[] { true });
                _emailLogsRepository.AddAsync(log);

                var abc = "Success";
                return abc;
            }
            catch (Exception ex)
            {
                var abc = "Success";
                return ex.Message.ToString();
            }
        }
        #endregion

        #region View Uploads / View Documents

        public async Task<DashboardViewModel> ViewDocument(int Id)
        {
            DashboardViewModel viewModel = new DashboardViewModel();
            Request req = await _requestRepository.GetByIdAsync(Id);
            RequestClient requestClient = await _requestclientRepository.CheckUserByClientID(req.RequestId);
            viewModel.Username = requestClient.FirstName + " " + requestClient.LastName;
            viewModel.RequstId = Id;
            viewModel.Email = requestClient.Email;
            viewModel.RequestWiseFiles = (
                                         from rwf in _requestwisefileRepository.GetAll()
                                         where rwf.RequestId == Id && rwf.IsDeleted == null
                                         select new RequestWiseFile
                                         {
                                             CreatedDate = rwf.CreatedDate,
                                             FileName = rwf.FileName,
                                             RequestWiseFileId = rwf.RequestWiseFileId,
                                             RequestId = rwf.RequestId,
                                             AdminId = rwf.AdminId,
                                             Admin = rwf.Admin,
                                             Request = rwf.Request,
                                         }).ToList();
            return viewModel;
        }
        public async Task<string> ViewDocument(IFormFile a, int Id, int adminid, int userid)
        {
            if (a != null && a.Length > 0)
            {
                var fileName = Path.GetFileName(a.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    a.CopyTo(fileStream);
                }
                var newRequestWiseFile = new RequestWiseFile
                {
                    RequestId = Id,
                    FileName = fileName,
                    CreatedDate = DateTime.Now,
                    AdminId = adminid == 0 ? null : adminid,

                };
                await _requestwisefileRepository.AddAsync(newRequestWiseFile);
            }
            return "";
        }

        public async Task<RequestWiseFile> DownloadFile(int fileId)
        {
            RequestWiseFile reqw = await _requestwisefileRepository.GetByIdAsync(fileId);

            return reqw;
        }

        public async Task<byte[]> DownloadAllByChecked(IEnumerable<int> documentValues)
        {
            //selectedFiles = selectedFiles.Distinct();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in documentValues)
                    {
                        var temp = await _requestwisefileRepository.GetByIdAsync(file);
                        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/uploads/", temp.FileName);
                        byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);

                        var zipEntry = zipArchive.CreateEntry(temp.FileName);
                        using (var zipEntryStream = zipEntry.Open())
                        {
                            zipEntryStream.Write(fileBytes, 0, fileBytes.Length);
                        }
                    }
                }
                memoryStream.Seek(0, SeekOrigin.Begin);

                return memoryStream.ToArray();
            }
        }
        public async Task<byte[]> DownloadAll(IEnumerable<int> documentValues, int? requestid)
        {
            var filesRow = _requestwisefileRepository.GetAll().Where(x => x.RequestId == requestid && x.IsDeleted == null).ToList();
            MemoryStream ms = new MemoryStream();
            using (ZipArchive zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
                filesRow.ForEach(file =>
                {
                    var path = "wwwroot\\uploads\\" + Path.GetFileName(file.FileName);
                    ZipArchiveEntry zipEntry = zip.CreateEntry(file.FileName);
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    using (Stream zipEntryStream = zipEntry.Open())
                    {
                        fs.CopyTo(zipEntryStream);
                    }
                });
            ms.Seek(0, SeekOrigin.Begin);

            return ms.ToArray();
        }
        #endregion

        #region PatientForgotPassword
        public async Task<string> PatientForgotPassword(CreateAccountViewModel createAccountViewModel)
        {
            AspNetUser myUser = _aspnetuserRepository.GetAll().Where(x => x.Email == createAccountViewModel.Email).FirstOrDefault();

            myUser.PasswordHash = _aspnetuserRepository.EncodePasswordToBase64(createAccountViewModel.ConfirmPassword);
            await _aspnetuserRepository.UpdateAsync(myUser);
            _aspnetuserRepository.SetTempData("Message", "Password Updated");
            return "RegisterdPatientLogin";

        }
        #endregion

        #region Admin Forgot Password
        public async Task<string> AdminResetPassword(CreateAccountViewModel createAccountViewModel)
        {
            AspNetUser myUser = await _aspnetuserRepository.CheckUserByEmail(createAccountViewModel.Email);

            if (myUser != null)
            {
                if (createAccountViewModel.PasswordHash == createAccountViewModel.ConfirmPassword)
                {
                    myUser.PasswordHash = _aspnetuserRepository.EncodePasswordToBase64(createAccountViewModel.ConfirmPassword);
                    await _aspnetuserRepository.UpdateAsync(myUser);
                    _aspnetuserRepository.SetTempData("Message", "Password Updated");
                    return "RegisterdPatientLogin";
                }
                else
                {
                    _aspnetuserRepository.SetTempData("Message", "Password and Confirm Password must be same!!!");

                }
            }
            else
            {
                _aspnetuserRepository.SetTempData("Message", "Invalid User Name");
            }
            return "PatientForgotPassword";

        }
        #endregion

        #region dashboard
        public async Task<int> GetCount(int statusId)
        {
            return await _requestRepository.GetCountAsync(r => r.Status == statusId && r.UserId != null && r.IsDeleted == null);
        }

        #endregion

        #region Send Agreement
        public async Task<RequestClient> GetRequestClientByID(int id)
        {
            RequestClient result = await _requestclientRepository.GetByIdAsync(id);
            return result;
        }

        public async Task<object> sendAgreement(int id)
        {
            ViewCaseViewModel viewModel = new ViewCaseViewModel();
            RequestClient req = await _requestclientRepository.GetByIdAsync(id);
            viewModel.Email = req.Email;
            viewModel.PhoneNumber = req.PhoneNumber;
            viewModel.requestclientID = id;
            return viewModel;
        }

        public async Task<object> AcceptAgreement(int id)
        {
            RequestClient req = await _requestclientRepository.GetByIdAsync(id);
            Request request = await _requestRepository.GetByIdAsync(req.RequestId);
            request.Status = 4;

            await _requestRepository.UpdateAsync(request);

            RequestStatusLog log = new RequestStatusLog
            {
                RequestId = req.RequestId,
                Status = 4,
                Notes = "Accepted By Patient",
                CreatedDate = DateTime.Now

            };
            await _requestStatusLogRepository.AddAsync(log);

            Encounter encounter = new Encounter()
            {
                Requestid = req.RequestId,
                Firstname = req.FirstName,
                Lastname = req.LastName,
                Phonenumber = req.PhoneNumber,
                Location = req.Address,
                Dateofbirth= new DateTime((int)req.IntYear, Convert.ToInt32(req.StrMonth), (int)req.IntDate),
                Email = req.Email,
                Physicianid=request.PhysicianId,
                
            };
            await _encounterRepository.AddAsync(encounter);
            return "";
        }

        public async Task<object> ConfirmCancelAgreement(int id, string note)
        {
            RequestClient req = await _requestclientRepository.GetByIdAsync(id);
            Request request = await _requestRepository.GetByIdAsync(req.RequestId);
            request.Status = 7;
            await _requestRepository.UpdateAsync(request);

            RequestStatusLog log = new RequestStatusLog
            {
                RequestId = req.RequestId,
                Status = 7,
                Notes = "Declined By Patient:" + note,
            };
            await _requestStatusLogRepository.AddAsync(log);
            RequestClosed requestClosed = new RequestClosed
            {
                RequestId = req.RequestId,
                RequestStatusLogId = log.RequestStatusLogId,
                ClientNotes = note
            };
            await _requestClosedRepository.AddAsync(requestClosed);

            return "";
        }
        #endregion

        #region Send Order
        public async Task<List<HealthProfessional>> GetBusinessByProfession(int professionId)
        {
            return await _healthProfessionalsRepository.GetBusinessByProfession(professionId);
        }
        public async Task<HealthProfessional> GetBusinessDetails(object BusinessId)
        {
            HealthProfessional business = await _healthProfessionalsRepository.GetByIdAsync(BusinessId);
            return business;
        }
        public async Task<object> SendOrder(int Id)
        {
            SendOrderViewModel viewModel = new SendOrderViewModel();
            viewModel.RequestID = Id;
            viewModel.Profession = await _healthProfessionalTypeRepository.GetProfession();
            viewModel.Business = await _healthProfessionalsRepository.GetVendor();
            return viewModel;
        }
        public async Task<string> SendOrderDetails(SendOrderViewModel viewModel, int id, string createdById)
        {
            OrderDetail order = new OrderDetail();
            order.VendorId = viewModel.BusinessID;
            order.FaxNumber = viewModel.FaxNumber;
            order.RequestId = id;
            order.Email = viewModel.Email;
            order.BusinessContact = viewModel.Contact;
            order.Prescription = viewModel.OrderDetails;
            order.NoOfRefill = viewModel.Refill;
            order.CreatedDate = DateTime.Now;
            order.CreatedBy = createdById;
            await _orderDetailsRepository.AddAsync(order);
            return "";
        }
        #endregion
    }
}
