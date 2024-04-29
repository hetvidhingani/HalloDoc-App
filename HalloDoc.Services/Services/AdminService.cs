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
using Org.BouncyCastle.Utilities.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq.Expressions;
using LinqKit;
using Microsoft.IdentityModel.Tokens;

using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Security.Policy;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.Entity;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Xml.Linq;
using Newtonsoft.Json;

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
        private readonly IMenuRepository _menuRepository;
        private readonly IRoleMenuRepository _roleMenuRepository;
        private readonly IEmailLogsRepository _emailLogsRepository;
        private readonly ISMSLogRepository _smmsLogRepository;
        private readonly IAspNetUserRolesRepository _userRolesRepository;
        private readonly IShiftDetailsRepository _shiftDetailsRepository;
        private readonly IShiftRepository _shiftRepository;
        private readonly IPhysicianRegionRepository _physicianRegionRepository;
        private readonly IPhysicianLocationRepository _physicianLocationRepository;
        private const string AccountSid = "AC224885bd4f29fd4d16ea6dfbdaf4c609";
        private const string AuthToken = "9e16acc4370092159b4970030c4e6a58";
        private const string TwilioPhoneNumber = "+12515722513";
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
                               IPhysicianNotificationRepository physicianNotificationRepository, IStatusRepository statusRepository,
                               IMenuRepository menuRepository, IRoleMenuRepository roleMenuRepository, IEmailLogsRepository emailLogsRepository,
                               ISMSLogRepository smmsLogRepository, IAspNetUserRolesRepository userRolesRepository, IShiftDetailsRepository shiftDetailsRepository,
                               IShiftRepository shiftRepository, IPhysicianLocationRepository physicianLocationRepository, IPhysicianRegionRepository physicianRegionRepository)
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
            _menuRepository = menuRepository;
            _roleMenuRepository = roleMenuRepository;
            _emailLogsRepository = emailLogsRepository;
            _smmsLogRepository = smmsLogRepository;
            _userRolesRepository = userRolesRepository;
            _shiftDetailsRepository = shiftDetailsRepository;
            _shiftRepository = shiftRepository;
            _physicianLocationRepository = physicianLocationRepository;
            _physicianRegionRepository = physicianRegionRepository;
        }
        #endregion

        #region common methods

        public async Task<Admin> GetAdmin(string email)
        {
            Admin user = await _adminRepository.CheckUserByEmail(email);
            return user;
        }


        public async Task<T> GetTempData<T>(string key)
        {
            return await Task.FromResult(_aspnetuserRepository.GetTempData<T>(key));
        }

        public List<Region> getstateDropdown()
        {
            return _regionRepository.GetAll().ToList();
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

                where r.Status == status && r.IsDeleted == null && phy.IsDeleted == null && r.UserId != null

                select new AdminDashboardViewModel
                {
                    PatientName = rec.FirstName + "," + rec.LastName,
                    DateOfBirth = new DateTime((int)rec.IntYear, Convert.ToInt32(rec.StrMonth), (int)rec.IntDate),
                    Requestor = r.FirstName + "," + r.LastName,
                    RequestedDate = r.CreatedDate,
                    PatientPhone = rec.PhoneNumber,
                    RequestorPhone = r.PhoneNumber,
                    Address = rec.Street + "," + rec.City + "," + rec.State + "," + rec.ZipCode,
                    Notes = rec.Request.RequestStatusLogs.OrderBy(u => u.CreatedDate).Last(u => u.RequestId == rec.RequestId).Notes,
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
            if (CurrentPage == 0)
            {

                CurrentPage = 1;
                List<AdminDashboardViewModel> client = new List<AdminDashboardViewModel>();
                client = newState.ToList();
                AdminDashboardViewModel model = new AdminDashboardViewModel();
                model.PagingData = client;
                return model;

            }
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

        public List<AdminDashboardViewModel> ExportAllData()
        {
            List<AdminDashboardViewModel> dashboard = new List<AdminDashboardViewModel>();

            var temp = _requestclientRepository.GetAllData(
                x => new AdminDashboardViewModel
                {
                    PatientName = x.FirstName + " " + x.LastName,
                    DateOfBirth = new DateTime((int)x.IntYear, Convert.ToInt32(x.StrMonth), (int)x.IntDate),
                    Requestor = x.Request.FirstName + " " + x.Request.LastName,
                    RequestedDate = x.Request.CreatedDate,
                    PatientPhone = x.PhoneNumber,
                    RequestorPhone = x.Request.PhoneNumber,
                    Address = x.Street + "," + x.City + "," + x.State + "," + x.ZipCode,
                    Notes = x.Request.RequestStatusLogs.OrderByDescending(u => u.CreatedDate).Last(u => u.RequestId == x.RequestId).Notes,
                    PhysicianName = x.Request.Physician.FirstName + " " + x.Request.Physician.LastName,
                    RequestTypeID = x.Request.RequestTypeId,
                    RequstClientId = x.RequestClientId,
                    requestID = x.RequestId,
                    Email = x.Email,
                    RegionId = (int)x.RegionId,
                    StateofTable = x.State,
                },
                rc => rc.Request.IsDeleted == null && rc.Request.UserId != null);
            foreach (AdminDashboardViewModel item in temp)
            {
                AdminDashboardViewModel dashboardPartialTableViewModel = item;

                dashboard.Add(dashboardPartialTableViewModel);
            }
            dashboard = dashboard.ToList();
            return dashboard;
        }

        public string UnscheduledPhysicians(string message)
        {

            var physiciansNotInShift = _physicianRepository.GetAll().Where(p => _shiftRepository.GetAll().Any(s => s.PhysicianId == p.PhysicianId)).Select(p => p.Email).ToList();
            if (physiciansNotInShift.Count > 0)
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
                    Subject = "Request Support",
                    IsBodyHtml = true,
                    Body = message,
                };
                foreach (string item in physiciansNotInShift)
                {
                    mailMessage.To.Add(item);
                }

                smtpClient.Send(mailMessage);
                return "";

            }
            else
            {
                return "No Physician available";
            }


        }

        #endregion

        #region View Case

        public async Task<object> ViewCase(int userId)
        {
            ViewCaseViewModel viewmodel = new ViewCaseViewModel();
            RequestClient user = await _requestclientRepository.GetByIdAsync(userId);
            Request req = await _requestRepository.GetByIdAsync(user.RequestId);

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
                viewmodel.DateOfBirth = new DateTime((int)user.IntYear, Convert.ToInt32(user.StrMonth), (int)user.IntDate);
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
                viewmodel.PhysicianNotes = requestNote.PhysicianNotes;
            }
            viewmodel.TransferNotes = (from r in _requestRepository.GetAll()
                                       join rsl in _requestStatusLogRepository.GetAll() on r.RequestId equals rsl.RequestId
                                       join p in _physicianRepository.GetAll() on rsl.TransToPhysicianId equals p.PhysicianId into g
                                       from p in g.DefaultIfEmpty()
                                       where r.RequestId == req.RequestId && rsl.Status != 7 && rsl.Status != 3 && rsl.Status != 8 && rsl.Status != 4
                                       orderby rsl.CreatedDate descending
                                       select new TransferNotesViewModel
                                       {
                                           RequestId = r.RequestId,
                                           FirstName = r.FirstName,
                                           LastName = r.LastName,
                                           CreatedDate = rsl.CreatedDate,
                                           Note = rsl.Notes,
                                           status = rsl.Status,
                                           AdminName = rsl.Admin.FirstName + rsl.Admin.LastName,
                                           transferByPhy = rsl.Physician.FirstName + " " + rsl.Physician.LastName,
                                           PhysicianName = p != null ? p.FirstName + " " + p.LastName : null
                                       }).ToList();

            viewmodel.CancelationNotes = (from r in _requestRepository.GetAll()
                                          join rsl in _requestStatusLogRepository.GetAll() on r.RequestId equals rsl.RequestId

                                          where r.RequestId == req.RequestId && (rsl.Status == 7 || rsl.Status == 3 || rsl.Status == 8)
                                          orderby rsl.CreatedDate descending
                                          select new TransferNotesViewModel
                                          {
                                              Note = rsl.Notes,

                                          }).ToList();

            return viewmodel;
        }

        public async Task<object> AddNotes(string additionalNotes, int id, string AdminID)
        {
            RequestNote requestNote = await _requestNotesRepository.CheckByRequestID(id);

            if (requestNote != null)
            {
                requestNote.AdminNotes = additionalNotes;
                await _requestNotesRepository.UpdateAsync(requestNote);

            }
            else
            {
                RequestNote requestNote1 = new RequestNote();
                requestNote1.RequestId = id;
                requestNote1.AdminNotes = additionalNotes;
                requestNote1.CreatedDate = DateTime.Now;
                requestNote1.CreatedBy = AdminID;
                await _requestNotesRepository.AddAsync(requestNote1);

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
        public async Task<string> ConfirmCancelCase(CancelCaseViewModel viewModel, int id, int adminid)
        {
            RequestClient req = await _requestclientRepository.GetByIdAsync(id);
            RequestStatusLog requestNote1 = await _requestStatusLogRepository.CheckByRequestID(req.RequestId);


            if (requestNote1 != null)
            {
                requestNote1.Status = 3;
                requestNote1.AdminId = adminid;
                requestNote1.Notes = viewModel.AdditionalNotes;
                await _requestStatusLogRepository.UpdateAsync(requestNote1);
            }
            else
            {
                RequestStatusLog requesStatusLog = new RequestStatusLog();
                requesStatusLog.Status = 3;
                requesStatusLog.AdminId = adminid;
                requesStatusLog.RequestId = req.RequestId;
                requesStatusLog.Notes = viewModel.AdditionalNotes;
                requesStatusLog.CreatedDate = DateTime.Now;

                await _requestStatusLogRepository.AddAsync(requesStatusLog);
            }


            Request request = await _requestRepository.GetByIdAsync(req.RequestId);

            request.Status = 3;
            request.ModifiedDate = DateTime.Now;
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
        public async Task<string> AssignRequest(AssignCaseViewModel viewModel, int id, int adminID)
        {
            RequestClient req = await _requestclientRepository.GetByIdAsync(id);
            RequestStatusLog requestNote1 = await _requestStatusLogRepository.CheckByRequestID(req.RequestId);
            if (requestNote1 != null)
            {
                requestNote1.Status = 1;
                requestNote1.AdminId = adminID;
                requestNote1.Notes = viewModel.AdditionalNotes;
                await _requestStatusLogRepository.UpdateAsync(requestNote1);
            }

            RequestStatusLog requesStatusLog = new RequestStatusLog();
            requesStatusLog.Status = 1;
            requesStatusLog.AdminId = adminID;
            requesStatusLog.RequestId = req.RequestId;
            requesStatusLog.Notes = viewModel.AdditionalNotes;
            requesStatusLog.CreatedDate = DateTime.Now;
            requesStatusLog.TransToPhysicianId = viewModel.physicianID;
            await _requestStatusLogRepository.AddAsync(requesStatusLog);

            Request request = await _requestRepository.GetByIdAsync(req.RequestId);

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
        public async Task<string> BlockCaseRequest(CancelCaseViewModel viewModel, int id, int adminid)
        {
            RequestClient req = await _requestclientRepository.GetByIdAsync(id);
            RequestStatusLog requestNote1 = await _requestStatusLogRepository.CheckByRequestID(req.RequestId);
            Request requestor = await _requestRepository.GetByIdAsync(req.RequestId);
            if (requestNote1 != null)
            {
                requestNote1.Status = 11;
                requestNote1.AdminId = adminid;
                requestNote1.Notes = viewModel.AdditionalNotes;
                await _requestStatusLogRepository.UpdateAsync(requestNote1);
            }
            else
            {
                RequestStatusLog requesStatusLog = new RequestStatusLog();
                requesStatusLog.Status = 11;
                requesStatusLog.AdminId = adminid;
                requesStatusLog.RequestId = req.RequestId;
                requesStatusLog.Notes = viewModel.AdditionalNotes;
                requesStatusLog.CreatedDate = DateTime.Now;
                await _requestStatusLogRepository.AddAsync(requesStatusLog);
            }

            Request request = await _requestRepository.GetByIdAsync(req.RequestId);
            request.Status = 11;
            await _requestRepository.UpdateAsync(request);

            BlockRequest blockRequest = new BlockRequest();
            blockRequest.PhoneNumber = requestor.PhoneNumber;
            blockRequest.CreatedDate = DateTime.Now;
            blockRequest.Email = requestor.Email;
            blockRequest.Reason = viewModel.AdditionalNotes;
            blockRequest.RequestId = req.RequestId;
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
        public async Task<string> TransferRequest(AssignCaseViewModel viewModel, int id, int adminID)
        {
            RequestClient req = await _requestclientRepository.GetByIdAsync(id);

            RequestStatusLog requesStatusLog = new RequestStatusLog
            {
                Status = 1,
                AdminId = adminID,
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
        public async Task<string> ConfirmCloseCase(int id, int adminid)
        {
            Request req = await _requestRepository.GetByIdAsync(id);
            req.Status = 9;
            await _requestRepository.UpdateAsync(req);
            RequestStatusLog log = new RequestStatusLog()
            {
                RequestId = req.RequestId,
                Status = (short)req.Status,
                AdminId = adminid,
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

        #region Admin My Profile

        public async Task<object> AdminMyProfile(int? adminId)
        {
            Admin admin = await _adminRepository.GetByIdAsync(adminId);
            AspNetUser aspNetUser = await _aspnetuserRepository.GetByIdAsync(admin.AspNetUserId);
            List<AdminRegion> adminRegion = _adminRegionRepository.GetAll().Where(u => u.AdminId == adminId).ToList();
            List<Region> regions = _regionRepository.GetAll().ToList();

            Dictionary<int, bool> checkedRegions = new Dictionary<int, bool>();

            foreach (AdminRegion ar in adminRegion)
            {
                checkedRegions[ar.RegionId] = true;
            }

            AdminMyProfileViewModel model = new AdminMyProfileViewModel()
            {
                AdminID = admin.AdminId,
                UserName = aspNetUser.UserName,
                Password = _aspnetuserRepository.DecodeFrom64(aspNetUser.PasswordHash),
                status = _statusRepository.GetAll().Where(x => x.Statusid == 4 || x.Statusid == 2).ToList(),
                RoleID = (int)admin.RoleId,
                statusId = (int)admin.Status,
                roles = _roleRepository.GetAll().Where(u => u.AccountType == 1).ToList(),
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                ConfirmEmail = admin.Email,
                PhoneNumber = admin.Mobile,
                Address1 = admin.Address1,
                Address2 = admin.Address2,
                City = admin.City,
                RegionId = (int)admin.RegionId,
                State = regions,
                StateCheckbox = regions.Select(r => new RegionViewModel()
                {
                    RegionId = r.RegionId,
                    Name = r.Name,
                    IsChecked = checkedRegions.ContainsKey(r.RegionId)
                }).ToList(),
                Zip = admin.Zip,
                BillingPhoneNumber = admin.AltPhone,
                AdminRegions = adminRegion.Select(r => r.Region).ToList(),
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
        public async Task<object> SaveAdminInfo(AdminMyProfileViewModel model, List<int> ids, int adminId)
        {
            Admin admin = await _adminRepository.GetByIdAsync(adminId);

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

            List<AdminRegion> region = _adminRegionRepository.GetAll().Where(u => u.AdminId == model.AdminID).ToList();
            foreach (var adminregion in region)
            {
                _adminRegionRepository.Remove(adminregion);
            }
            var adminregion1 = ids.Select(id => new AdminRegion
            {
                AdminId = model.AdminID,
                RegionId = id,
            });

            foreach (var newPhysician in adminregion1)
            {
                _adminRegionRepository.AddAsync(newPhysician);
            }
            return user;
        }
        public async Task<object> SaveBillingInfo(AdminMyProfileViewModel model, int adminid)
        {
            Admin admin = await _adminRepository.GetByIdAsync(adminid);
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


            EncounterViewModel model = new EncounterViewModel();

            model.RequestID = req.RequestId;
            model.FirstName = encounter.Firstname;
            model.LastName = encounter.Lastname;
            model.Location = encounter.Location;
            model.DateOfBirth = encounter.Dateofbirth;
            model.PhoneNumber = encounter.Phonenumber;
            model.MedicalReport = encounter.Medicalreport;
            model.Date = encounter.Date;
            model.Email = encounter.Email;
            model.HistoryOfPresentIllness = encounter.Historyofpresentillness;
            model.MedicalHistory = encounter.Medicalhistory;
            model.Medications = encounter.Medications;
            model.Temp = encounter.Temp;
            model.BloodPressureSystolic = encounter.Bloodpressuresystolic;
            model.BloodPressureDiastolic = encounter.Bloodpressurediastolic;
            model.Heent = encounter.Heent;
            model.Chest = encounter.Chest;
            model.Hr = encounter.Hr;
            model.Allergies = encounter.Allergies;
            model.Cv = encounter.Cv;
            model.ABD = encounter.Abd;
            model.Extr = encounter.Extr;
            model.Skin = encounter.Skin;
            model.Neuro = encounter.Neuro;
            model.Diagnosis = encounter.Diagnosis;
            model.MedicationsDispensed = encounter.Medicationsdispensed;
            model.Followup = encounter.Followup;
            model.Other = encounter.Other;
            model.TreatmentPlan = encounter.Treatmentplan;
            model.Procedures = encounter.Procedures;
            model.Rr = encounter.Rr;
            model.Pain = encounter.Pain;
            model.IsChanged = encounter.Ischanged;
            model.IsFinalized = encounter.Isfinalized;

            return model;
        }

        public async Task<object> EncounterFormSaveChanges(EncounterViewModel model)
        {
            Encounter encounter = await _encounterRepository.checkBYRequestID(model.RequestID);
            encounter.Firstname = model.FirstName;
            encounter.Lastname = model.LastName;
            encounter.Location = model.Location;
            encounter.Dateofbirth = model.DateOfBirth;
            encounter.Phonenumber = model.PhoneNumber;
            encounter.Medicalreport = model.MedicalReport;
            encounter.Date = model.Date;
            encounter.Email = model.Email;
            encounter.Historyofpresentillness = model.HistoryOfPresentIllness;
            encounter.Medicalhistory = model.MedicalHistory;
            encounter.Medications = model.Medications;
            encounter.Temp = model.Temp;
            encounter.Bloodpressuresystolic = model.BloodPressureSystolic;
            encounter.Bloodpressurediastolic = model.BloodPressureDiastolic;
            encounter.Heent = model.Heent;
            encounter.Chest = model.Chest;
            encounter.Hr = model.Hr;
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
            encounter.Rr = model.Rr;
            encounter.Pain = model.Pain;
            encounter.Ischanged = model.IsChanged;
            encounter.Isfinalized = model.IsFinalized;
            await _encounterRepository.UpdateAsync(encounter);

            return encounter;
        }
        #endregion

        #region Create  Provider
        private static readonly HttpClient client = new HttpClient();
        public async Task<object> Createprovider()
        {
            ProviderViewModel model = new ProviderViewModel();
            model.State = await _regionRepository.GetRegions();
            model.Role = _roleRepository.GetAll().Where(u => u.AccountType == 2).ToList();
            model.status = _statusRepository.GetAll().Where(x => x.Statusid == 4 || x.Statusid == 2).ToList();

            return model;
        }
        public async Task<object> CreateProvider(ProviderViewModel model)
        {
            AspNetUser user = new AspNetUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.FirstName+"."+model.LastName,
                PasswordHash = _aspnetuserRepository.EncodePasswordToBase64(model.Password),
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CreatedDate = DateTime.Now,
            };
            await _aspnetuserRepository.AddAsync(user);
            AspNetUserRole userRole = new AspNetUserRole
            {
                UserId = user.Id,
                RoleId = "3"

            };
            await _userRolesRepository.AddAsync(userRole);

            Physician physician = new Physician();

            physician.Id = user.Id;
            physician.FirstName = model.FirstName;
            physician.LastName = model.LastName;
            physician.Email = model.Email;
            physician.MedicalLicense = model.MedicalLicense;
            physician.Npinumber = model.NPINumber;
            physician.SyncEmailAddress = model.SyncEmail;
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
            physician.Photo = model.Photo == null ? null : model.Photo.FileName;


            await _physicianRepository.AddAsync(physician);
            if (model.physicianRegionids != null)
            {
                foreach (var u in model.physicianRegionids)
                {
                    PhysicianRegion adminRegion = new PhysicianRegion()
                    {
                        PhysicianId = physician.PhysicianId,
                        RegionId = u,
                    };
                    await _physicianRegionRepository.AddAsync(adminRegion);
                }
            }

            if (model.Photo != null)
            {
                var fileName = $"{physician.PhysicianId}_Photo.jpg";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads\physician\image", fileName);
                using var stream = System.IO.File.Create(filePath);
                model.Photo.CopyTo(stream);
            }

            if (model.IsAgreementDoc == true && model.contractoragreement != null)
            {

                var fileName1 = $"{physician.PhysicianId}_contractoragreement.pdf";
                var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads\physician\doc", fileName1);
                using var stream1 = System.IO.File.Create(filePath1);
                model.contractoragreement.CopyTo(stream1);
                physician.IsAgreementDoc = new BitArray(new bool[] { true });
            }
            if (model.isbackgroundcheck == true && model.backgroundcheck != null)
            {

                var fileName2 = $"{physician.PhysicianId}_backgroundcheck.pdf";
                var filePath2 = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads\physician\doc", fileName2);
                using var stream2 = System.IO.File.Create(filePath2);
                model.backgroundcheck.CopyTo(stream2);
                physician.IsBackgroundDoc = new BitArray(new bool[] { true });
            }

            if (model.Ishippa == true && model.hippa != null)
            {

                var fileName3 = $"{physician.PhysicianId}_hippa.pdf";
                var filePath3 = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads\physician\doc", fileName3);
                using var stream3 = System.IO.File.Create(filePath3);
                model.hippa.CopyTo(stream3);
                physician.IsTrainingDoc = new BitArray(new bool[] { true });
            }

            if (model.IsAgreementDocnondisclosure == true && model.nondisclosure != null)
            {

                var fileName4 = $"{physician.PhysicianId}_nonDisclosure.pdf";
                var filePath4 = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads\physician\doc", fileName4);
                using var stream4 = System.IO.File.Create(filePath4);
                model.nondisclosure.CopyTo(stream4);
                physician.IsNonDisclosureDoc = new BitArray(new bool[] { true });
            }

            await _physicianRepository.UpdateAsync(physician);

            PhysicianLocation ploc = new PhysicianLocation();
            ploc.PhysicianId = physician.PhysicianId;
          
            ploc.PhysicianName = physician.FirstName+" " +physician.LastName;
            ploc.Address = model.City;
            ploc.CreatedDate = DateTime.Now;
            string address = $"{model.Address1}, {model.Address2}, {model.City}";

            string url = $"https://dev.virtualearth.net/REST/v1/Locations?query={Uri.EscapeDataString(address)}&key=ArFwtO9nS9mpwQiIqtwwQroNwrdxAI8dJCEHmjXKYWKAW1o7RTH1Db1Pj1wZxQ1K";

            var response = await client.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<BingMapsResponse>(content);

            if (result.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault() is BingMapsResponse.ResourceSet.Resource bingMapsResult)
            {
                ploc.Latitude = (decimal?)bingMapsResult.Point.Coordinates[0];
                ploc.Longitude = (decimal?)bingMapsResult.Point.Coordinates[1];
               
            }
            await _physicianLocationRepository.AddAsyncss(ploc);



            return "ProviderInformation";
        }

        #endregion

        #region Create Admin
        public async Task<object> CreateAdmin()
        {
            AdminMyProfileViewModel model = new AdminMyProfileViewModel();
            model.State = await _regionRepository.GetRegions();
            model.roles = _roleRepository.GetAll().Where(u => u.AccountType == 1).ToList();

            return model;
        }
        public async Task<object> CreateAdmin(AdminMyProfileViewModel model)
        {
            AspNetUser user = new AspNetUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                PasswordHash = _aspnetuserRepository.EncodePasswordToBase64(model.Password),
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CreatedDate = DateTime.Now,
            };
            await _aspnetuserRepository.AddAsync(user);

            AspNetUserRole userRole = new AspNetUserRole
            {
                UserId = user.Id,
                RoleId = "1"

            };
            await _userRolesRepository.AddAsync(userRole);
            Admin admin = new Admin()
            {
                AspNetUserId = user.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Mobile = model.PhoneNumber,
                Address1 = model.Address1,
                Address2 = model.Address2,
                City = model.City,
                RegionId = model.RegionId,
                Zip = model.Zip,
                AltPhone = model.BillingPhoneNumber,
                CreatedBy = user.Id,
                CreatedDate = DateTime.Now,
                Status = 1,
                RoleId = model.RoleID
            };
            await _adminRepository.AddAsync(admin);

            foreach (var u in model.AdminRegionID)
            {
                AdminRegion adminRegion = new AdminRegion()
                {
                    AdminId = admin.AdminId,
                    RegionId = u,
                };
                await _adminRegionRepository.AddAsync(adminRegion);
            }

            return model;
        }
        #endregion

        #region edit provider
        public async Task<object> EditProvider(int physicianID)
        {
            List<PhysicianRegion> adminRegion = _physicianRegionRepository.GetAll().Where(u => u.PhysicianId == physicianID).ToList();
            List<Region> regions = _regionRepository.GetAll().ToList();

            Dictionary<int, bool> checkedRegions = new Dictionary<int, bool>();

            foreach (PhysicianRegion ar in adminRegion)
            {
                checkedRegions[ar.RegionId] = true;
            }
            Physician phy = await _physicianRepository.GetByIdAsync(physicianID);
            AspNetUser asp = await _aspnetuserRepository.getById(phy.Id);
            ProviderViewModel model = new ProviderViewModel();
            model.UserName = phy.FirstName + phy.LastName;
            model.PhysicianId = physicianID;
            model.FirstName = phy.FirstName;
            model.LastName = phy.LastName;
            model.PhoneNumber = phy.Mobile;
            model.AdminNotes = phy.AdminNotes;
            model.Email = phy.Email;
            model.MedicalLicense = phy.MedicalLicense;
            model.NPINumber = phy.Npinumber;
            model.Address1 = phy.Address1;
            model.Address2 = phy.Address2;
            model.City = phy.City;
            model.RegionId = (int)phy.RegionId;
            model.Regions = await _regionRepository.GetRegions();
            model.Zip = phy.Zip;
            model.BillingPhoneNumber = phy.AltPhone;
            model.BusinessName = phy.BusinessName;
            model.BusinessWebsite = phy.BusinessWebsite;
            model.Password = _aspnetuserRepository.DecodeFrom64(asp.PasswordHash);
            model.RoleId = (int)phy.RoleId;
            model.Role = _roleRepository.GetAll().Where(u => u.AccountType == 2).ToList();
            model.statusId = (int)phy.Status;
            model.status = _statusRepository.GetAll().Where(x => x.Statusid == 4 || x.Statusid == 2).ToList();
            model.IsAgreementDoc = phy.IsAgreementDoc == null ? false : true;
            model.isbackgroundcheck = phy.IsBackgroundDoc == null ? false : true;
            model.IsAgreementDocnondisclosure = phy.IsNonDisclosureDoc == null ? false : true;
            model.Ishippa = phy.IsTrainingDoc == null ? false : true;
            model.filename = phy.PhysicianId + "_Signature.png";
            model.IsSignature = phy.Signature == null ? false : true;
            model.StateCheckbox = regions.Select(r => new RegionViewModel()
            {
                RegionId = r.RegionId,
                Name = r.Name,
                IsChecked = checkedRegions.ContainsKey(r.RegionId)
            }).ToList();
            return model;
        }
        public void DeleteProvider(int id)
        {
            Physician phy = _physicianRepository.GetById(id);
            phy.IsDeleted = new BitArray(new bool[] { true });
            _physicianRepository.UpdateAsync(phy);
        }
        #endregion

        #region provider info
        public object RegionList()
        {
            ProviderInfoViewModel viewModel = new ProviderInfoViewModel();
            viewModel.Regions = _regionRepository.GetRegion();
            return viewModel;
        }
        public ProviderInfoViewModel ProviderInformation(int RegionId, int CurrentPage)
        {
            Expression<Func<Physician, bool>> tableData = PredicateBuilder.New<Physician>();
            tableData = x => true;
            List<TableProviderInfo> AccountAccessTable = new List<TableProviderInfo>();
            if (RegionId != 0)
            {
                tableData = tableData.And(e => e.RegionId == RegionId);
            }
            var data = _physicianRepository.GetAllWithPagination(x => new TableProviderInfo
            {
                onCallStatus = "Un Available",
                PhysicianID = x.PhysicianId,
                ProviderName = x.FirstName + " " + x.LastName,
                stopNotification = x.PhysicianNotifications.First(u => u.PhysicianId == x.PhysicianId) == null ? null : "1",
                roleName = x.Role.Name,
                status = (int)x.Status,

            }, tableData.And(x => x.IsDeleted == null), CurrentPage, 5, x => x.FirstName, true);

            foreach (TableProviderInfo requiredData in data)
            {
                AccountAccessTable.Add(requiredData);
            }

            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = _physicianRepository.GetTotalCount(tableData.And(x => x.IsDeleted == null));
            int totalPage = (int)Math.Ceiling((double)totalCount / dataSize);
            int FirstItemIndex = Math.Min((CurrentPage - 1) * dataSize + 1, totalCount);
            int LastItemIndex = Math.Min(CurrentPage * dataSize, totalCount);

            return new ProviderInfoViewModel
            {
                PagingData = AccountAccessTable,
                TotalCount = totalCount,
                TotalPages = totalPage,
                CurrentPage = CurrentPage,
                PageSize = 3,
                FirstItemIndex = FirstItemIndex,
                LastItemIndex = LastItemIndex,
            };
        }
        public object ContectProviderModel(int id)
        {
            Physician phy = _physicianRepository.GetById(id);
            ContactProviderViewModel viewModel = new ContactProviderViewModel();
            viewModel.physicianId = id;
            viewModel.phone = phy.Mobile;
            viewModel.email = phy.Email;
            viewModel.roleID = (int)phy.RoleId;
            return viewModel;
        }
        public Physician ContectProvider(int id)
        {
            return _physicianRepository.GetById(id);

        }

        public void StopNotificationPhysician(List<int> ids)
        {
            var existingPhysicians = _physicianNotificationRepository.GetAll().ToList();

            //var physiciansToRemove = existingPhysicians.Where(p => ids.Contains(p.PhysicianId)).ToList();
            foreach (var physician in existingPhysicians)
            {
                _physicianNotificationRepository.Remove(physician);
            }

            var newPhysicians = ids.Select(id => new PhysicianNotification
            {
                PhysicianId = id,
                IsNotificationStopped = new BitArray(new bool[] { true }),
            });
            foreach (var newPhysician in newPhysicians)
            {
                _physicianNotificationRepository.AddAsync(newPhysician);
            }

        }
        public void sendSMS(ContactProviderViewModel model, int adminId)
        {
            Physician physician = _physicianRepository.GetById(model.physicianId);
            var message1 = "Admin wants to contact you : " + model.message;
            var toPhoneNumber = physician.Mobile;
            SendSMS(toPhoneNumber, message1);
            Smslog smslog = new()
            {
                Smstemplate = model.message,
                MobileNumber = model.phone,
                RoleId = model.roleID,
                AdminId = adminId,
                PhysicianId = model.physicianId,
                CreateDate = DateTime.Now,
                SentDate = DateTime.Now,

                IsSmssent = new BitArray(new[] { true })
            };
            _smmsLogRepository.AddAsync(smslog);
        }
        public void SendSMS(string toPhoneNumber, string message)
        {
            TwilioClient.Init(AccountSid, AuthToken);
            var messageOptions = new CreateMessageOptions(
            new PhoneNumber(toPhoneNumber));
            messageOptions.From = new PhoneNumber("+12515722513");
            messageOptions.Body = message;
            var messages = MessageResource.Create(messageOptions);
            Console.WriteLine(messages.Body);
        }
        #endregion

        #region save provider
        public async Task<object> resetPasswordProvider(int id, string password)
        {
            Physician phy = await _physicianRepository.GetByIdAsync(id);
            AspNetUser user = await _aspnetuserRepository.GetByIdAsync(phy.Id);
            user.PasswordHash = _aspnetuserRepository.EncodePasswordToBase64(password);
            await _aspnetuserRepository.UpdateAsync(user);
            return user;
        }
        public async Task<object> resetRoleStatus(ProviderViewModel model, int id)
        {
            Physician phy = await _physicianRepository.GetByIdAsync(id);
            phy.RoleId = model.RoleId;
            phy.Status = (short?)model.statusId;
            await _physicianRepository.UpdateAsync(phy);
            return phy;
        }
        public Physician savePhysicianInformation(ProviderViewModel model, int id, List<int> ids)
        {
            Physician physician = _physicianRepository.GetById(id);

            physician.FirstName = model.FirstName;
            physician.LastName = model.LastName;
            physician.Email = model.Email;
            physician.Mobile = model.PhoneNumber;
            physician.MedicalLicense = model.MedicalLicense;
            physician.Npinumber = model.NPINumber;
            physician.SyncEmailAddress = model.SyncEmail;
            _physicianRepository.UpdateAsync(physician);

            AspNetUser user = _aspnetuserRepository.GetAll().Where(x => x.Id == physician.Id).FirstOrDefault();
            user.Email = model.Email;
            _aspnetuserRepository.UpdateAsync(user);

            List<PhysicianRegion> region = _physicianRegionRepository.GetAll().Where(u => u.PhysicianId == model.PhysicianId).ToList();
            foreach (var adminregion in region)
            {
                _physicianRegionRepository.Remove(adminregion);
            }

            var adminregion1 = ids.Select(id => new PhysicianRegion
            {
                PhysicianId = model.PhysicianId,
                RegionId = id,
            });

            foreach (var newPhysician in adminregion1)
            {
                _physicianRegionRepository.AddAsync(newPhysician);
            }
            return physician;
        }
        public object saveBillingInformation(ProviderViewModel model, int id)
        {
            Physician physician = _physicianRepository.GetById(id);
            physician.Address1 = model.Address1;
            physician.Address2 = model.Address2;
            physician.RegionId = model.RegionId;
            physician.Zip = model.Zip;
            physician.AltPhone = model.BillingPhoneNumber;
            _physicianRepository.UpdateAsync(physician);

            return physician;
        }
        public object providerProfile(ProviderViewModel model, int id)
        {
            Physician physician = _physicianRepository.GetById(id);
            physician.BusinessName = model.BusinessName;
            physician.BusinessWebsite = model.BusinessWebsite;
            physician.AdminNotes = model.AdminNotes;
            if (model.Photo != null)
            {
                var newName = $"{physician.PhysicianId}_Photo.jpg";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/physician/image", newName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                using var stream = System.IO.File.Create(filePath);
                model.Photo.CopyTo(stream);
                physician.Photo = model.Photo.FileName;
            }
            if (model.Signature != null)
            {
                var newName = $"{physician.PhysicianId}_Signature.png";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/physician/image", newName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                using var stream = System.IO.File.Create(filePath);
                model.Signature.CopyTo(stream);

                physician.Signature = model.Signature.FileName;
            }
            _physicianRepository.UpdateAsync(physician);

            return physician;
        }
        public object documentsProvider(ProviderViewModel model)
        {
            Physician physician = _physicianRepository.GetById(model.PhysicianId);

            if (model.IsAgreementDoc == true)
            {
                if (model.contractoragreement != null)
                {

                    var newName = $"{model.PhysicianId}_contractoragreement.pdf";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/physician/doc", newName);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    using var stream = System.IO.File.Create(filePath);
                    model.contractoragreement.CopyTo(stream);

                    physician.IsAgreementDoc = new BitArray(new bool[] { true });
                }
                else if (model.contractoragreement == null && physician.IsAgreementDoc != null)
                {
                    physician.IsAgreementDoc = new BitArray(new bool[] { true });

                }

            }
            else
            {
                physician.IsAgreementDoc = null;

            }
            if (model.isbackgroundcheck == true)
            {

                if (model.backgroundcheck != null)
                {

                    var newName = $"{model.PhysicianId}_backgroundcheck.pdf";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/physician/doc", newName);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    using var stream = System.IO.File.Create(filePath);
                    model.backgroundcheck.CopyTo(stream);

                    physician.IsBackgroundDoc = new BitArray(new bool[] { true });
                }
                else if (model.backgroundcheck == null && physician.IsBackgroundDoc != null)
                {
                    physician.IsBackgroundDoc = new BitArray(new bool[] { true });

                }
            }
            else
            {
                physician.IsBackgroundDoc = null;
            }
            if (model.Ishippa == true)
            {
                if (model.hippa != null)
                {

                    var newName = $"{model.PhysicianId}_hippa.pdf";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/physician/doc", newName);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    using var stream = System.IO.File.Create(filePath);
                    model.hippa.CopyTo(stream);

                    physician.IsTrainingDoc = new BitArray(new bool[] { true });
                }
                else if (model.hippa == null && physician.IsTrainingDoc != null)
                {
                    physician.IsTrainingDoc = new BitArray(new bool[] { true });

                }
            }
            else
            {
                physician.IsTrainingDoc = null;
            }
            if (model.IsAgreementDocnondisclosure == true)
            {
                if (model.nondisclosure != null)
                {

                    var newName = $"{model.PhysicianId}_nonDisclosure.pdf";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/physician/doc", newName);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    using var stream = System.IO.File.Create(filePath);
                    model.nondisclosure.CopyTo(stream);

                    physician.IsNonDisclosureDoc = new BitArray(new bool[] { true });
                }
                else if (model.nondisclosure == null && physician.IsNonDisclosureDoc != null)
                {
                    physician.IsNonDisclosureDoc = new BitArray(new bool[] { true });

                }
            }
            else
            {
                physician.IsNonDisclosureDoc = null;
            }



            _physicianRepository.UpdateAsync(physician);

            return physician;
        }

        public void addSignature(ProviderViewModel model, int id)
        {
            Physician physician = _physicianRepository.GetById(id);
            if (model.Signature.FileName != null)
            {
                var newName = $"{physician.PhysicianId}_Signature.png";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/physician/image", newName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                using var stream = System.IO.File.Create(filePath);
                model.Signature.CopyTo(stream);

                physician.Signature = model.Signature.FileName;
            }
            _physicianRepository.UpdateAsync(physician);
        }
        #endregion

        #region Account Access

        public async Task<object> AccountAccessTable(int CurrentPage)
        {

            Expression<Func<Role, bool>> tableData = PredicateBuilder.New<Role>();

            tableData = x => true;
            List<TableAccountAccess> AccountAccessTable = new List<TableAccountAccess>();

            var data = _roleRepository.GetAllWithPagination(x => new TableAccountAccess
            {
                roleID = x.RoleId,
                AccountType = x.AccountType == 1 ? "Admin" : "Physician",
                Name = x.Name,

            }, tableData.And(x => x.IsDeleted == null), CurrentPage, 5, x => x.Name, true);

            foreach (TableAccountAccess requiredData in data)
            {
                AccountAccessTable.Add(requiredData);
            }

            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = _roleRepository.GetTotalCount(tableData.And(x => x.IsDeleted == null));
            int totalPage = (int)Math.Ceiling((double)totalCount / dataSize);
            int FirstItemIndex = Math.Min((CurrentPage - 1) * dataSize + 1, totalCount);
            int LastItemIndex = Math.Min(CurrentPage * dataSize, totalCount);

            return new AccountAccessViewModel
            {
                PagingData = AccountAccessTable,
                TotalCount = totalCount,
                TotalPages = totalPage,
                CurrentPage = CurrentPage,
                PageSize = 3,
                FirstItemIndex = FirstItemIndex,
                LastItemIndex = LastItemIndex,
            };
        }


        #endregion

        #region Create Role
        public AccountAccessViewModel CreateRole(int AdminId)
        {
            var Admin = _adminRepository.GetById(AdminId);
            AccountAccessViewModel viewModel = new AccountAccessViewModel()
            {
                AdminName = Admin.FirstName + " " + Admin.LastName,
                roles = _roleRepository.GetAll().ToList(),
            };
            return viewModel;
        }
        public AccountAccessViewModel MenuName(int AccountTypeId, int typename, int id = 0)
        {
            List<Menu> menu = null;

            List<RoleMenu> rolemenu = _roleMenuRepository.GetAll().ToList();

            if (AccountTypeId == 1)
            {
                List<Menu> menu1 = _menuRepository.GetAll().ToList();
                menu = menu1.Where(u => u.AccountType == 1).ToList();
            }
            if (AccountTypeId == 2)
            {
                List<Menu> menu2 = _menuRepository.GetAll().ToList();

                menu = menu2.Where(u => u.AccountType == 2).ToList();
            }

            AccountAccessViewModel data = new()
            {
                MenuNames = menu,
                rolemenus = rolemenu.Where(u => u.RoleId == id).ToList(),
                type = typename
            };

            return data;

        }
        public async Task<object> CreateAccess(AccountAccessViewModel viewModel, string aspAdmin)
        {
            List<Role> roles = _roleRepository.GetAll().ToList();
            var role = new Role()
            {
                Name = viewModel.RoleName,
                AccountType = (short)viewModel.accountTypeId,
                CreatedDate = DateTime.Now,
                CreatedBy = aspAdmin,
            };
            await _roleRepository.AddAsync(role);

            var roleName = _roleRepository.findByName(viewModel.RoleName);
            foreach (var roleMenuId in viewModel.RoleId)
            {
                var roleMenu = new RoleMenu()
                {
                    RoleId = roleName.RoleId,
                    MenuId = roleMenuId
                };
                await _roleMenuRepository.AddAsync(roleMenu);
            }
            return "";
        }
        #endregion

        #region Edit Role
        public AccountAccessViewModel EditAccountAccess(int id, int AdminId)
        {
            var Admin = _adminRepository.GetAll().Where(u => u.AdminId == AdminId).FirstOrDefault();
            var role = _roleRepository.GetAll().Where(u => u.RoleId == id).FirstOrDefault();

            AccountAccessViewModel viewModel = new AccountAccessViewModel()
            {
                AdminName = Admin.FirstName + " " + Admin.LastName,
                roles = _roleRepository.GetAll().ToList(),
                RoleName = role.Name,
                accountTypeId = role.AccountType,
                roleinput = id
            };
            return viewModel;
        }
        public async Task<object> submitEditAccess(AccountAccessViewModel viewModel)
        {
            var roles = _roleRepository.GetAll().Where(u => u.RoleId == viewModel.roleinput).FirstOrDefault();

            roles.Name = viewModel.RoleName;
            await _roleRepository.UpdateAsync(roles);

            var roleMenu = _roleMenuRepository.GetAll().Where(u => u.RoleId == viewModel.roleinput).ToList();

            foreach (var rolemenu in roleMenu)
            {
                _roleMenuRepository.Remove(rolemenu);
            }


            foreach (var roleMenuId in viewModel.RoleId)
            {
                var roleMenu1 = new RoleMenu()
                {
                    RoleId = viewModel.roleinput,
                    MenuId = roleMenuId
                };
                await _roleMenuRepository.AddAsync(roleMenu1);
            }
            return roles;
        }

        public async Task<object> deleteAccountAccess(int id)
        {
            Role role = _roleRepository.GetAll().Where(u => u.RoleId == id).FirstOrDefault();
            role.IsDeleted = new BitArray(new bool[] { true });
            await _roleRepository.UpdateAsync(role);
            return role.RoleId;
        }

        #endregion

        #region User Access

        public UserAccessViewModel UserAccess(int accountTypeId, int CurrentPage)
        {

            List<TableUserAccess> userAccessTable = new List<TableUserAccess>();
            Expression<Func<Admin, bool>> tableData = PredicateBuilder.New<Admin>();

            tableData = x => true;
            Expression<Func<Physician, bool>> tableData2 = PredicateBuilder.New<Physician>();

            tableData2 = x => true;

            if (accountTypeId == 0 || accountTypeId == 2)
            {
                var temp = _adminRepository.GetAllData(x => new TableUserAccess
                {
                    AdminID = x.AdminId,
                    AccountPOC = x.FirstName + ", " + x.LastName,
                    AccountType = "Admin",
                    OpenRequest = 0,
                    Phone = x.Mobile,
                    PhysicianID = 0,
                    Status = x.StatusNavigation.Statusname
                }, x => x.IsDeleted == null);
                foreach (TableUserAccess item in temp)
                {
                    userAccessTable.Add(item);
                }
            }
            if (accountTypeId == 0 || accountTypeId == 1)
            {
                var temp = _physicianRepository.GetAllData(x => new TableUserAccess
                {
                    AdminID = 0,
                    AccountPOC = x.FirstName + ", " + x.LastName,
                    AccountType = "Physician",
                    OpenRequest = 0,
                    Phone = x.Mobile,
                    PhysicianID = x.PhysicianId,
                    Status = x.StatusNavigation.Statusname

                }, x => x.IsDeleted == null);
                foreach (TableUserAccess item in temp)
                {
                    userAccessTable.Add(item);
                }
            }

            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = userAccessTable.Count;

            int totalPage = (int)Math.Ceiling((double)totalCount / dataSize);
            int FirstItemIndex = Math.Min((CurrentPage - 1) * dataSize + 1, totalCount);
            int LastItemIndex = Math.Min(CurrentPage * dataSize, totalCount);
            List<TableUserAccess> clients = userAccessTable

              .Skip((CurrentPage - 1) * dataSize)
              .Take(dataSize)
              .ToList();
            return new UserAccessViewModel
            {
                PagingData = clients,
                TotalCount = totalCount,
                TotalPages = totalPage,
                CurrentPage = CurrentPage,
                PageSize = 3,
                FirstItemIndex = FirstItemIndex,
                LastItemIndex = LastItemIndex,
            };
        }
        #endregion

        #region Vendor Details
        public VendorsViewModel VendorDetail()
        {
            VendorsViewModel vendors = new VendorsViewModel()
            {
                ProfessionType = _healthProfessionalTypeRepository.GetAll().ToList(),
            };
            return vendors;
        }

        public VendorsViewModel VendorTable(int VendorProfessionTypeId, string VendorName, int CurrentPage)
        {
            Expression<Func<HealthProfessional, bool>> tableData = PredicateBuilder.New<HealthProfessional>();

            tableData = x => true;
            if (!string.IsNullOrWhiteSpace(VendorName))
            {
                tableData = tableData.And(e => e.VendorName.ToLower().Contains(VendorName.ToLower()));
            }
            if (VendorProfessionTypeId != 0)
            {
                tableData = tableData.And(e => e.Profession == VendorProfessionTypeId);
            }
            List<TableModelVendor> vendorTable = new List<TableModelVendor>();

            var data = _healthProfessionalsRepository.GetAllWithPagination(x => new TableModelVendor
            {
                VendorId = x.VendorId,
                Profession = x.ProfessionNavigation.ProfessionName,
                BusinessContact = x.BusinessContact,
                BusinessName = x.VendorName,
                Email = x.Email,
                FaxNumber = x.FaxNumber,
                PhoneNumber = x.PhoneNumber,
            }, tableData.And(x => x.IsDeleted == null), CurrentPage, 5, x => x.VendorName, true);

            foreach (TableModelVendor requiredData in data)
            {
                vendorTable.Add(requiredData);
            }

            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = _healthProfessionalsRepository.GetTotalCount(tableData.And(x => x.IsDeleted == null));
            int totalPage = (int)Math.Ceiling((double)totalCount / dataSize);
            int FirstItemIndex = Math.Min((CurrentPage - 1) * dataSize + 1, totalCount);
            int LastItemIndex = Math.Min(CurrentPage * dataSize, totalCount);

            return new VendorsViewModel
            {
                PagingData = vendorTable,
                TotalCount = totalCount,
                TotalPages = totalPage,
                CurrentPage = CurrentPage,
                PageSize = 3,
                FirstItemIndex = FirstItemIndex,
                LastItemIndex = LastItemIndex,
            };
        }

        public VendorsViewModel AddVendor()
        {
            VendorsViewModel model = new VendorsViewModel()
            {
                ProfessionType = _healthProfessionalTypeRepository.GetAll().ToList(),
                regions = _regionRepository.GetAll().ToList(),
            };
            return model;
        }

        public async Task<VendorsViewModel> AddVendor(VendorsViewModel model)
        {
            HealthProfessional healthProfessional = _healthProfessionalsRepository.GetById(model.VendorID);
            if (healthProfessional == null)
            {
                HealthProfessional vendor = new()
                {
                    VendorName = model.vendorName,
                    Profession = model.ProfessionTypeID,
                    FaxNumber = model.FaxNumber,
                    Address = model.street,
                    City = model.City,
                    RegionId = model.RegionID,
                    State = await _regionRepository.FindState(model.RegionID),
                    Zip = model.zip,
                    CreatedDate = DateTime.Now,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    BusinessContact = model.BusinessContact,
                };
                await _healthProfessionalsRepository.AddAsync(vendor);
            }
            else
            {
                healthProfessional.VendorName = model.vendorName;
                healthProfessional.Profession = model.ProfessionTypeID;
                healthProfessional.FaxNumber = model.FaxNumber;
                healthProfessional.Address = model.street;
                healthProfessional.City = model.City;
                healthProfessional.RegionId = model.RegionID;
                healthProfessional.State = await _regionRepository.FindState(model.RegionID);
                healthProfessional.Zip = model.zip;
                healthProfessional.CreatedDate = DateTime.Now;
                healthProfessional.PhoneNumber = model.PhoneNumber;
                healthProfessional.Email = model.Email;
                healthProfessional.BusinessContact = model.BusinessContact;
                await _healthProfessionalsRepository.UpdateAsync(healthProfessional);
            }
            return model;
        }

        public VendorsViewModel EditVendor(int id)
        {
            HealthProfessional healthProfessional = _healthProfessionalsRepository.GetById(id);
            VendorsViewModel model = new VendorsViewModel()
            {
                VendorID = healthProfessional.VendorId,
                vendorName = healthProfessional.VendorName,
                ProfessionTypeID = (int)healthProfessional.Profession,
                FaxNumber = healthProfessional.FaxNumber,
                street = healthProfessional.Address,
                City = healthProfessional.City,
                RegionID = (int)healthProfessional.RegionId,
                zip = healthProfessional.Zip,
                PhoneNumber = healthProfessional.PhoneNumber,
                Email = healthProfessional.Email,
                BusinessContact = healthProfessional.BusinessContact,
                ProfessionType = _healthProfessionalTypeRepository.GetAll().ToList(),
                regions = _regionRepository.GetAll().ToList(),
            };
            return model;
        }

        public void DeleteVendor(int id)
        {
            HealthProfessional healthProfessional = _healthProfessionalsRepository.GetById(id);
            healthProfessional.IsDeleted = new BitArray(new bool[] { true });
            _healthProfessionalsRepository.UpdateAsync(healthProfessional);
        }

        #endregion

        #region Email Logs & SMS Logs
        public LogsViewModel Logs()
        {
            LogsViewModel model = new LogsViewModel()
            {
                role = _roleRepository.GetAll().ToList(),

            };
            return model;
        }
        public LogsViewModel LogTable(int RoleID, string ReciverName, string email, string phoneNo, DateTime? createdDate, DateTime? sentDate, int type, int CurrPage)
        {
            if (type == 1)
            {
                Expression<Func<EmailLog, bool>> tableData = PredicateBuilder.New<EmailLog>();

                tableData = x => true;

                if (!string.IsNullOrWhiteSpace(ReciverName))
                {
                    tableData = tableData.And(e => e.Request.RequestClients.First(u => u.RequestId == e.RequestId).FirstName.ToLower().Contains(ReciverName.ToLower()) ||
                    e.Request.RequestClients.First(u => u.RequestId == e.RequestId).LastName.ToLower().Contains(ReciverName.ToLower()));
                }
                if (RoleID != 0 && RoleID != null)
                {
                    tableData = tableData.And(e => e.RoleId == RoleID);
                }
                if (!string.IsNullOrWhiteSpace(email))
                {
                    tableData = tableData.And(e => e.EmailId.ToLower().Contains(email.ToLower()));
                }
                if (createdDate.HasValue)
                {
                    tableData = tableData.And(e => e.CreateDate.Date == createdDate.Value.Date);
                }

                if (sentDate.HasValue)
                {
                    tableData = tableData.And(e => e.SentDate == sentDate.Value.Date);
                }
                List<TableModelLogs> logTable = new List<TableModelLogs>();

                var data = _emailLogsRepository.GetAllWithPagination(x => new TableModelLogs
                {
                    EmailLogId = x.EmailLogId,
                    reciverName = x.Request.RequestClients.First(u => u.RequestId == x.RequestId).FirstName + " " + x.Request.RequestClients.First(u => u.RequestId == x.RequestId).LastName,
                    action = x.SubjectName,
                    roleName = x.Role.Name,
                    email = x.EmailId,
                    CreatedDate = x.CreateDate,
                    SentDate = (DateTime)x.SentDate,
                    sent = x.IsEmailSent,
                    sentTries = x.SentTries,
                    confirmationNumber = x.ConfirmationNumber

                }, tableData, CurrPage, 5, x => x.SentDate, true);

                foreach (TableModelLogs requiredData in data)
                {
                    logTable.Add(requiredData);
                }
                if (CurrPage == 0) { CurrPage = 1; }
                int dataSize = 5;
                int totalCount = _emailLogsRepository.GetTotalCount(tableData);
                int totalPage = (int)Math.Ceiling((double)totalCount / dataSize);
                int FirstItemIndex = Math.Min((CurrPage - 1) * dataSize + 1, totalCount);
                int LastItemIndex = Math.Min(CurrPage * dataSize, totalCount);


                return new LogsViewModel
                {
                    paging = logTable,
                    TotalCount = totalCount,
                    TotalPages = totalPage,
                    CurrentPage = CurrPage,
                    PageSize = 3,
                    FirstItemIndex = FirstItemIndex,
                    LastItemIndex = LastItemIndex,
                    type = 1,
                };
            }
            else
            {
                Expression<Func<Smslog, bool>> tableData = PredicateBuilder.New<Smslog>();

                tableData = x => true;

                if (!string.IsNullOrWhiteSpace(ReciverName))
                {
                    tableData = tableData.And(e => e.Request.RequestClients.First(u => u.RequestId == e.RequestId).FirstName.ToLower().Contains(ReciverName.ToLower()) ||
                     e.Request.RequestClients.First(u => u.RequestId == e.RequestId).LastName.ToLower().Contains(ReciverName.ToLower()));
                }
                if (RoleID != 0 && RoleID != null)
                {
                    tableData = tableData.And(e => e.RoleId == RoleID);
                }
                if (!string.IsNullOrWhiteSpace(phoneNo))
                {
                    tableData = tableData.And(e => e.MobileNumber.ToLower().Contains(phoneNo.ToLower()));
                }
                if (createdDate.HasValue)
                {
                    tableData = tableData.And(e => e.CreateDate.Date == createdDate.Value.Date);
                }

                if (sentDate.HasValue)
                {
                    tableData = tableData.And(e => e.SentDate == sentDate.Value.Date);
                }
                List<TableModelLogs> logTable = new List<TableModelLogs>();

                var data = _smmsLogRepository.GetAllWithPagination(x => new TableModelLogs
                {
                    SMSLogId = (int)x.SmslogId,
                    reciverName = x.Request.FirstName + " " + x.Request.LastName,
                    action = x.Smstemplate,
                    roleName = x.Role.Name,
                    PhoneNumber = x.MobileNumber,
                    CreatedDate = x.CreateDate,
                    SentDate = (DateTime)x.SentDate,
                    sent = x.IsSmssent,
                    sentTries = x.SentTries,
                    confirmationNumber = x.ConfirmationNumber

                }, tableData, CurrPage, 5, x => x.SentDate, false);

                foreach (TableModelLogs requiredData in data)
                {
                    logTable.Add(requiredData);
                }
                if (CurrPage == 0) { CurrPage = 1; }
                int dataSize = 5;
                int totalCount = _smmsLogRepository.GetTotalCount(tableData);
                int totalPage = (int)Math.Ceiling((double)totalCount / dataSize);
                int FirstItemIndex = Math.Min((CurrPage - 1) * dataSize + 1, totalCount);
                int LastItemIndex = Math.Min(CurrPage * dataSize, totalCount);


                return new LogsViewModel
                {
                    paging = logTable,
                    TotalCount = totalCount,
                    TotalPages = totalPage,
                    CurrentPage = CurrPage,
                    PageSize = 3,
                    FirstItemIndex = FirstItemIndex,
                    LastItemIndex = LastItemIndex,
                    type = 2,
                };

            }
        }
        #endregion

        #region Patient History
        public PatientHistoryViewModel PatientHistory(string FirstName, string LastName, string Email, string PhoneNumber, int CurrentPage = 1)
        {
            Expression<Func<User, bool>> tableData = PredicateBuilder.New<User>();

            tableData = x => true;

            if (!FirstName.IsNullOrEmpty())
            {
                tableData = tableData.And(e => e.FirstName.ToLower().Contains(FirstName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(LastName))
            {
                tableData = tableData.And(e => e.LastName.ToLower().Contains(LastName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(Email))
            {
                tableData = tableData.And(e => e.Email.ToLower().Contains(Email.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(PhoneNumber))
            {
                tableData = tableData.And(e => e.Mobile.Contains(PhoneNumber));
            }

            List<TableModel> data = new List<TableModel>();
            var temp = _userRepository.GetAllWithPagination(x => new TableModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Address = x.Street + "," + x.City + "," + x.State,
                email = x.Email,
                phone = x.Mobile,
                userID = x.UserId,

            }, tableData, CurrentPage, 5, x => x.CreatedDate, true); ;

            foreach (TableModel requestClient in temp)
            {
                data.Add(requestClient);
            }

            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = _userRepository.GetTotalCount(tableData);
            int totalPage = (int)Math.Ceiling((double)totalCount / dataSize);
            int FirstItemIndex = Math.Min((CurrentPage - 1) * dataSize + 1, totalCount);
            int LastItemIndex = Math.Min(CurrentPage * dataSize, totalCount);


            return new PatientHistoryViewModel
            {
                PagingData = data,
                TotalCount = totalCount,
                TotalPages = totalPage,
                CurrentPage = CurrentPage,
                PageSize = 3,
                FirstItemIndex = FirstItemIndex,
                LastItemIndex = LastItemIndex,
            };

        }
        #endregion

        #region patient Records
        public PatientRecordViewModel PatientRecordTable(int userID, int CurrentPage)
        {
            Expression<Func<RequestClient, bool>> tableData = PredicateBuilder.New<RequestClient>();

            tableData = x => x.Request.UserId == userID;
            List<RecordTableModel> data = new List<RecordTableModel>();
            var temp = _requestclientRepository.GetAllWithPagination(x => new RecordTableModel
            {

                clientName = x.FirstName + " " + x.LastName,
                ConcludedDate = x.Request.CreatedDate,
                CreatedDate = x.Request.CreatedDate,
                confirmationNumber = x.Request.ConfirmationNumber,
                ProviderName = x.Request.Physician.FirstName + " " + x.Request.Physician.LastName,
                requestclientID = x.RequestClientId,
                RequestId = x.RequestId,
                status = x.Request.StatusNavigation.Statusname,
                userID = (int)x.Request.UserId,
            }, tableData, CurrentPage, 5, x => x.FirstName, false);

            foreach (RecordTableModel requestClient in temp)
            {
                data.Add(requestClient);
            }

            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = _requestclientRepository.GetTotalCount(tableData);
            int totalPage = (int)Math.Ceiling((double)totalCount / dataSize);
            int FirstItemIndex = Math.Min((CurrentPage - 1) * dataSize + 1, totalCount);
            int LastItemIndex = Math.Min(CurrentPage * dataSize, totalCount);

            return new PatientRecordViewModel
            {
                PagingData = data,
                TotalCount = totalCount,
                TotalPages = totalPage,
                CurrentPage = CurrentPage,
                PageSize = 3,
                FirstItemIndex = FirstItemIndex,
                LastItemIndex = LastItemIndex,

            };
        }
        #endregion

        #region Block History
        public BlockHistoryViewModel BlockHistoryTable(string Name, DateTime? Date, string Email, string PhoneNumber, int CurrentPage)

        {
            Expression<Func<BlockRequest, bool>> tableData = PredicateBuilder.New<BlockRequest>();

            tableData = x => true;
            if (!Name.IsNullOrEmpty())
            {
                tableData = tableData.And(e => e.Request.FirstName.ToLower().Contains(Name.ToLower()) || e.Request.LastName.ToLower().Contains(Name.ToLower()));
            }
            if (Date != null)
            {
                tableData = tableData.And(e => e.CreatedDate.Value.Date == Date.Value.Date);
            }
            if (!string.IsNullOrWhiteSpace(Email))
            {
                tableData = tableData.And(e => e.Email.ToLower().Contains(Email.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(PhoneNumber))
            {
                tableData = tableData.And(e => e.PhoneNumber.Contains(PhoneNumber));
            }
            List<BlockTable> data = new List<BlockTable>();
            var requiredData = _blockRequestRepository.GetAllWithPagination(x => new BlockTable
            {
                BlockId = x.BlockRequestId,
                CreatedDate = x.CreatedDate,
                Email = x.Email,
                IsActive = x.IsActive == null ? "Yes" : "No",
                PhoneNumber = x.PhoneNumber,
                Note = x.Reason,
                PatientName = x.Request.RequestClients.First(u => u.RequestId == x.RequestId).FirstName + " " + x.Request.RequestClients.First(u => u.RequestId == x.RequestId).LastName,
            }, tableData, CurrentPage, 5, x => x.RequestId, false);

            foreach (BlockTable requestClient in requiredData)
            {
                data.Add(requestClient);
            }

            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = _blockRequestRepository.GetTotalCount(tableData);
            int totalPage = (int)Math.Ceiling((double)totalCount / dataSize);
            int FirstItemIndex = Math.Min((CurrentPage - 1) * dataSize + 1, totalCount);
            int LastItemIndex = Math.Min(CurrentPage * dataSize, totalCount);

            return new BlockHistoryViewModel
            {
                pagingData = data,
                TotalCount = totalCount,
                TotalPages = totalPage,
                CurrentPage = CurrentPage,
                PageSize = 3,
                FirstItemIndex = FirstItemIndex,
                LastItemIndex = LastItemIndex,

            };
        }

        public void UnblockRequest(int id)
        {
            BlockRequest obj = _blockRequestRepository.GetById(id);
            Request req = _requestRepository.GetById(obj.RequestId);
            req.Status = 1;
            _requestRepository.UpdateAsync(req);
            _blockRequestRepository.Remove(obj);

        }
        #endregion

        #region Search Record
        public SearchRecordViewModel SearchRecordTable(int statusOfRequest, string Name, int requestType, DateTime? DateOfService, DateTime? ToDateOfService, string physician, string Email, string PhoneNumber, int CurrentPage = 1)
        {
            Expression<Func<RequestClient, bool>> tableData = PredicateBuilder.New<RequestClient>();

            tableData = x => x.Request.IsDeleted == null && x.Request.UserId != null;

            if (statusOfRequest != 0)
            {
                tableData = tableData.And(e => e.Request.Status == statusOfRequest);
            }
            if (!string.IsNullOrWhiteSpace(Name))
            {
                tableData = tableData.And(e => e.FirstName.ToLower().Contains(Name.ToLower()) || e.LastName.ToLower().Contains(Name.ToLower()));
            }
            if (requestType != 0)
            {
                tableData = tableData.And(e => e.Request.RequestTypeId == requestType);
            }
            if (DateOfService.HasValue)
            {
                tableData = tableData.And(e => e.Request.CreatedDate.Date >= DateOfService.Value.Date);
            }
            if (ToDateOfService.HasValue)
            {
                tableData = tableData.And(e => e.Request.CreatedDate.Date >= ToDateOfService.Value.Date);
            }
            if (!string.IsNullOrWhiteSpace(physician))
            {
                tableData = tableData.And(e => e.Request.Physician.FirstName.ToLower().Contains(physician.ToLower()) || e.Request.Physician.LastName.ToLower().Contains(physician.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(Email))
            {
                tableData = tableData.And(e => e.Email.ToLower().Contains(Email.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(PhoneNumber))
            {
                tableData = tableData.And(e => e.PhoneNumber.Contains(PhoneNumber));
            }

            List<SearchRecordTable> data = new List<SearchRecordTable>();
            var temp = _requestclientRepository.GetAllWithPagination(x => new SearchRecordTable
            {
                address = x.Street + "," + x.City + "," + x.State + "," + x.ZipCode,
                CancelByProviderNote = "-",
                CloseCaseDate = DateTime.Now,
                DateOfService = x.Request.CreatedDate,
                AdminNote = x.Request.RequestNotes.First(u => u.RequestId == x.RequestId).AdminNotes,
                PatientName = x.FirstName + " " + x.LastName,
                Requestor = x.Request.FirstName + " " + x.Request.LastName,
                email = x.Email,
                phone = x.PhoneNumber,
                physician = x.Request.Physician.FirstName + " " + x.Request.Physician.LastName,
                RequestClientId = x.RequestClientId,
                requestStatus = x.Request.StatusNavigation.Statusname,
                zip = x.ZipCode,
                PatientNote = x.Notes,
                PhysicianNote = x.Request.RequestNotes.First(u => u.RequestId == x.RequestId).PhysicianNotes,

            }, tableData, CurrentPage, 5, x => x.FirstName, true);

            foreach (SearchRecordTable requestClient in temp)
            {
                data.Add(requestClient);
            }

            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = _requestclientRepository.GetTotalCount(tableData);
            int totalPage = (int)Math.Ceiling((double)totalCount / dataSize);
            int FirstItemIndex = Math.Min((CurrentPage - 1) * dataSize + 1, totalCount);
            int LastItemIndex = Math.Min(CurrentPage * dataSize, totalCount);


            return new SearchRecordViewModel
            {
                PagingData = data,
                TotalCount = totalCount,
                TotalPages = totalPage,
                CurrentPage = CurrentPage,
                PageSize = 3,
                FirstItemIndex = FirstItemIndex,
                LastItemIndex = LastItemIndex,
            };
        }

        public void DeleteRequest(int id)
        {
            RequestClient client = _requestclientRepository.GetById(id);
            Request req = _requestRepository.GetById(client.RequestId);
            req.IsDeleted = new BitArray(new bool[] { true });
            _requestRepository.UpdateAsync(req);

        }

        #endregion

        #region Create Request By Admin
        public async Task CreateRequestByAdmin(PatientRequestViewModel viewModel, int adminId)
        {
            var data = _adminRepository.GetById(adminId);
            Request request = new Request
            {
                RequestTypeId = 1,
                FirstName = data.FirstName,
                LastName = data.LastName,
                PhoneNumber = data.Mobile,
                Email = data.Email,
                CreatedDate = DateTime.Now,
                Status = 1,

            };
            await _requestRepository.AddAsync(request);

            RequestClient requestClient = new RequestClient
            {
                RequestId = request.RequestId,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                PhoneNumber = viewModel.PhoneNumber,
                RegionId = viewModel.RegionId,
                Street = viewModel.Street,
                City = viewModel.City,
                State = await _regionRepository.FindState(viewModel.RegionId),
                ZipCode = viewModel.ZipCode,
                Notes = viewModel.Note,
                Email = viewModel.Email,
                IntDate = viewModel.DateOfBirth.Day,
                IntYear = viewModel.DateOfBirth.Year,
                StrMonth = viewModel.DateOfBirth.Month.ToString(),
                Address = viewModel.Street + "," + viewModel.City + "," + viewModel.ZipCode,


            };
            await _requestclientRepository.AddAsync(requestClient);

            User user = await _userRepository.CheckUserByEmail(viewModel.Email);
            if (user != null)
            {
                request.UserId = user.UserId;
                await _requestRepository.UpdateAsync(request);

            }

            AspNetUser userExist = await _aspnetuserRepository.CheckUserByEmail(viewModel.Email);
            if (userExist == null)
            {
                AspNetUser newaspNetUSer = new AspNetUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = viewModel.FirstName,
                    PhoneNumber = viewModel.PhoneNumber,
                    Email = viewModel.Email,
                    CreatedDate = DateTime.Now
                };

                await _aspnetuserRepository.AddAsync(newaspNetUSer);
                AspNetUserRole userRole = new AspNetUserRole
                {
                    UserId = newaspNetUSer.Id,
                    RoleId = "2"
                };
                await _userRolesRepository.AddAsync(userRole);
            }
        }
        #endregion

        #region Schaduling
        public SchedulingModel Scheduling(int region)
        {
            SchedulingModel schedulingModel = new SchedulingModel();

            List<Resources> resources = new List<Resources>();
            List<Events> events = new List<Events>();

            Expression<Func<Physician, bool>> tableData = PredicateBuilder.New<Physician>();
            Expression<Func<ShiftDetail, bool>> ShiftDetailstableData = PredicateBuilder.New<ShiftDetail>();
            tableData = x => x.IsDeleted == null;
            ShiftDetailstableData = x => x.IsDeleted == null;

            if (region != 0)
            {
                tableData = tableData.And(x => x.RegionId == region);
                ShiftDetailstableData = ShiftDetailstableData.And(x => x.RegionId == region);

            }
            var temp = _physicianRepository.GetAllData(x => new Resources
            {
                id = x.PhysicianId.ToString(),
                title = x.FirstName + " " + x.LastName,
                avtar = "null"
            }, tableData);
            foreach (Resources resource in temp)
            {
                resources.Add(resource);
            }
            temp = _shiftDetailsRepository.GetAllData(x => new Events
            {
                id = x.ShiftDetailId.ToString(),
                resourceId = x.Shift.PhysicianId.ToString(),
                start = Convert.ToDateTime(x.ShiftDate.Year + "/" + x.ShiftDate.Month + "/" + x.ShiftDate.Day).AddHours(x.StartTime.Hour).AddMinutes(x.StartTime.Minute).ToUniversalTime().ToString("O"),
                end = Convert.ToDateTime(x.ShiftDate.Year + "/" + x.ShiftDate.Month + "/" + x.ShiftDate.Day).AddHours(x.EndTime.Hour).AddMinutes(x.EndTime.Minute).ToUniversalTime().ToString("O"),
                title = x.StartTime.ToString() + "-" + x.EndTime + "\n" + x.Shift.Physician.FirstName,
                color = x.Status == 1 ? "lightgreen" : "pink",



            }, ShiftDetailstableData);
            foreach (Events e in temp)
            {
                events.Add(e);
            }
            schedulingModel.events = events;
            schedulingModel.resources = resources;
            return schedulingModel;
        }
        #endregion

        #region Create Shift
        public void AddShift(CreateShiftViewModel model, List<DayOfWeek> WeekDays, string adminId)
        {
            Shift shift = new()
            {
                PhysicianId = model.physicianID,
                StartDate = model.ShiftDate,
                IsRepeat = new BitArray(1, model.toggle),
                WeekDays = WeekDays.ToString(),
                RepeatUpto = model.repeatEnd,
                CreatedBy = adminId.ToString(),
                CreatedDate = DateTime.Now,
            };
            _shiftRepository.AddAsync(shift);

            ShiftDetail shiftDetail = new()
            {
                ShiftId = shift.ShiftId,
                ShiftDate = shift.StartDate,
                StartTime = model.startTime,
                EndTime = model.endTime,
                RegionId = model.regionId,
            };
            _shiftDetailsRepository.AddAsync(shiftDetail);


            for (int i = 0; i < model.repeatEnd; i++)
            {
                DateOnly nextDay = model.ShiftDate.AddDays(7 * i);
                foreach (DayOfWeek day in WeekDays)
                {
                    ShiftDetail nextShift = new ShiftDetail
                    {
                        ShiftId = shift.ShiftId,
                        ShiftDate = day - nextDay.DayOfWeek > 0 ? nextDay.AddDays(day - nextDay.DayOfWeek) : nextDay.AddDays(7 + day - nextDay.DayOfWeek),
                        StartTime = model.startTime,
                        EndTime = model.endTime,
                        RegionId = model.regionId,
                    };
                    _shiftDetailsRepository.AddAsync(nextShift);
                }
            }
        }
        #endregion

        #region Edit Shift

        public CreateShiftViewModel GetShiftDetailsById(int shiftDetailsId)
        {
            Expression<Func<ShiftDetail, bool>> tableData = PredicateBuilder.New<ShiftDetail>();
            ShiftDetail shiftDetail = _shiftDetailsRepository.IncludeEntity(x => x.ShiftDetailId == shiftDetailsId, x => x.Shift);

            CreateShiftViewModel model = new CreateShiftViewModel();
            model.ShiftDetailId = shiftDetailsId;
            model.physicianID = shiftDetail.Shift.PhysicianId;
            model.regionId = (int)shiftDetail.RegionId;
            model.ShiftDate = shiftDetail.ShiftDate;
            model.startTime = shiftDetail.StartTime;
            model.endTime = shiftDetail.EndTime;

            return model;
        }
        public void EditShiftData(CreateShiftViewModel viewModel)
        {
            ShiftDetail shiftDetail = _shiftDetailsRepository.GetById(viewModel.ShiftDetailId);
            shiftDetail.StartTime = viewModel.startTime;
            shiftDetail.EndTime = viewModel.endTime;
            shiftDetail.ShiftDate = viewModel.ShiftDate;
            _shiftDetailsRepository.UpdateAsync(shiftDetail);


        }
        public object returnShift(int id)
        {
            ShiftDetail data = _shiftDetailsRepository.GetById(id);
            if (data.Status == 0)
            {
                data.Status = 1;
            }
            else
            {
                data.Status = 0;
            }
            _shiftDetailsRepository.UpdateAsync(data);
            return data;
        }
        public void deleteShift(int id)
        {
            ShiftDetail data = _shiftDetailsRepository.GetById(id);
            data.IsDeleted = new BitArray(1);
            _shiftDetailsRepository.UpdateAsync(data);
        }
        #endregion

        #region Requested Shift
        public RequestedShiftViewModel RequestedShift(int month, int region, int CurrentPage)
        {
            Expression<Func<ShiftDetail, bool>> table = PredicateBuilder.New<ShiftDetail>();

            table = x => x.IsDeleted == null && x.Status == 0 && x.ShiftDate >= DateOnly.FromDateTime(DateTime.Now);

            if (region != 0)
            {
                table = table.And(u => u.RegionId == region);
            }
            if (month != 0)
            {
                table = table.And(u => u.ShiftDate.Month == month);
            }
            List<shiftTable> shiftTable = new List<shiftTable>();

            var data = _shiftDetailsRepository.GetAllWithPagination(x => new shiftTable
            {
                day = x.ShiftDate.ToString(),
                shiftdetailId = x.ShiftDetailId,
                staff = x.Shift.Physician.FirstName + " " + x.Shift.Physician.LastName,
                time = x.StartTime + "-" + x.EndTime,
                region = x.Region.Name,

            }, table, CurrentPage, 5, x => x.ShiftDate, false);

            foreach (shiftTable requiredData in data)
            {
                shiftTable.Add(requiredData);
            }

            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = _shiftDetailsRepository.GetTotalCount(table);
            int totalPage = (int)Math.Ceiling((double)totalCount / dataSize);
            int FirstItemIndex = Math.Min((CurrentPage - 1) * dataSize + 1, totalCount);
            int LastItemIndex = Math.Min(CurrentPage * dataSize, totalCount);

            return new RequestedShiftViewModel
            {
                PagingData = shiftTable,
                TotalCount = totalCount,
                TotalPages = totalPage,
                CurrentPage = CurrentPage,
                PageSize = 3,
                FirstItemIndex = FirstItemIndex,
                LastItemIndex = LastItemIndex,
            };
        }
        public void ApproveShift(List<int> selectedShift)
        {
            foreach (var shift in selectedShift)
            {
                returnShift(shift);
            }

        }
        public void deleteshifts(List<int> selectedShift)
        {
            foreach (var shift in selectedShift)
            {
                deleteShift(shift);
            }
        }
        #endregion

        #region Physician Location
        public List<PhysicianLocation> ProviderLocation()
        {
            return _physicianLocationRepository.GetAll().ToList();
        }
        #endregion

        #region MD on Call
        public ProvidersOnCallViewModel? GetProvidersOnCall(int regionId)
        {
            Expression<Func<ShiftDetail, bool>> tableData = PredicateBuilder.New<ShiftDetail>();
            Expression<Func<Physician, bool>> PhysiciantableData = PredicateBuilder.New<Physician>();
            PhysiciantableData = x => x.IsDeleted == null;
            if (regionId != 0)
            {
                PhysiciantableData = PhysiciantableData.And(x => x.RegionId == regionId);
            }
            tableData = x => x.IsDeleted == null && x.Status == 1;
            tableData = tableData.And(x => x.ShiftDate == DateOnly.FromDateTime(DateTime.Now));
            tableData = tableData.And(x => x.StartTime < TimeOnly.FromDateTime(DateTime.Now));
            tableData = tableData.And(x => x.EndTime > TimeOnly.FromDateTime(DateTime.Now));

            List<PhysicianOnCallModal> physicians = new List<PhysicianOnCallModal>();
            List<int> physicianOnCall = new List<int>();
            var data = _shiftDetailsRepository.GetAllData(x => x.Shift.PhysicianId, tableData);
            foreach (int item in data)
            {
                physicianOnCall.Add(item);
            }

            ProvidersOnCallViewModel providersOnCallViewModel = new ProvidersOnCallViewModel();
            providersOnCallViewModel.regions = getstateDropdown();

            var temp = _physicianRepository.GetAllData(x => new PhysicianOnCallModal
            {
                physicianId = x.PhysicianId,
                physicianName = x.FirstName + " " + x.LastName,
                Photo = Path.Combine("/uploads/physician/image", $"{x.PhysicianId}_Photo.jpg"),
            }, PhysiciantableData);
            foreach (PhysicianOnCallModal physician in temp)
            {
                if (physicianOnCall.Contains(physician.physicianId))
                {
                    physician.IsOnCall = true;

                };
                physicians.Add(physician);
            }
            providersOnCallViewModel.PaggingData = physicians;
            return providersOnCallViewModel;
        }

        #endregion


    }
}
