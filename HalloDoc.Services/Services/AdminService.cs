using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using HalloDoc.Repository.IRepository;
using HalloDoc.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Services.Services
{
    public class AdminService : IAdminService
    {
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

        #region constructor
        public AdminService(IAspNetUserRepository aspnetuserRepository, IUserRepository userRepository,
                               IRequestRepository requestRepository, IRequestClientRepository requestclientRepository,
                               IRequestWiseFilesRepository requestwisefileRepository, IBusinessRepository businessRepository,
                               IConciergeRepository conciergeRepository,
                               IPhysicianRepository physicianRepository, IAdminRepository adminRepository, IRequestNotesRepository requestNotesRepository, 
                               ICaseTagRepository caseTagRepository, IRequestStatusLogRepository requestStatusLogRepository)
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
        }
        #endregion
        
        #region common methods
        public async Task<AspNetUser> checkEmailPassword(AspNetUser user)
        {

            return await _aspnetuserRepository.Login(user.Email, user.PasswordHash); ;
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
            return await _requestRepository.GetCountAsync(r => r.Status == statusId);
        }
        #endregion
       
        #region Dashboard
        public List<AdminDashboardViewModel> New()
        {

            var tabledashboard1 = (

              from r in _requestRepository.GetAll()
              join p in _requestclientRepository.GetAll() on r.RequestId equals p.RequestId
              where r.Status == 1
              select new AdminDashboardViewModel
              {
                  PatientName = p.FirstName + "," + p.LastName,
                  DateOfBirth = p.DateOfBirth,
                  Requestor = r.FirstName + "," + r.LastName,
                  RequestedDate = r.CreatedDate,
                  PatientPhone = p.PhoneNumber,
                  RequestorPhone = r.PhoneNumber,
                  Address = p.Street + "," + p.City + "," + p.State + "," + p.ZipCode,
                  Notes = p.Notes,
                  RequestTypeID = r.RequestTypeId,
                  Status = r.Status,
                  requestID = p.RequestClientId


              }).ToList();
            return tabledashboard1;
        }
        public List<AdminDashboardViewModel> Pending()
        {

            var tabledashboard1 = (
              from r in _requestRepository.GetAll()
              join rec in _requestclientRepository.GetAll() on r.RequestId equals rec.RequestId
              join phy in _physicianRepository.GetAll() on r.PhysicianId equals phy.PhysicianId
              where r.Status == 2

              select new AdminDashboardViewModel
              {
                  PatientName = rec.FirstName + "," + rec.LastName,
                  DateOfBirth = rec.DateOfBirth,
                  Requestor = r.FirstName + "," + r.LastName,
                  RequestedDate = r.CreatedDate,
                  PatientPhone = rec.PhoneNumber,
                  RequestorPhone = r.PhoneNumber,
                  Address = rec.Street + "," + rec.City + "," + rec.State + "," + rec.ZipCode,
                  Notes = rec.Notes,
                  PhysicianName = phy.FirstName + " " + phy.LastName,
                  RequestTypeID = r.RequestTypeId



              }).ToList();
            return tabledashboard1;
        }
        public List<AdminDashboardViewModel> Active()
        {

            var tabledashboard1 = (
              from r in _requestRepository.GetAll()
              join rec in _requestclientRepository.GetAll() on r.RequestId equals rec.RequestId
              join phy in _physicianRepository.GetAll() on r.PhysicianId equals phy.PhysicianId
              where r.Status == 4

              select new AdminDashboardViewModel
              {
                  PatientName = rec.FirstName + "," + rec.LastName,
                  DateOfBirth = rec.DateOfBirth,
                  Requestor = r.FirstName + "," + r.LastName,
                  RequestedDate = r.CreatedDate,
                  PatientPhone = rec.PhoneNumber,
                  RequestorPhone = r.PhoneNumber,
                  Address = rec.Street + "," + rec.City + "," + rec.State + "," + rec.ZipCode,
                  Notes = rec.Notes,
                  PhysicianName = phy.FirstName + " " + phy.LastName,
                  RequestTypeID = r.RequestTypeId



              }).ToList();
            return tabledashboard1;
        }
        public List<AdminDashboardViewModel> Conclude()
        {

            var tabledashboard1 = (
              from r in _requestRepository.GetAll()
              join rec in _requestclientRepository.GetAll() on r.RequestId equals rec.RequestId
              join phy in _physicianRepository.GetAll() on r.PhysicianId equals phy.PhysicianId
              where r.Status == 6

              select new AdminDashboardViewModel
              {
                  PatientName = rec.FirstName + "," + rec.LastName,
                  DateOfBirth = rec.DateOfBirth,
                  Requestor = r.FirstName + "," + r.LastName,
                  RequestedDate = r.CreatedDate,
                  PatientPhone = rec.PhoneNumber,
                  RequestorPhone = r.PhoneNumber,
                  Address = rec.Street + "," + rec.City + "," + rec.State + "," + rec.ZipCode,
                  Notes = rec.Notes,
                  PhysicianName = phy.FirstName + " " + phy.LastName,
                  RequestTypeID = r.RequestTypeId



              }).ToList();
            return tabledashboard1;
        }
        public List<AdminDashboardViewModel> ToClose()
        {
            var tabledashboard1 = (
                from r in _requestRepository.GetAll()
                join rec in _requestclientRepository.GetAll() on r.RequestId equals rec.RequestId
                join phy in _physicianRepository.GetAll().DefaultIfEmpty() on r.PhysicianId equals phy.PhysicianId into leftJoin
                from subPhy in leftJoin.DefaultIfEmpty()
                where r.Status == 3

                select new AdminDashboardViewModel
                {
                    PatientName = rec.FirstName + "," + rec.LastName,
                    DateOfBirth = rec.DateOfBirth,
                    Requestor = r.FirstName + "," + r.LastName,
                    RequestedDate = r.CreatedDate,
                    PatientPhone = rec.PhoneNumber,
                    RequestorPhone = r.PhoneNumber,
                    Address = rec.Street + "," + rec.City + "," + rec.State + "," + rec.ZipCode,
                    Notes = rec.Notes,
                    PhysicianName = subPhy.FirstName + " " + subPhy.LastName,
                    RequestTypeID = r.RequestTypeId
                }).ToList();
            return tabledashboard1;
        }
        public List<AdminDashboardViewModel> Unpaid()
        {

            var tabledashboard1 = (
              from r in _requestRepository.GetAll()
              join rec in _requestclientRepository.GetAll() on r.RequestId equals rec.RequestId
              join phy in _physicianRepository.GetAll() on r.PhysicianId equals phy.PhysicianId
              where r.Status == 9

              select new AdminDashboardViewModel
              {
                  PatientName = rec.FirstName + "," + rec.LastName,
                  DateOfBirth = rec.DateOfBirth,
                  Requestor = r.FirstName + "," + r.LastName,
                  RequestedDate = r.CreatedDate,
                  PatientPhone = rec.PhoneNumber,
                  RequestorPhone = r.PhoneNumber,
                  Address = rec.Street + "," + rec.City + "," + rec.State + "," + rec.ZipCode,
                  Notes = rec.Notes,
                  PhysicianName = phy.FirstName + " " + phy.LastName,
                  RequestTypeID = r.RequestTypeId



              }).ToList();
            return tabledashboard1;
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
        public async Task<object> ViewNotes(AdminDashboardViewModel viewmodel, int id)
        {
            RequestClient req = await _requestclientRepository.GetByIdAsync(id);
            RequestNote requestNote = await _requestNotesRepository.CheckByRequestID(req.RequestId);
            if (requestNote != null)
            {
                viewmodel.AdminNotes = requestNote.AdminNotes;
                
            }
            return viewmodel;
        }

        public async Task<object> AddNotes(AdminDashboardViewModel viewmodel, int Id)
        {
            RequestClient req = await _requestclientRepository.GetByIdAsync(Id);

            RequestNote requestNote = await _requestNotesRepository.CheckByRequestID(req.RequestId);

            if (requestNote != null)
            {
                requestNote.AdminNotes = viewmodel.AdditionalNotes;
                await _requestNotesRepository.UpdateAsync(requestNote);
                viewmodel.AdminNotes=requestNote.AdminNotes;
               
            }
            else
            {
                RequestNote requestNote1 = new RequestNote();
                requestNote1.RequestId = req.RequestId;
                requestNote1.AdminNotes = viewmodel.AdditionalNotes;
                requestNote1.CreatedDate = DateTime.Now;
                requestNote1.CreatedBy = "admin";
                await _requestNotesRepository.AddAsync(requestNote1);
                viewmodel.AdminNotes = requestNote1.AdminNotes;
               

            }
            viewmodel.AdditionalNotes= string.Empty;
            return viewmodel;
        }
        #endregion

        #region cancel case
        public async Task<object> CancelCase(CancelCaseViewModel viewModel, int id)
        {

            RequestClient patient =await _requestclientRepository.CheckUserByID(id);
           
            viewModel.PatientName = patient.FirstName + " " + patient.LastName;
            viewModel.RequestClientID = id;
            viewModel.Case = await _caseTagRepository.GetCases() ;
          
             return viewModel;
           
        }
        public async Task<string> ConfirmCancelCase(CancelCaseViewModel viewModel , int id)
        {
            RequestClient req = await _requestclientRepository.GetByIdAsync(id);

            RequestStatusLog requestNote1 = await _requestStatusLogRepository.CheckByRequestID(req.RequestId);

          
            if(requestNote1 != null)
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
           
           
            Request request =await _requestRepository.GetByIdAsync(req.RequestId);
            
            request.Status = 3;
            request.CaseTag = viewModel.CaseTagID;
            await _requestRepository.UpdateAsync(request);

            return "Dashboard";

        }
        #endregion
    }
}
