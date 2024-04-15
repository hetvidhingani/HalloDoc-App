﻿using HalloDoc.Entities.DataModels;
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
        private readonly IPhysicianLocationRepository _physicianLocationRepository;
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
                               IShiftRepository shiftRepository, IPhysicianLocationRepository physicianLocationRepository)
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
        }
        #endregion

        #region common methods


        public async Task<Admin> GetAdmin(string email)
        {
            Admin user = await _adminRepository.CheckUserByEmail(email);
            return user;
        }
        public async Task<int> GetCount(int statusId)
        {
            return await _requestRepository.GetCountAsync(r => r.Status == statusId && r.UserId != null && r.IsDeleted == null);
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

        public List<Region> getstateDropdown()
        {
            return _regionRepository.GetAll().ToList();
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
            Expression<Func<ShiftDetail, bool>> whereClauseSyntax = PredicateBuilder.New<ShiftDetail>();

            whereClauseSyntax = x => x.IsDeleted == null && x.Status == 0;
            whereClauseSyntax = whereClauseSyntax.And(x => x.ShiftDate == DateOnly.FromDateTime(DateTime.Now));
            whereClauseSyntax = whereClauseSyntax.And(x => x.StartTime >= TimeOnly.FromDateTime(DateTime.Now));
            whereClauseSyntax = whereClauseSyntax.And(x => x.EndTime >= TimeOnly.FromDateTime(DateTime.Now));

            List<string> emailList = new List<string>();
            var data = _shiftDetailsRepository.GetAllData(x => x.Shift.Physician.Email, whereClauseSyntax);

            if (data.Count > 0)
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
                foreach (string item in data)
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
                Status = (short)req.Status,
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
            request.Status = 7;
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
            List<AdminRegion> adminRegion = _adminRegionRepository.GetAll().Where(u => u.AdminId == adminId).ToList();
            List<Region> regions = _regionRepository.GetAll().ToList();

            // Create the view model
            AdminMyProfileViewModel model = new AdminMyProfileViewModel()
            {
                AdminID = admin.AdminId,
                UserName = aspNetUser.UserName,
                Password = _aspnetuserRepository.DecodeFrom64(aspNetUser.PasswordHash),
                Status = (int)admin.Status,
                RoleID = admin.RoleId,
                roles = _roleRepository.GetAll().Where(u => u.AccountType == 1).ToList(),

                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                ConfirmEmail = admin.Email,
                PhoneNumber = admin.Mobile,
                Address1 = admin.Address1,
                Address2 = admin.Address2,
                City = admin.City,
                RegionId = admin.RegionId,
                State = regions,
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

        public async Task<object> SaveAdminInfo(AdminMyProfileViewModel model, List<int> ids)
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
            model.Role = _roleRepository.GetAll().Where(u => u.AccountType == 2).ToList();
            model.status = _statusRepository.GetAll().ToList();

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

            Physician physician = new Physician
            {
                Id = user.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                MedicalLicense = model.MedicalLicense,
                Mobile = model.PhoneNumber,
                AdminNotes = model.AdminNotes,
                Address1 = model.Address1,
                Address2 = model.Address2,
                Zip = model.Zip,
                City = model.City,
                RegionId = model.RegionId,
                RoleId = model.RoleId,
                CreatedDate = DateTime.Now,
                Status = 3,
                CreatedBy = user.Id,
                BusinessName = model.BusinessName,
                BusinessWebsite = model.BusinessWebsite,
                Photo = model.Photo.FileName,
            };

            await _physicianRepository.AddAsync(physician);
            if (model.Photo.FileName != null)
            {

                var fileName = $"{physician.PhysicianId}_Photo.jpg";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads\physician\image", fileName);
                using var stream = System.IO.File.Create(filePath);
                model.Photo.CopyTo(stream);
            }
            if (model.contractoragreement != null)
            {

                var fileName1 = $"{physician.PhysicianId}_contractoragreement.pdf";
                var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads\physician\doc", fileName1);
                using var stream1 = System.IO.File.Create(filePath1);
                model.Photo.CopyTo(stream1);
                physician.IsAgreementDoc = new BitArray(new bool[] { true });
            }
            if (model.backgroundcheck != null)
            {

                var fileName2 = $"{physician.PhysicianId}_backgroundcheck.pdf";
                var filePath2 = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads\physician\doc", fileName2);
                using var stream2 = System.IO.File.Create(filePath2);
                model.Photo.CopyTo(stream2);
                physician.IsBackgroundDoc = new BitArray(new bool[] { true });
            }

            if (model.hippa != null)
            {

                var fileName3 = $"{physician.PhysicianId}_hippa.pdf";
                var filePath3 = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads\physician\doc", fileName3);
                using var stream3 = System.IO.File.Create(filePath3);
                model.Photo.CopyTo(stream3);
                physician.IsTrainingDoc = new BitArray(new bool[] { true });
            }

            if (model.nondisclosure != null)
            {

                var fileName4 = $"{physician.PhysicianId}_nonDisclosure.pdf";
                var filePath4 = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads\physician\doc", fileName4);
                using var stream4 = System.IO.File.Create(filePath4);
                model.Photo.CopyTo(stream4);
                physician.IsNonDisclosureDoc = new BitArray(new bool[] { true });
            }

            await _physicianRepository.UpdateAsync(physician);

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
                PasswordHash = model.Password,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CreatedDate = DateTime.Now,
            };
            await _aspnetuserRepository.AddAsync(user);
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
            Physician phy = await _physicianRepository.GetByIdAsync(physicianID);
            AspNetUser asp = await _aspnetuserRepository.getById(phy.Id);
            ProviderViewModel model = new ProviderViewModel();
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
            model.UserName = asp.UserName;
            model.Password = asp.PasswordHash;
            model.RoleId = (int)phy.RoleId;
            model.Role = _roleRepository.GetAll().Where(u => u.AccountType == 2).ToList();
            model.statusId = (int)phy.Status;
            model.status = _statusRepository.GetAll().ToList();
            model.IsAgreementDoc = phy.IsAgreementDoc;
            model.isbackgroundcheck = phy.IsBackgroundDoc;

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

            Expression<Func<Physician, bool>> whereClauseSyntax = PredicateBuilder.New<Physician>();

            whereClauseSyntax = x => true;
            List<TableProviderInfo> AccountAccessTable = new List<TableProviderInfo>();
            if (RegionId != 0)
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.RegionId == RegionId);
            }
            var data = _physicianRepository.GetAllWithPagination(x => new TableProviderInfo
            {
                onCallStatus = "Un Available",
                PhysicianID = x.PhysicianId,
                ProviderName = x.FirstName + " " + x.LastName,
                stopNotification = x.PhysicianNotifications.First(u => u.PhysicianId == x.PhysicianId) == null ? null : "1",
                roleName = x.Role.Name,
                status = x.StatusNavigation.Statusname,

            }, whereClauseSyntax.And(x => x.IsDeleted == null), CurrentPage, 5, x => x.FirstName, true);

            foreach (TableProviderInfo requiredData in data)
            {
                AccountAccessTable.Add(requiredData);
            }

            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = _physicianRepository.GetTotalCount(whereClauseSyntax.And(x => x.IsDeleted == null));
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
            ContactProviderViewModel viewModel = new ContactProviderViewModel();
            viewModel.physicianId = id;

            return viewModel;
        }
        public Physician ContectProvider(int id)
        {
            return _physicianRepository.GetById(id);

        }

        public void StopNotificationPhysician(List<int> ids)
        {
            var existingPhysicians = _physicianNotificationRepository.GetAll();

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
        public Physician savePhysicianInformation(ProviderViewModel model, int id)
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
            if (model.Signature != null || model.File != null)
            {

                physician.Signature = model.Signature.FileName;
                physician.Photo = model.File.FileName;
            }

            _physicianRepository.UpdateAsync(physician);

            return physician;
        }
        public object documentsProvider(ProviderViewModel model)
        {
            Physician physician = _physicianRepository.GetById(model.PhysicianId);
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

                physician.IsAgreementDoc = new BitArray(new bool[] { true });
            }
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



            _physicianRepository.UpdateAsync(physician);

            return physician;
        }
        #endregion

        #region Account Access

        public async Task<object> AccountAccessTable(int CurrentPage)
        {

            Expression<Func<Role, bool>> whereClauseSyntax = PredicateBuilder.New<Role>();

            whereClauseSyntax = x => true;
            List<TableAccountAccess> AccountAccessTable = new List<TableAccountAccess>();

            var data = _roleRepository.GetAllWithPagination(x => new TableAccountAccess
            {
                roleID = x.RoleId,
                AccountType = x.AccountType == 1 ? "Admin" : "Physician",
                Name = x.Name,

            }, whereClauseSyntax.And(x => x.IsDeleted == null), CurrentPage, 5, x => x.Name, true);

            foreach (TableAccountAccess requiredData in data)
            {
                AccountAccessTable.Add(requiredData);
            }

            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = _roleRepository.GetTotalCount(whereClauseSyntax.And(x => x.IsDeleted == null));
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
        public async Task<object> CreateAccess(AccountAccessViewModel viewModel)
        {
            List<Role> roles = _roleRepository.GetAll().ToList();
            var role = new Role()
            {
                Name = viewModel.RoleName,
                AccountType = (short)viewModel.accountTypeId,
                CreatedDate = DateTime.Now,
                CreatedBy = "bda27f31-02b1-442f-9120-bed8f09a4966",
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
                await _roleMenuRepository.Remove(rolemenu);
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
            Expression<Func<Admin, bool>> whereClauseSyntax = PredicateBuilder.New<Admin>();

            whereClauseSyntax = x => true;
            Expression<Func<Physician, bool>> whereClauseSyntax2 = PredicateBuilder.New<Physician>();

            whereClauseSyntax2 = x => true;

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
            Expression<Func<HealthProfessional, bool>> whereClauseSyntax = PredicateBuilder.New<HealthProfessional>();

            whereClauseSyntax = x => true;
            if (!string.IsNullOrWhiteSpace(VendorName))
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.VendorName.ToLower().Contains(VendorName.ToLower()));
            }
            if (VendorProfessionTypeId != 0)
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.Profession == VendorProfessionTypeId);
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
            }, whereClauseSyntax.And(x => x.IsDeleted == null), CurrentPage, 5, x => x.VendorName, true);

            foreach (TableModelVendor requiredData in data)
            {
                vendorTable.Add(requiredData);
            }

            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = _healthProfessionalsRepository.GetTotalCount(whereClauseSyntax.And(x => x.IsDeleted == null));
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
                Expression<Func<EmailLog, bool>> whereClauseSyntax = PredicateBuilder.New<EmailLog>();

                whereClauseSyntax = x => true;

                if (!string.IsNullOrWhiteSpace(ReciverName))
                {
                    whereClauseSyntax = whereClauseSyntax.And(e => e.Request.FirstName.ToLower().Contains(ReciverName.ToLower()) || e.Request.LastName.ToLower().Contains(ReciverName.ToLower()));
                }
                if (RoleID != 0 && RoleID != null)
                {
                    whereClauseSyntax = whereClauseSyntax.And(e => e.RoleId == RoleID);
                }
                if (!string.IsNullOrWhiteSpace(email))
                {
                    whereClauseSyntax = whereClauseSyntax.And(e => e.EmailId.ToLower().Contains(email.ToLower()));
                }
                if (createdDate.HasValue)
                {
                    whereClauseSyntax = whereClauseSyntax.And(e => e.CreateDate.Date == createdDate.Value.Date);
                }

                if (sentDate.HasValue)
                {
                    whereClauseSyntax = whereClauseSyntax.And(e => e.SentDate == sentDate.Value.Date);
                }
                List<TableModelLogs> logTable = new List<TableModelLogs>();

                var data = _emailLogsRepository.GetAllWithPagination(x => new TableModelLogs
                {
                    EmailLogId = x.EmailLogId,
                    reciverName = x.Request.FirstName + " " + x.Request.LastName,
                    action = x.SubjectName,
                    roleName = x.Role.Name,
                    email = x.EmailId,
                    CreatedDate = x.CreateDate,
                    SentDate = x.SentDate,
                    sent = x.IsEmailSent,
                    sentTries = x.SentTries,
                    confirmationNumber = x.ConfirmationNumber

                }, whereClauseSyntax, CurrPage, 5, x => x.SentDate, false);

                foreach (TableModelLogs requiredData in data)
                {
                    logTable.Add(requiredData);
                }
                if (CurrPage == 0) { CurrPage = 1; }
                int dataSize = 5;
                int totalCount = _emailLogsRepository.GetTotalCount(whereClauseSyntax);
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
                Expression<Func<Smslog, bool>> whereClauseSyntax = PredicateBuilder.New<Smslog>();

                whereClauseSyntax = x => true;

                if (!string.IsNullOrWhiteSpace(ReciverName))
                {
                    whereClauseSyntax = whereClauseSyntax.And(e => e.Request.FirstName.ToLower().Contains(ReciverName.ToLower()) || e.Request.LastName.ToLower().Contains(ReciverName.ToLower()));
                }
                if (RoleID != 0 && RoleID != null)
                {
                    whereClauseSyntax = whereClauseSyntax.And(e => e.RoleId == RoleID);
                }
                if (!string.IsNullOrWhiteSpace(phoneNo))
                {
                    whereClauseSyntax = whereClauseSyntax.And(e => e.MobileNumber.ToLower().Contains(phoneNo.ToLower()));
                }
                if (createdDate.HasValue)
                {
                    whereClauseSyntax = whereClauseSyntax.And(e => e.CreateDate.Date == createdDate.Value.Date);
                }

                if (sentDate.HasValue)
                {
                    whereClauseSyntax = whereClauseSyntax.And(e => e.SentDate == sentDate.Value.Date);
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
                    SentDate = x.SentDate,
                    sent = x.IsSmssent,
                    sentTries = x.SentTries,
                    confirmationNumber = x.ConfirmationNumber

                }, whereClauseSyntax, CurrPage, 5, x => x.SentDate, false);

                foreach (TableModelLogs requiredData in data)
                {
                    logTable.Add(requiredData);
                }
                if (CurrPage == 0) { CurrPage = 1; }
                int dataSize = 5;
                int totalCount = _smmsLogRepository.GetTotalCount(whereClauseSyntax);
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
            Expression<Func<User, bool>> whereClauseSyntax = PredicateBuilder.New<User>();

            whereClauseSyntax = x => true;

            if (!FirstName.IsNullOrEmpty())
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.FirstName.ToLower().Contains(FirstName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(LastName))
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.LastName.ToLower().Contains(LastName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(Email))
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.Email.ToLower().Contains(Email.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(PhoneNumber))
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.Mobile.Contains(PhoneNumber));
            }

            List<TableModel> data = new List<TableModel>();
            var temp = _userRepository.GetAllWithPagination(x => new TableModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Address = x.Street + "," + x.City + "," + x.State,
                email = x.Email,
                phone = x.Mobile,
                //RequestId = x.Requests.RequestId,
                //userID = x.Request.RequestCloseds.First(y=>y.RequestId == x.RequestId).RequestId
                userID = x.UserId,

            }, whereClauseSyntax, CurrentPage, 5, x => x.FirstName, true); ;

            foreach (TableModel requestClient in temp)
            {
                data.Add(requestClient);
            }

            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = _userRepository.GetTotalCount(whereClauseSyntax);
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
            Expression<Func<RequestClient, bool>> whereClauseSyntax = PredicateBuilder.New<RequestClient>();

            whereClauseSyntax = x => x.Request.UserId == userID;
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
            }, whereClauseSyntax, CurrentPage, 5, x => x.FirstName, false);

            foreach (RecordTableModel requestClient in temp)
            {
                data.Add(requestClient);
            }

            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = _requestclientRepository.GetTotalCount(whereClauseSyntax);
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
            Expression<Func<BlockRequest, bool>> whereClauseSyntax = PredicateBuilder.New<BlockRequest>();

            whereClauseSyntax = x => true;

            if (!Name.IsNullOrEmpty())
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.Request.FirstName.ToLower().Contains(Name.ToLower()) || e.Request.LastName.ToLower().Contains(Name.ToLower()));
            }
            if (Date != null)
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.CreatedDate.Value.Date == Date.Value.Date);
            }
            if (!string.IsNullOrWhiteSpace(Email))
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.Email.ToLower().Contains(Email.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(PhoneNumber))
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.PhoneNumber.Contains(PhoneNumber));
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
                PatientName = x.Request.FirstName + " " + x.Request.LastName,

            }, whereClauseSyntax, CurrentPage, 5, x => x.RequestId, false);

            foreach (BlockTable requestClient in requiredData)
            {
                data.Add(requestClient);
            }

            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = _blockRequestRepository.GetTotalCount(whereClauseSyntax);
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
            Expression<Func<RequestClient, bool>> whereClauseSyntax = PredicateBuilder.New<RequestClient>();

            whereClauseSyntax = x => x.Request.IsDeleted == null;

            if (statusOfRequest != 0)
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.Request.Status == statusOfRequest);
            }
            if (!string.IsNullOrWhiteSpace(Name))
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.FirstName.ToLower().Contains(Name.ToLower()) || e.LastName.ToLower().Contains(Name.ToLower()));
            }
            if (requestType != 0)
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.Request.RequestTypeId == requestType);
            }
            if (DateOfService.HasValue)
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.Request.CreatedDate.Date >= DateOfService.Value.Date);
            }
            if (ToDateOfService.HasValue)
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.Request.CreatedDate.Date >= ToDateOfService.Value.Date);
            }
            if (!string.IsNullOrWhiteSpace(physician))
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.Request.Physician.FirstName.ToLower().Contains(physician.ToLower()) || e.Request.Physician.LastName.ToLower().Contains(physician.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(Email))
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.Email.ToLower().Contains(Email.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(PhoneNumber))
            {
                whereClauseSyntax = whereClauseSyntax.And(e => e.PhoneNumber.Contains(PhoneNumber));
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

            }, whereClauseSyntax, CurrentPage, 5, x => x.FirstName, true);

            foreach (SearchRecordTable requestClient in temp)
            {
                data.Add(requestClient);
            }

            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = _requestclientRepository.GetTotalCount(whereClauseSyntax);
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
                Notes = viewModel.Symptoms,
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

            Expression<Func<Physician, bool>> whereClauseSyntax = PredicateBuilder.New<Physician>();
            Expression<Func<ShiftDetail, bool>> ShiftDetailswhereClauseSyntax = PredicateBuilder.New<ShiftDetail>();
            whereClauseSyntax = x => x.IsDeleted == null;
            ShiftDetailswhereClauseSyntax = x => x.IsDeleted == null;

            if (region != 0)
            {
                whereClauseSyntax = whereClauseSyntax.And(x => x.RegionId == region);
                ShiftDetailswhereClauseSyntax = ShiftDetailswhereClauseSyntax.And(x => x.RegionId == region);

            }
            var temp = _physicianRepository.GetAllData(x => new Resources
            {
                id = x.PhysicianId.ToString(),
                title = x.FirstName + " " + x.LastName,
                avtar = "null"
            }, whereClauseSyntax);
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



            }, ShiftDetailswhereClauseSyntax);
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
            Expression<Func<ShiftDetail, bool>> whereClauseSyntax = PredicateBuilder.New<ShiftDetail>();
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
            Expression<Func<ShiftDetail, bool>> whereClauseSyntax = PredicateBuilder.New<ShiftDetail>();
            Expression<Func<Physician, bool>> PhysicianwhereClauseSyntax = PredicateBuilder.New<Physician>();
            PhysicianwhereClauseSyntax = x => x.IsDeleted == null;
            if (regionId != 0)
            {
                PhysicianwhereClauseSyntax = PhysicianwhereClauseSyntax.And(x => x.RegionId == regionId);
            }
            whereClauseSyntax = x => x.IsDeleted == null && x.Status == 1;
            whereClauseSyntax = whereClauseSyntax.And(x => x.ShiftDate == DateOnly.FromDateTime(DateTime.Now));
            whereClauseSyntax = whereClauseSyntax.And(x => x.StartTime < TimeOnly.FromDateTime(DateTime.Now));
            whereClauseSyntax = whereClauseSyntax.And(x => x.EndTime > TimeOnly.FromDateTime(DateTime.Now));

            List<PhysicianOnCallModal> physicians = new List<PhysicianOnCallModal>();
            List<int> physicianOnCall = new List<int>();
            var data = _shiftDetailsRepository.GetAllData(x => x.Shift.PhysicianId, whereClauseSyntax);
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
                Photo = x.Photo!,
            }, PhysicianwhereClauseSyntax);
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



        #endregion


    }
}
