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
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestClientRepository _requestclientRepository;
        private readonly IRequestWiseFilesRepository _requestwisefileRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IConciergeRepository _conciergeRepository;
        private readonly IPhysicianRepository _physicianRepository;

        #region constructor
        public AdminService(IAspNetUserRepository aspnetuserRepository, IUserRepository userRepository,
                               IRequestRepository requestRepository, IRequestClientRepository requestclientRepository,
                               IRequestWiseFilesRepository requestwisefileRepository, IBusinessRepository businessRepository,
                               IConciergeRepository conciergeRepository,
                               IPhysicianRepository physicianRepository)
        {
            _userRepository = userRepository;
            _aspnetuserRepository = aspnetuserRepository;
            _requestRepository = requestRepository;
            _requestclientRepository = requestclientRepository;
            _requestwisefileRepository = requestwisefileRepository;
            _businessRepository = businessRepository;
            _conciergeRepository = conciergeRepository;
            _physicianRepository = physicianRepository;
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
              where r.Status==1 
              select new AdminDashboardViewModel
              {
                 PatientName=p.FirstName+","+p.LastName,
                  DateOfBirth = p.DateOfBirth,
                  Requestor=r.FirstName+","+r.LastName,
                  RequestedDate=r.CreatedDate,
                  PatientPhone=p.PhoneNumber,
                  RequestorPhone=r.PhoneNumber,
                  Address=p.Street+","+p.City + ","+p.State+","+p.ZipCode,
                  Notes=p.Notes,
                  RequestTypeID=r.RequestTypeId,
                  Status=r.Status,
                  requestID=p.RequestClientId
                    
                  
              }).ToList();
              return  tabledashboard1;
         }
        public List<AdminDashboardViewModel> Pending()
        {

            var tabledashboard1 = (
              from r in _requestRepository.GetAll()
              join rec in _requestclientRepository.GetAll() on r.RequestId equals rec.RequestId
              join phy in _physicianRepository.GetAll() on r.PhysicianId equals phy.PhysicianId
              where r.Status == 1

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
                  PhysicianName=phy.FirstName+" "+phy.LastName,
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
        public List<AdminDashboardViewModel> Conclude()
        {

            var tabledashboard1 = (
              from r in _requestRepository.GetAll()
              join rec in _requestclientRepository.GetAll() on r.RequestId equals rec.RequestId
              join phy in _physicianRepository.GetAll() on r.PhysicianId equals phy.PhysicianId
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
        public List<AdminDashboardViewModel> Unpaid()
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
                viewmodel.City = user.City;
                viewmodel.FirstName = user.FirstName;
                viewmodel.State = user.State;
                viewmodel.Street = user.Street;
                viewmodel.Email = user.Email;
                viewmodel.PhoneNumber = user.PhoneNumber;
                viewmodel.ZipCode = user.ZipCode;
               
              
                return viewmodel;

            }
            return "ViewCase";
        }

        #endregion
    }
}
