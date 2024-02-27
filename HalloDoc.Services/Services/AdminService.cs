using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using HalloDoc.Repository.IRepository;
using HalloDoc.Services.IServices;
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

        public async Task<AspNetUser> checkEmailPassword(AspNetUser user)
        {

            return await _aspnetuserRepository.Login(user.Email, user.PasswordHash); ;
        }
        public async Task<User> GetUser(string email)
        {
            User user = await _userRepository.CheckUserByEmail(email);
            return user;
        }
        public  List<AdminDashboardViewModel> New()
        {

            var tabledashboard1 = (
              from r in _requestRepository.GetAll()
              join p in _requestclientRepository.GetAll() on r.RequestId equals p.RequestId
              select new AdminDashboardViewModel
              {
                 PatientName=p.FirstName+","+p.LastName,
                  DateOfBirth = p.DateOfBirth,
                  Requestor=r.FirstName+","+r.LastName,
                  RequestedDate=r.CreatedDate,
                  PatientPhone=p.PhoneNumber,
                  RequestorPhone=r.PhoneNumber,
                  Address=p.Street+","+p.City + ","+p.State+","+p.ZipCode,
                  Notes=p.Notes
 
                  
              }).ToList();
              return  tabledashboard1;
         }
        public List<AdminDashboardViewModel> Pending()
        {

            var tabledashboard1 = (
              from r in _requestRepository.GetAll()
              join rec in _requestclientRepository.GetAll() on r.RequestId equals rec.RequestId
              join phy in _physicianRepository.GetAll() on r.PhysicianId equals phy.PhysicianId
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
                  PhysicianName=phy.FirstName+" "+phy.LastName



              }).ToList();
            return tabledashboard1;
        }
    }
}
