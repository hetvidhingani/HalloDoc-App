using HalloDoc.Repository.IRepository;
using HalloDoc.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


    }
}
