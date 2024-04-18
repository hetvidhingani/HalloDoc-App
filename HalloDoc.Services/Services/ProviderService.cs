using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using HalloDoc.Repository.IRepository;
using HalloDoc.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HalloDoc.Entities.ViewModels.ViewNotesViewModel;

namespace HalloDoc.Services.Services
{
    public class ProviderService : IProviderService
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
        public ProviderService(IAspNetUserRepository aspnetuserRepository, IUserRepository userRepository,
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
        public List<Region> getstateDropdown()
        {
            return _regionRepository.GetAll().ToList();
        }
        public async Task<int> GetCount(int statusId, int physicianID)
        {
            return await _requestRepository.GetCountAsync(r => r.Status == statusId && r.UserId != null && r.IsDeleted == null && r.PhysicianId == physicianID);
        }
        #endregion

        #region Dashboard
        public List<ProviderDashboardViewModel> ProviderTable(int providerId, string state, List<ProviderDashboardViewModel> list, int status)
        {

            var tabledashboard = (
                from r in _requestRepository.GetAll()
                join rec in _requestclientRepository.GetAll() on r.RequestId equals rec.RequestId
                where r.Status == status && r.IsDeleted == null && r.UserId != null && r.PhysicianId == providerId

                select new ProviderDashboardViewModel
                {
                    PatientName = rec.FirstName + "," + rec.LastName,
                    PatientPhone = rec.PhoneNumber,
                    RequestorPhone = r.PhoneNumber,
                    Address = rec.Street + "," + rec.City + "," + rec.State + "," + rec.ZipCode,
                    RequestTypeID = r.RequestTypeId,
                    RequstClientId = rec.RequestClientId,
                    requestID = rec.RequestId,
                    StateofTable = rec.State,
                    stateTab = state,

                }).ToList();

            return tabledashboard.OrderByDescending(x => x.PatientName).ToList();
        }
        public ProviderDashboardViewModel Pagination(string state, int CurrentPage, string? PatientName, int? ReqType, List<ProviderDashboardViewModel> newState)
        {
            if (!string.IsNullOrWhiteSpace(PatientName))
            {
                newState = newState.Where(a => a.PatientName.ToLower().Contains(PatientName.ToLower())).ToList();
            }
            if (ReqType != 0 && ReqType != null)
            {
                newState = newState.Where(a => a.RequestTypeID == ReqType).ToList();
            }
            if (CurrentPage == 0)
            {
                CurrentPage = 1;
            }
            int dataSize = 5;
            int totalCount = newState.Count;
            int totalPage = (int)Math.Ceiling((double)totalCount / dataSize);
            int FirstItemIndex = Math.Min((CurrentPage - 1) * dataSize + 1, totalCount);
            int LastItemIndex = Math.Min(CurrentPage * dataSize, totalCount);
            List<ProviderDashboardViewModel> clients = newState
                .OrderByDescending(u => u.PatientName)
                .Skip((CurrentPage - 1) * dataSize)
                .Take(dataSize)
                .ToList();

            return new ProviderDashboardViewModel
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
                viewmodel.PhysicianNotes= requestNote.PhysicianNotes;
            }
            viewmodel.TransferNotes = (from r in _requestRepository.GetAll()
                                       join rsl in _requestStatusLogRepository.GetAll() on r.RequestId equals rsl.RequestId
                                       join p in _physicianRepository.GetAll() on rsl.TransToPhysicianId equals p.PhysicianId into g
                                       from p in g.DefaultIfEmpty()
                                       where r.RequestId == req.RequestId && rsl.Status != 7 && rsl.Status != 3 && rsl.Status != 8
                                       orderby rsl.CreatedDate descending
                                       select new TransferNotesViewModel
                                       {
                                           RequestId = r.RequestId,
                                           FirstName = r.FirstName,
                                           LastName = r.LastName,
                                           CreatedDate = rsl.CreatedDate,
                                           Note = rsl.Notes,
                                           AdminName=rsl.Admin.FirstName + rsl.Admin.LastName,
                                           transferByPhy = rsl.Physician.FirstName + rsl.Physician.LastName,
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

        public async Task<object> AddNotes(string additionalNotes, int id, string providerId)
        {
            RequestNote requestNote = await _requestNotesRepository.CheckByRequestID(id);

            if (requestNote != null)
            {
                requestNote.PhysicianNotes = additionalNotes;
                await _requestNotesRepository.UpdateAsync(requestNote);

            }
            else
            {
                RequestNote requestNote1 = new RequestNote();
                requestNote1.RequestId = id;
                requestNote1.PhysicianNotes = additionalNotes;
                requestNote1.CreatedDate = DateTime.Now;
                requestNote1.CreatedBy = providerId;
                await _requestNotesRepository.AddAsync(requestNote1);

            }

            return "ViewNotes";
        }

        #endregion

        #region Accept Case
        public async Task AcceptCase(int requestClientId, int providerId)
        {
            RequestClient req = await _requestclientRepository.GetByIdAsync(requestClientId);

            RequestStatusLog requesStatusLog = new RequestStatusLog();
            requesStatusLog.Status = 2;
            requesStatusLog.PhysicianId = providerId;
            requesStatusLog.RequestId = req.RequestId;
            requesStatusLog.CreatedDate = DateTime.Now;
            await _requestStatusLogRepository.AddAsync(requesStatusLog);

            Request request = await _requestRepository.GetByIdAsync(req.RequestId);
            request.Status = 2;
            await _requestRepository.UpdateAsync(request);
        }
        #endregion

        #region Transfer Case
        public async Task TransferCase(int id,int providerId,string note)
        {
            RequestClient req = await _requestclientRepository.GetByIdAsync(id);
            RequestStatusLog findAdmin = _requestStatusLogRepository.GetAll().Where(x=>x.RequestId==req.RequestId && x.AdminId!=null).OrderByDescending(x=>x.CreatedDate).FirstOrDefault();
            RequestStatusLog requesStatusLog = new RequestStatusLog
            {
                Status = 1,
                RequestId = req.RequestId,
                Notes =note,
                PhysicianId= providerId,
                CreatedDate = DateTime.Now
            };
            await _requestStatusLogRepository.AddAsync(requesStatusLog);


            Request request = await _requestRepository.GetByIdAsync(req.RequestId);
            request.PhysicianId = null;
            request.Status = 1;
            await _requestRepository.UpdateAsync(request);
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

        
    }
}
