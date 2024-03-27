using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using HalloDoc.Repository.IRepository;
using HalloDoc.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Ocsp;
using static HalloDoc.Entities.ViewModels.AdminDashboardViewModel;
using static HalloDoc.Entities.ViewModels.ViewNotesViewModel;
using System.Drawing.Printing;


namespace HalloDoc.Services.Services
{
    public class AdminService : IAdminService
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
        private readonly IRoleRepository _roleRepository;
        private readonly IPhysicianNotificationRepository _physicianNotificationRepository;
        private readonly IStatusRepository _statusRepository;

        public AdminService(IAspNetUserRepository aspnetuserRepository, IUserRepository userRepository,
                               IRequestRepository requestRepository, IRequestClientRepository requestclientRepository,
                               IRequestWiseFilesRepository requestwisefileRepository, IBusinessRepository businessRepository,
                               IConciergeRepository conciergeRepository,
                               IPhysicianRepository physicianRepository, IAdminRepository adminRepository, IRequestNotesRepository requestNotesRepository,
                               ICaseTagRepository caseTagRepository, IRequestStatusLogRepository requestStatusLogRepository, IRegionRepository regionRepository,
                               IOrderDetailsRepository orderDetailsRepository, IHealthProfessionalsRepository healthProfessionalsRepository,
                               IHealthProfessionalTypeRepository healthProfessionalTypeRepository, IBlockRequestRepository blockRequestRepository,
                               IAdminRegionRepository adminRegionRepository,
                               IEncounterRepository encounterRepository, IRequestClosedRepository requestClosedRepository, IRoleRepository roleRepository,
                               IPhysicianNotificationRepository physicianNotificationRepository, IStatusRepository statusRepository)
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
            _roleRepository = roleRepository;
            _physicianNotificationRepository = physicianNotificationRepository;
            _statusRepository = statusRepository;
        }
        #endregion

        #region common methods
        public async Task<AspNetUser> checkEmailPassword(string email, string password)
        {

            return await _aspnetuserRepository.Login(email, password);
        }
        public async Task<User> GetUser(string email)
        {
            User user = await _userRepository.CheckUserByEmail(email);
            return user;
        }
        public async Task<Admin> GetAdmin(string email)
        {
            Admin user = await _adminRepository.CheckUserByEmail(email);
            return user;
        }
        public async Task<int> GetCount(int statusId)
        {
            return await _requestRepository.GetCountAsync(r => r.Status == statusId && r.UserId != null);
        }
        public async Task<List<RequestWiseFile>> GetFilesSelectedByFileID(List<int> selectedFilesIds)
        {
            return await _requestwisefileRepository.GetFilesSelectedByFileID(selectedFilesIds);
        }
        //public async Task<RequestWiseFile> FindFile(string fileName)
        //{
        //    RequestWiseFile file = await _requestwisefileRepository.FindFile(fileName);
        //    return file;
        //}
        //public async Task<RequestWiseFile> FindFile(string fileName, int? ID)
        //{
        //    RequestWiseFile file = await _requestwisefileRepository.FindFileByID(fileName, ID);
        //    return file;
        //}
        public async Task<T> GetTempData<T>(string key)
        {
            return await Task.FromResult(_aspnetuserRepository.GetTempData<T>(key));
        }
        public async Task<RequestClient> GetRequestClientByID(int id)
        {
            RequestClient result = await _requestclientRepository.GetByIdAsync(id);
            return result;
        }

        #endregion


        #region Admin
        #region Send Mail
        public string SendEmail(string email, string link, string subject, string body, List<string> attachmentFilePath = null)
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

        #region Dashboard
        public List<AdminDashboardViewModel> Admintbl(string state, List<AdminDashboardViewModel> list, int status)
        {

            var tabledashboard = (
                from r in _requestRepository.GetAll()
                join rec in _requestclientRepository.GetAll() on r.RequestId equals rec.RequestId
                join phy in _physicianRepository.GetAll() on r.PhysicianId equals phy.PhysicianId into physicianGroup
                from phy in physicianGroup.DefaultIfEmpty()

                where r.Status == status

                select new AdminDashboardViewModel
                {
                    PatientName = rec.FirstName + "," + rec.LastName,
                    DateOfBirth = new DateTime((int)rec.IntYear, Convert.ToInt32(rec.StrMonth), (int)rec.IntDate),
                    Requestor = r.FirstName + "," + r.LastName,
                    RequestedDate = r.CreatedDate,
                    PatientPhone = rec.PhoneNumber,
                    RequestorPhone = r.PhoneNumber,
                    Address = rec.Street + "," + rec.City + "," + rec.State + "," + rec.ZipCode,
                    Notes = rec.Notes,
                    PhysicianName = phy.FirstName + " " + phy.LastName,
                    RequestTypeID = r.RequestTypeId,
                    RequstClientId = rec.RequestClientId,
                    requestID = rec.RequestId,
                    Email = rec.Email,
                    RegionId = (int)rec.RegionId,
                    StateofTable = rec.State,
                    stateTab = state,

                }).ToList();

            return tabledashboard.OrderByDescending(x => x.RequestedDate).ToList();
        }

        public AdminDashboardViewModel Pagination(string state, int CurrentPage, string? PatientName, int? ReqType, int? RegionId, List<AdminDashboardViewModel> newState)
        {
            if (!string.IsNullOrWhiteSpace(PatientName))
            {
                newState = newState.Where(a => a.PatientName.ToLower().Contains(PatientName.ToLower())).ToList();
            }
            if (ReqType != 0 && ReqType != null)
            {
                newState = newState.Where(a => a.RequestTypeID == ReqType).ToList();
            }
            if (RegionId != 0 && RegionId != null)
            {
                newState = newState.Where(a => a.RegionId == RegionId).ToList();
            }
            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = newState.Count;
            int totalPage = (int)Math.Ceiling((double)totalCount / dataSize);
            int FirstItemIndex = Math.Min((CurrentPage - 1) * dataSize + 1, totalCount);
            int LastItemIndex = Math.Min(CurrentPage * dataSize, totalCount);
            List<AdminDashboardViewModel> clients = newState
                .OrderByDescending(u => u.CreatedDate)
                .Skip((CurrentPage - 1) * dataSize)
                .Take(dataSize)
                .ToList();

            return new AdminDashboardViewModel
            {
                stateTab = state,
                PagingData = clients,
                TotalCount = totalCount,
                TotalPages = totalPage,
                CurrentPage = CurrentPage,
                PageSize = 3,
                FirstItemIndex = FirstItemIndex,
                LastItemIndex = LastItemIndex,
            };
        }

        public async Task<object> DashboardRegions(AdminDashboardViewModel viewModel)
        {
            viewModel.State = await _regionRepository.GetRegions();
            return viewModel;
        }
        #endregion

        #region View Case
        public async Task<int> GetUserByRequestClientID(int id)
        {
            int user = await _requestRepository.CheckUserByID(id);
            return user;
        }

        public async Task<object> ViewCase(int userId)
        {

            ViewCaseViewModel viewmodel = new ViewCaseViewModel();
            RequestClient user = await _requestclientRepository.GetByIdAsync(userId);
            Request req = await _requestRepository.GetByIdAsync(user.RequestId);
            DateTime dob = new DateTime((int)user.IntYear, Convert.ToInt32(user.StrMonth), (int)user.IntDate);

            if (user != null)
            {
                viewmodel.Symptoms = user.Notes;
                viewmodel.LastName = user.LastName;
                viewmodel.FirstName = user.FirstName;
                viewmodel.State = user.State;
                viewmodel.Email = user.Email;
                viewmodel.PhoneNumber = user.PhoneNumber;
                viewmodel.Address = user.Street + "," + user.City + "," + user.ZipCode;
                viewmodel.requestclientID = user.RequestClientId;
                viewmodel.DateOfBirth = dob;
                viewmodel.status = req.Status;
                viewmodel.street = user.Street;
                viewmodel.city = user.City;
                return viewmodel;
            }
            return "ViewCase";
        }
        public async Task<string> EditNewRequest(ViewCaseViewModel viewModel, int? userId)
        {
            RequestClient user = await _requestclientRepository.GetByIdAsync(userId);
            if (user != null)
            {

                user.Email = viewModel.Email;

                user.PhoneNumber = viewModel.PhoneNumber;

                await _requestclientRepository.UpdateAsync(user);

            }
            return "ViewCase";
        }

        #endregion

        #region View Notes

        public async Task<ViewNotesViewModel> ViewNotes(int id)
        {
            ViewNotesViewModel viewmodel = new ViewNotesViewModel();
            RequestClient req = await _requestclientRepository.GetByIdAsync(id);
            viewmodel.requestID = req.RequestId;

            RequestNote requestNote = await _requestNotesRepository.CheckByRequestID(req.RequestId);
            if (requestNote != null)
            {
                viewmodel.AdminNotes = requestNote.AdminNotes;
            }

            viewmodel.TransferNotes = (from r in _requestRepository.GetAll()
                                       join rsl in _requestStatusLogRepository.GetAll() on r.RequestId equals rsl.RequestId
                                       join p in _physicianRepository.GetAll() on rsl.TransToPhysicianId equals p.PhysicianId into g
                                       from p in g.DefaultIfEmpty()
                                       where r.RequestId == req.RequestId
                                       orderby rsl.CreatedDate descending
                                       select new TransferNotesViewModel
                                       {
                                           RequestId = r.RequestId,
                                           FirstName = r.FirstName,
                                           LastName = r.LastName,
                                           CreatedDate = rsl.CreatedDate,
                                           Note = rsl.Notes,
                                           PhysicianName = p != null ? p.FirstName + " " + p.LastName : null
                                       }).ToList();

            return viewmodel;
        }

        public async Task<object> AddNotes(string? additionalNotes, string? adminNotes, int id)
        {


            RequestNote requestNote = await _requestNotesRepository.CheckByRequestID(id);

            if (requestNote != null)
            {
                requestNote.AdminNotes = additionalNotes;
                await _requestNotesRepository.UpdateAsync(requestNote);
                adminNotes = requestNote.AdminNotes;

            }
            else
            {
                RequestNote requestNote1 = new RequestNote();
                requestNote1.RequestId = id;
                requestNote1.AdminNotes = additionalNotes;
                requestNote1.CreatedDate = DateTime.Now;
                requestNote1.CreatedBy = "bda27f31-02b1-442f-9120-bed8f09a4966";
                await _requestNotesRepository.AddAsync(requestNote1);
                adminNotes = requestNote1.AdminNotes;


            }

            return "ViewNotes";
        }
        #endregion

        #region cancel case
        public async Task<object> CancelCase(CancelCaseViewModel viewModel, int id)
        {

            RequestClient patient = await _requestclientRepository.CheckUserByID(id);

            viewModel.PatientName = patient.FirstName + " " + patient.LastName;
            viewModel.RequestClientID = id;
            viewModel.Case = await _caseTagRepository.GetCases();

            return viewModel;

        }
        public async Task<string> ConfirmCancelCase(CancelCaseViewModel viewModel, int id)
        {
            RequestClient req = await _requestclientRepository.GetByIdAsync(id);

            RequestStatusLog requestNote1 = await _requestStatusLogRepository.CheckByRequestID(req.RequestId);


            if (requestNote1 != null)
            {
                requestNote1.Status = 3;
                requestNote1.AdminId = 1;
                requestNote1.Notes = viewModel.AdditionalNotes;
                await _requestStatusLogRepository.UpdateAsync(requestNote1);


            }
            else
            {
                RequestStatusLog requesStatusLog = new RequestStatusLog();
                requesStatusLog.Status = 3;
                requesStatusLog.AdminId = 1;
                requesStatusLog.RequestId = req.RequestId;
                requesStatusLog.Notes = viewModel.AdditionalNotes;
                requesStatusLog.CreatedDate = DateTime.Now;

                await _requestStatusLogRepository.AddAsync(requesStatusLog);
            }


            Request request = await _requestRepository.GetByIdAsync(req.RequestId);

            request.Status = 3;
            request.CaseTag = viewModel.CaseTagID.ToString();
            await _requestRepository.UpdateAsync(request);

            return "Dashboard";

        }
        #endregion

        #region Assign Case
        public async Task<object> AssignCase(AssignCaseViewModel viewModel, int id)
        {
            RequestClient patient = await _requestclientRepository.CheckUserByID(id);
            viewModel.RequestClientID = id;
            viewModel.Region = await _regionRepository.GetRegions();
            viewModel.Physician = await _physicianRepository.GetPhysician();
            try
            {
                return viewModel;
            }
            catch (Exception ex)
            {

            }
            return viewModel;
        }
        public async Task<string> AssignRequest(AssignCaseViewModel viewModel, int id)
        {
            RequestClient req = await _requestclientRepository.GetByIdAsync(id);
            RequestStatusLog requestNote1 = await _requestStatusLogRepository.CheckByRequestID(req.RequestId);
            if (requestNote1 != null)
            {
                requestNote1.Status = 2;
                requestNote1.AdminId = 1;
                requestNote1.Notes = viewModel.AdditionalNotes;
                await _requestStatusLogRepository.UpdateAsync(requestNote1);
            }

            RequestStatusLog requesStatusLog = new RequestStatusLog();
            requesStatusLog.Status = 2;
            requesStatusLog.AdminId = 1;
            requesStatusLog.RequestId = req.RequestId;
            requesStatusLog.Notes = viewModel.AdditionalNotes;
            requesStatusLog.CreatedDate = DateTime.Now;
            requesStatusLog.TransToPhysicianId = viewModel.physicianID;
            await _requestStatusLogRepository.AddAsync(requesStatusLog);

            Request request = await _requestRepository.GetByIdAsync(req.RequestId);

            request.Status = 2;
            request.PhysicianId = viewModel.physicianID;
            //   request.CaseTag = viewModel.CaseTagID;
            await _requestRepository.UpdateAsync(request);

            return "Dashboard";

        }
        public async Task<List<Physician>> GetPhysiciansByRegion(int regionId)
        {
            return await _physicianRepository.GetPhysiciansByRegion(regionId);
        }
        #endregion

        #region Block case
        public async Task<object> BlockCase(CancelCaseViewModel viewModel, int id)
        {

            RequestClient patient = await _requestclientRepository.CheckUserByID(id);
            viewModel.PatientName = patient.FirstName + " " + patient.LastName;
            viewModel.RequestClientID = id;
            return viewModel;

        }
        public async Task<string> BlockCaseRequest(CancelCaseViewModel viewModel, int id)
        {
            RequestClient req = await _requestclientRepository.GetByIdAsync(id);
            RequestStatusLog requestNote1 = await _requestStatusLogRepository.CheckByRequestID(req.RequestId);
            Request requestor = await _requestRepository.GetByIdAsync(req.RequestId);
            if (requestNote1 != null)
            {
                requestNote1.Status = 11;
                requestNote1.AdminId = 1;
                requestNote1.Notes = viewModel.AdditionalNotes;
                await _requestStatusLogRepository.UpdateAsync(requestNote1);

            }
            else
            {
                RequestStatusLog requesStatusLog = new RequestStatusLog();
                requesStatusLog.Status = 11;
                requesStatusLog.AdminId = 1;
                requesStatusLog.RequestId = req.RequestId;
                requesStatusLog.Notes = viewModel.AdditionalNotes;
                requesStatusLog.CreatedDate = DateTime.Now;
                await _requestStatusLogRepository.AddAsync(requesStatusLog);
            }

            Request request = await _requestRepository.GetByIdAsync(req.RequestId);
            request.Status = 11;
            //request.CaseTag = viewModel.CaseTagID;
            await _requestRepository.UpdateAsync(request);

            BlockRequest blockRequest = new BlockRequest();
            blockRequest.PhoneNumber = requestor.PhoneNumber;
            blockRequest.CreatedDate = DateTime.Now;
            blockRequest.Email = requestor.Email;
            blockRequest.Reason = viewModel.AdditionalNotes;
            blockRequest.RequestId = Convert.ToString(req.RequestId);
            await _blockRequestRepository.AddAsync(blockRequest);

            return "Dashboard";

        }
        #endregion

        #region Transfer Case
        public async Task<object> TransferCase(AssignCaseViewModel viewModel, int id)
        {
            RequestClient patient = await _requestclientRepository.CheckUserByID(id);
            viewModel.RequestClientID = id;
            viewModel.Region = await _regionRepository.GetRegions();
            viewModel.Physician = await _physicianRepository.GetPhysician();
            return viewModel;
        }
        public async Task<string> TransferRequest(AssignCaseViewModel viewModel, int id)
        {
            RequestClient req = await _requestclientRepository.GetByIdAsync(id);

            RequestStatusLog requesStatusLog = new RequestStatusLog
            {
                Status = 2,
                AdminId = 1,
                RequestId = req.RequestId,
                Notes = viewModel.AdditionalNotes,
                TransToPhysicianId = viewModel.physicianID,
                CreatedDate = DateTime.Now
            };
            await _requestStatusLogRepository.AddAsync(requesStatusLog);


            Request request = await _requestRepository.GetByIdAsync(req.RequestId);
            request.PhysicianId = viewModel.physicianID;
            await _requestRepository.UpdateAsync(request);

            return "Dashboard";

        }
        #endregion

        #region ClearCase
        public async Task<string> ClearRequest(int? id)
        {

            Request request = await _requestRepository.GetByIdAsync(id);
            request.Status = 10;
            await _requestRepository.UpdateAsync(request);

            return "Dashboard";

        }
        #endregion

        #region CLose Case
        public async Task<CloseCaseViewModel> CloseCase(int Id)
        {

            Request req = await _requestRepository.GetByIdAsync(Id);
            RequestClient requestClient = await _requestclientRepository.CheckUserByClientID(req.RequestId);
            DateTime dob = new DateTime((int)requestClient.IntYear, Convert.ToInt32(requestClient.StrMonth), (int)requestClient.IntDate);
            CloseCaseViewModel viewModel = new CloseCaseViewModel
            {
                FirstName = requestClient.FirstName,
                LastName = requestClient.LastName,
                PhoneNumber = requestClient.PhoneNumber,
                Email = requestClient.Email,
                DateOfBirth = dob,
                RequstId = Id,
                Username = requestClient.FirstName + " " + requestClient.LastName,
                RequestClientID = requestClient.RequestClientId,
                RequestWiseFiles = (
                                         from rwf in _requestwisefileRepository.GetAll()
                                         where rwf.RequestId == Id && rwf.IsDeleted == null
                                         select new RequestWiseFile
                                         {
                                             CreatedDate = rwf.CreatedDate,
                                             FileName = rwf.FileName,
                                             RequestWiseFileId = rwf.RequestWiseFileId

                                         }).ToList()
            };
            return viewModel;

        }
        public async Task<object> EditClose(CloseCaseViewModel viewModel, int Id)
        {


            RequestClient user = await _requestclientRepository.GetByIdAsync(Id);
            if (user != null)
            {

                user.Email = viewModel.Email;

                user.PhoneNumber = viewModel.PhoneNumber;

                await _requestclientRepository.UpdateAsync(user);

            }
            return user.RequestId;
        }
        public async Task<string> ConfirmCloseCase(int id)
        {
            Request req = await _requestRepository.GetByIdAsync(id);
            req.Status = 9;
            await _requestRepository.UpdateAsync(req);
            RequestStatusLog log = new RequestStatusLog()
            {
                RequestId = req.RequestId,
                Status = req.Status,
                AdminId = 1,
                CreatedDate = DateTime.Now,
            };
            await _requestStatusLogRepository.AddAsync(log);
            RequestClosed reqClosed = new RequestClosed()
            {
                RequestId = req.RequestId,
                RequestStatusLogId = log.RequestStatusLogId

            };
            await _requestClosedRepository.AddAsync(reqClosed);

            return "Dashboard";
        }


        #endregion

        #region View Uploads

        public bool IsDeleted(BitArray? isDeleted)
        {
            if (isDeleted == null)
            {
                return false;
            }

            for (int i = 0; i < isDeleted.Length; i++)
            {
                if (isDeleted[i])
                {
                    return true;
                }
            }

            return false;
        }
        public async Task<object> DeleteFile(int fileID, int? reqID)
        {
            if (fileID == 0)
            {
                var filesRow = await _requestwisefileRepository.FindFileByRequestID(reqID).ToListAsync();
                foreach (var u in filesRow)
                {
                    if (IsDeleted(u.IsDeleted))
                    {
                        return new { Success = false, Message = "File is already deleted" };
                    }

                    u.IsDeleted = new BitArray(new bool[] { true });
                    await _requestwisefileRepository.UpdateAsync(u);
                }

            }
            else
            {
                RequestWiseFile file = await _requestwisefileRepository.GetByIdAsync(fileID);
                if (IsDeleted(file.IsDeleted))
                {
                    return new { Success = false, Message = "File is already deleted" };
                }

                file.IsDeleted = new BitArray(new bool[] { true });
                await _requestwisefileRepository.UpdateAsync(file);
            }

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
        public async Task<string> SendOrderDetails(SendOrderViewModel viewModel, int id, string adminID)
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
            order.CreatedBy = adminID;
            await _orderDetailsRepository.AddAsync(order);
            return "";
        }
        #endregion

        #region Send Agreement
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
                Location = req.State,
                Email = req.Email,
            };
            await _encounterRepository.AddAsync(encounter);
            return "";
        }

        public async Task<object> ConfirmCancelAgreement(int id, string note)
        {
            RequestClient req = await _requestclientRepository.GetByIdAsync(id);
            Request request = await _requestRepository.GetByIdAsync(req.RequestId);
            request.Status = 3;
            await _requestRepository.UpdateAsync(request);

            RequestStatusLog log = new RequestStatusLog
            {
                RequestId = req.RequestId,
                Status = 3,
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

        #region Admin My Profile
        public async Task<object> AdminMyProfile(int? adminId)
        {
            Admin admin = await _adminRepository.GetByIdAsync(adminId);
            AspNetUser aspNetUser = await _aspnetuserRepository.GetByIdAsync(admin.AspNetUserId);
            var adminRegion = await _adminRegionRepository.adminRegionByAdminID(admin.AdminId);
            AdminMyProfileViewModel model = new AdminMyProfileViewModel()
            {
                AdminID = admin.AdminId,
                UserName = aspNetUser.UserName,
                Password = _aspnetuserRepository.DecodeFrom64(aspNetUser.PasswordHash),
                Status = (int)admin.Status,
                Role = "Admin",
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                ConfirmEmail = admin.Email,
                PhoneNumber = admin.Mobile,
                Address1 = admin.Address1,
                Address2 = admin.Address2,
                City = admin.City,
                RegionId = admin.RegionId,
                State = await _regionRepository.GetRegions(),
                Zip = admin.Zip,
                BillingPhoneNumber = admin.AltPhone,
                AdminRegions = adminRegion

            };
            return model;


        }
        public async Task<object> ResetPasswordAdmin(AdminMyProfileViewModel model)
        {
            Admin admin = await _adminRepository.GetByIdAsync(model.AdminID);
            AspNetUser user = await _aspnetuserRepository.GetByIdAsync(admin.AspNetUserId);
            user.PasswordHash = _aspnetuserRepository.EncodePasswordToBase64(model.Password);
            user.ModifiedDate = DateTime.Now;
            await _aspnetuserRepository.UpdateAsync(user);
            return user;
        }

        public async Task<object> SaveAdminInfo(AdminMyProfileViewModel model)
        {
            Admin admin = await _adminRepository.GetByIdAsync(model.AdminID);
            AspNetUser user = await _aspnetuserRepository.GetByIdAsync(admin.AspNetUserId);

            admin.FirstName = model.FirstName;
            admin.LastName = model.LastName;
            admin.Email = model.Email;
            admin.Mobile = model.PhoneNumber;
            admin.ModifiedDate = DateTime.Now;
            await _adminRepository.UpdateAsync(admin);

            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.ModifiedDate = DateTime.Now;
            await _aspnetuserRepository.UpdateAsync(user);

            return user;
        }
        public async Task<object> SaveBillingInfo(AdminMyProfileViewModel model)
        {
            Admin admin = await _adminRepository.GetByIdAsync(model.AdminID);
            AspNetUser user = await _aspnetuserRepository.GetByIdAsync(admin.AspNetUserId);
            admin.Address1 = model.Address1;
            admin.Address2 = model.Address2;
            admin.Zip = model.Zip;
            admin.AltPhone = model.BillingPhoneNumber;
            admin.City = model.City;
            admin.RegionId = model.RegionId;

            await _adminRepository.UpdateAsync(admin);
            return user;
        }
        #endregion

        #region Encounter Form
        public async Task<object> EncounterForm(int RequestId)
        {
            Request req = await _requestRepository.GetByIdAsync(RequestId);
            RequestClient client = await _requestclientRepository.CheckUserByClientID(RequestId);
            Encounter encounter = await _encounterRepository.checkBYRequestID(req.RequestId);
            DateTime dob = new DateTime((int)client.IntYear, Convert.ToInt32(client.StrMonth), (int)client.IntDate);


            EncounterViewModel model = new EncounterViewModel()
            {
                RequestID = req.RequestId,
                FirstName = encounter.Firstname,
                LastName = encounter.Lastname,
                Location = encounter.Location,
                DateOfBirth = dob,
                PhoneNumber = encounter.Phonenumber,
                MedicalReport = encounter.Medicalreport,
                Date = encounter.Date,
                Email = encounter.Email,
                HistoryOfPresentIllness = encounter.Historyofpresentillness,
                MedicalHistory = encounter.Medicalhistory,
                Medications = encounter.Medications,
                Temp = encounter.Temp,
                BloodPressureSystolic = encounter.Bloodpressuresystolic,
                BloodPressureDiastolic = encounter.Bloodpressurediastolic,
                Heent = encounter.Heent,
                Chest = encounter.Chest,
                Hr = encounter.Hr,
                Allergies = encounter.Allergies,
                Cv = encounter.Cv,

                ABD = encounter.Abd,
                Extr = encounter.Extr,
                Skin = encounter.Skin,
                Neuro = encounter.Neuro,
                Diagnosis = encounter.Diagnosis,
                MedicationsDispensed = encounter.Medicationsdispensed,
                Followup = encounter.Followup,
                Other = encounter.Other,
                TreatmentPlan = encounter.Treatmentplan,
                Procedures = encounter.Procedures,
                Rr = encounter.Rr,
                Pain = encounter.Pain,
                IsChanged = encounter.Ischanged,
                IsFinalized = encounter.Isfinalized

            };
            return model;
        }

        public async Task<object> EncounterFormSaveChanges(EncounterViewModel model)
        {
            Encounter encounter = await _encounterRepository.checkBYRequestID(model.RequestID);
            encounter.Firstname = model.FirstName;
            encounter.Lastname = model.LastName;
            encounter.Location = model.Location;
            //encounter.Dateofbirth = model.DateOfBirth;
            encounter.Phonenumber = model.PhoneNumber;
            encounter.Medicalreport = model.MedicalReport;
            //   encounter.Date = model.Date.Value;
            encounter.Email = model.Email;
            encounter.Historyofpresentillness = model.HistoryOfPresentIllness;
            encounter.Medicalhistory = model.MedicalHistory;
            encounter.Medications = model.Medications;
            //encounter.Temp = model.Temp.Value;
            // encounter.Bloodpressuresystolic = model.BloodPressureSystolic.Value;
            // encounter.Bloodpressurediastolic = model.BloodPressureDiastolic.Value;
            encounter.Heent = model.Heent;
            encounter.Chest = model.Chest;
            //encounter.Hr = model.Hr.Value;
            encounter.Allergies = model.Allergies;
            encounter.Cv = model.Cv;
            encounter.Abd = model.ABD;
            encounter.Extr = model.Extr;
            encounter.Skin = model.Skin;
            encounter.Neuro = model.Neuro;
            encounter.Diagnosis = model.Diagnosis;
            encounter.Medicationsdispensed = model.MedicationsDispensed;
            encounter.Followup = model.Followup;
            encounter.Other = model.Other;
            encounter.Treatmentplan = model.TreatmentPlan;
            encounter.Procedures = model.Procedures;
            //encounter.Rr = model.Rr.Value;
            //encounter.Pain = model.Pain.Value;
            //encounter.Ischanged = model.IsChanged.Value;
            //encounter.Isfinalized = model.IsFinalized.Value;
            await _encounterRepository.UpdateAsync(encounter);

            return encounter;
        }
        #endregion
        #endregion

        #region Provider

        #region Create  Provider
      
        public async Task<object> Createprovider()
        {
            ProviderViewModel model = new ProviderViewModel();
            model.State = await _regionRepository.GetRegions();
            model.Role = await _roleRepository.GetRoles();
            return model;
        }
        public async Task<object> CreateProvider(ProviderViewModel model)
        {
            AspNetUser user = new AspNetUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                PasswordHash = model.Password,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CreatedDate = DateTime.Now,
            };
            await _aspnetuserRepository.AddAsync(user);

            Physician physician = new Physician();

            physician.Id = user.Id;
            physician.FirstName = model.FirstName;
            physician.LastName = model.LastName;
            physician.Email = model.Email;
            physician.MedicalLicense = model.MedicalLicense;
            physician.Mobile = model.PhoneNumber;
            physician.AdminNotes = model.AdminNotes;
            physician.Address1 = model.Address1;
            physician.Address2 = model.Address2;
            physician.Zip = model.Zip;
            physician.City = model.City;
            physician.RegionId = model.RegionId;
            physician.RoleId = model.RoleId;
            physician.CreatedDate = DateTime.Now;
            physician.Status = 3;
            physician.CreatedBy = user.Id;
            physician.BusinessName = model.BusinessName;
            physician.BusinessWebsite = model.BusinessWebsite;
            //physician.Photo=model.File.FileName;


            await _physicianRepository.AddAsync(physician);
            return "ViewUploads";
        }

        #endregion

        #region edit provider
        public async Task<object> EditProvider( int physicianID)
        {
            Physician phy =await _physicianRepository.GetByIdAsync(physicianID);
            AspNetUser asp =await _aspnetuserRepository.getById(phy.Id);
            ProviderViewModel model = new ProviderViewModel();
            model.FirstName= phy.FirstName;
            model.LastName = phy.LastName;
            model.PhoneNumber = phy.Mobile;
            model.AdminNotes = phy.AdminNotes;
            model.Email=phy.Email;
            model.MedicalLicense= phy.MedicalLicense;
            model.NPINumber = phy.Npinumber;
            model.Address1 = phy.Address1;
            model.Address2 = phy.Address2;
            model.City = phy.City;
            model.RegionId= phy.RegionId;
            model.Regions =await  _regionRepository.GetRegions();
            model.Zip = phy.Zip;
            model.BillingPhoneNumber = phy.AltPhone;
            model.BusinessName = phy.BusinessName;
            model.BusinessWebsite = phy.BusinessWebsite;
            model.UserName = asp.UserName;
            model.Password = asp.PasswordHash;
            model.RoleId = (int)phy.RoleId;
            model.Role = await _roleRepository.GetRoles();
            model.statusId = (int)phy.Status;
            model.status =  _statusRepository.GetAll().ToList();
            return model;
        }
        public object EditProvider(ProviderViewModel model, int physicianID)
        {

            return model;
        }
        #endregion

        #region provider info
        public object RegionList()
        {
            ProviderInfoViewModel viewModel = new ProviderInfoViewModel();
            viewModel.Regions = _regionRepository.GetRegion();
            return viewModel;
        }
        public  ProviderInfoViewModel ProviderInformation( int RegionId)
        {
            List<Physician> regionwisephysician = _physicianRepository.GetAll().ToList();

            if (RegionId != 0)
            {
                regionwisephysician = regionwisephysician.Where(a => a.RegionId == RegionId).ToList();
            }

            ProviderInfoViewModel data = new ProviderInfoViewModel()
            {
                physicians = regionwisephysician,
                Regions = _regionRepository.GetAll().ToList(),
                Role = _roleRepository.GetRolesProvider().ToList(),
                PhysicianNotifications = _physicianNotificationRepository.GetAll().ToList(),    
                status = _statusRepository.GetAll().ToList(),
            };

            return data;
        }
        public Physician ContectProvider(int id)
        {
            return  _physicianRepository.GetById(id);
            
        }
        #endregion









        #endregion
    }
}
