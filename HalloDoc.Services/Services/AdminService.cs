using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using HalloDoc.Repository.IRepository;
using HalloDoc.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
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
        private readonly IRegionRepository _regionRepository;
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IHealthProfessionalsRepository _healthProfessionalsRepository;
        private readonly IHealthProfessionalTypeRepository _healthProfessionalTypeRepository;
        private readonly IBlockRequestRepository _blockRequestRepository;

        #region constructor
        public AdminService(IAspNetUserRepository aspnetuserRepository, IUserRepository userRepository,
                               IRequestRepository requestRepository, IRequestClientRepository requestclientRepository,
                               IRequestWiseFilesRepository requestwisefileRepository, IBusinessRepository businessRepository,
                               IConciergeRepository conciergeRepository,
                               IPhysicianRepository physicianRepository, IAdminRepository adminRepository, IRequestNotesRepository requestNotesRepository, 
                               ICaseTagRepository caseTagRepository, IRequestStatusLogRepository requestStatusLogRepository,IRegionRepository regionRepository,
                               IOrderDetailsRepository orderDetailsRepository,IHealthProfessionalsRepository healthProfessionalsRepository,
                               IHealthProfessionalTypeRepository healthProfessionalTypeRepository,IBlockRequestRepository blockRequestRepository)
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
        public async Task<RequestWiseFile> FindFile(string fileName)
        {
            RequestWiseFile file = await _requestwisefileRepository.FindFile(fileName);
            return file;
        }
        public async Task<RequestWiseFile> FindFile(string fileName,int? ID)
        {
            RequestWiseFile file = await _requestwisefileRepository.FindFileByID(fileName,ID);
            return file;
        }
        public async Task<T> GetTempData<T>(string key)
        {
            return await Task.FromResult(_aspnetuserRepository.GetTempData<T>(key));
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
                  RequstClientId = p.RequestClientId


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
                  RequestTypeID = r.RequestTypeId,
                  RequstClientId   = rec.RequestClientId,
                  requestID = rec.RequestId


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
                  RequestTypeID = r.RequestTypeId,
                  RequstClientId = rec.RequestClientId,
                  requestID = rec.RequestId



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
                  RequestTypeID = r.RequestTypeId,
                  RequstClientId = rec.RequestClientId,
                  requestID = rec.RequestId


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
                    RequestTypeID = r.RequestTypeId,
                    RequstClientId = rec.RequestClientId,
                    requestID = rec.RequestId
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
                  RequestTypeID = r.RequestTypeId,
                  RequstClientId = rec.RequestClientId,
                  requestID = rec.RequestId

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

        #region Assign Case
        public async Task<object> AssignCase(AssignCaseViewModel viewModel, int id)
        {
            RequestClient patient = await _requestclientRepository.CheckUserByID(id);
            viewModel.RequestClientID = id;
            viewModel.Region = await _regionRepository.GetRegions();
            viewModel.Physician = await _physicianRepository.GetPhysician();
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
            else
            {
                RequestStatusLog requesStatusLog = new RequestStatusLog();
                requesStatusLog.Status = 2;
                requesStatusLog.AdminId = 1;
                requesStatusLog.RequestId = req.RequestId;
                requesStatusLog.Notes = viewModel.AdditionalNotes;
                requesStatusLog.CreatedDate = DateTime.Now;
                await _requestStatusLogRepository.AddAsync(requesStatusLog);
            }

            Request request = await _requestRepository.GetByIdAsync(req.RequestId);

            request.Status = 2;
            request.PhysicianId = viewModel.physicianID;
            request.CaseTag = viewModel.CaseTagID;
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
            request.CaseTag = viewModel.CaseTagID;
            await _requestRepository.UpdateAsync(request);

            BlockRequest blockRequest = new BlockRequest();
            blockRequest.PhoneNumber = requestor.PhoneNumber;
            blockRequest.CreatedDate = DateTime.Now;
            blockRequest.Email = requestor.Email;
            blockRequest.Reason = viewModel.AdditionalNotes;
            blockRequest.RequestId =Convert.ToString( req.RequestId);
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
        public async Task<DashboardViewModel> ViewDocument(int Id)
        { 
            DashboardViewModel viewModel = new DashboardViewModel();
            Request req = await _requestRepository.GetByIdAsync(Id);
            RequestClient requestClient = await _requestclientRepository.CheckUserByClientID(req.RequestId);
            viewModel.FirstName = requestClient.FirstName;
            viewModel.RequstId = Id;
            viewModel.RequestWiseFiles = (
                                         from rwf in _requestwisefileRepository.GetAll()
                                         where rwf.RequestId == Id && rwf.IsDeleted==null
                                         select new RequestWiseFile
                                         {
                                             CreatedDate = rwf.CreatedDate,
                                             FileName = rwf.FileName,
                                             RequestWiseFileId = rwf.RequestWiseFileId

                                         }).ToList();
            return viewModel;
        }
        public async Task<string> ViewDocument(IFormFile a, int Id)
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
                    CreatedDate = DateTime.Now
                };
                await _requestwisefileRepository.AddAsync(newRequestWiseFile);
            }
            return "";
        }

        public async Task<RequestWiseFile> DownloadFile(string name)
        {
            RequestWiseFile reqw = await _requestwisefileRepository.FindFile(name);

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
            var filesRow = await _requestwisefileRepository.FindFileByRequestID(requestid).ToListAsync();
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
       
        public async Task<object> DeleteFile(int fileID)
        {
            RequestWiseFile file = await _requestwisefileRepository.GetByIdAsync(fileID);
            if (IsDeleted(file.IsDeleted))
            {
                return new { Success = false, Message = "File is already deleted" };
            }

            file.IsDeleted = new BitArray(new bool[] { true });
            await _requestwisefileRepository.UpdateAsync(file);
            return file;
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
        public async Task<object> SendOrder(SendOrderViewModel viewModel, int Id)
        {

            viewModel.RequestID = Id;

            viewModel.Profession = await _healthProfessionalTypeRepository.GetProfession();
            viewModel.Business = await _healthProfessionalsRepository.GetVendor();
            return viewModel;
        }
        public async Task<string> SendOrderDetails(SendOrderViewModel viewModel, int id)
        {
            OrderDetail order = new OrderDetail();
            order.VendorId = viewModel.BusinessID;
            order.FaxNumber = viewModel.FaxNumber;
            order.RequestId = id;
            order.Email= viewModel.Email;
            order.BusinessContact = viewModel.Contact;
            order.Prescription = viewModel.OrderDetails;
            order.NoOfRefill = viewModel.Refill;
            order.CreatedDate=DateTime.Now;
            await _orderDetailsRepository.AddAsync(order);
            return "";
        }
        #endregion



    }
}
