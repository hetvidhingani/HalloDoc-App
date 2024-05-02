using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using HalloDoc.Repository.IRepository;
using HalloDoc.Services.IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static HalloDoc.Entities.ViewModels.ViewNotesViewModel;
using static iTextSharp.text.pdf.AcroFields;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private readonly ITimeDetailsRepository _timeDetailsRepository;
        private readonly IQuaterSheetRepository _quaterSheetRepository;
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
                               IShiftRepository shiftRepository, IPhysicianLocationRepository physicianLocationRepository, IPhysicianRegionRepository physicianRegionRepository,
                               ITimeDetailsRepository timeDetailsRepository, IQuaterSheetRepository quaterSheetRepository)
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
            _timeDetailsRepository = timeDetailsRepository;
            _quaterSheetRepository = quaterSheetRepository;
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
                    callType = r.CallType,
                    stateTab = state,
                    isfinalize = r.CompletedByPhysician == null ? false : true,
                    PhysicianId = providerId,

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
        public async Task TransferCase(int id, int providerId, string note)
        {
            RequestClient req = await _requestclientRepository.GetByIdAsync(id);
            RequestStatusLog findAdmin = _requestStatusLogRepository.GetAll().Where(x => x.RequestId == req.RequestId && x.AdminId != null).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            RequestStatusLog requesStatusLog = new RequestStatusLog
            {
                Status = 1,
                RequestId = req.RequestId,
                Notes = note,
                PhysicianId = providerId,
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

        #region Type of Care
        public void HouseCallOrCounsult(int id, int requestId)
        {
            Request request = _requestRepository.GetById(requestId);
            request.ModifiedDate = DateTime.Now;
            if (id == 2)
            {
                request.Status = 6;
                request.CallType = 2;
                _requestRepository.UpdateAsync(request);
            }
            else if (id == 1)
            {
                request.CallType = 1;
                request.Status = 5;

                _requestRepository.UpdateAsync(request);
            }
            else if (id == 3)
            {
                request.Status = 6;
                _requestRepository.UpdateAsync(request);
            }
        }
        #endregion

        #region conclude care
        public ConcludeCareViewModel ConcludeCare(int id)
        {
            RequestClient req = _requestclientRepository.GetAll().Where(x => x.RequestId == id).FirstOrDefault();
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads", id + "_" + "Encounter_Data.pdf");
            ConcludeCareViewModel model = new ConcludeCareViewModel();
            model.filePath = id + "_" + "Encounter_Data.pdf";
            model.requestId = id;
            model.username = req.FirstName + " " + req.LastName;
            return model;
        }
        public void ConcludeRequest(int id, string? note, string providerID)
        {
            Request request = _requestRepository.GetById(id);
            request.ModifiedDate = DateTime.Now;
            request.Status = 8;
            _requestRepository.UpdateAsync(request);

            RequestNote reqNote = _requestNotesRepository.GetAll().Where(x => x.RequestId == id).FirstOrDefault();
            if (reqNote != null)
            {

                reqNote.PhysicianNotes = note;
                _requestNotesRepository.UpdateAsync(reqNote);
            }
            else
            {
                RequestNote requestNote1 = new RequestNote();
                requestNote1.RequestId = id;
                requestNote1.PhysicianNotes = note;
                requestNote1.CreatedDate = DateTime.Now;
                requestNote1.CreatedBy = providerID;
                _requestNotesRepository.AddAsync(requestNote1);
            }

            RequestStatusLog statusLog = new()
            {
                RequestId = id,
                Status = 8,
                PhysicianId = request.PhysicianId,
                CreatedDate = DateTime.Now,
            };
            _requestStatusLogRepository.UpdateAsync(statusLog);

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

        public async Task FinalizeReport(int RequestID)
        {
            var enc = _encounterRepository.GetAll().Where(x => x.Requestid == RequestID).FirstOrDefault();
            enc.Isfinalized = true;
            _encounterRepository.UpdateAsync(enc);
            Request req = _requestRepository.GetById(RequestID);

            req.CompletedByPhysician = new BitArray(new bool[] { true });
            _requestRepository.UpdateAsync(req);
        }

        public Encounter GetEncounterForm(int id)
        {
            Encounter request = _encounterRepository.GetAll().Where(x => x.Requestid == id).FirstOrDefault();

            return request;
        }

        public byte[] Downloadpdf(Encounter rowdata)
        {
            Document document = new Document();
            MemoryStream ms = new MemoryStream();
            PdfWriter.GetInstance(document, ms);
            document.Open();

            PdfPTable dataTable = new PdfPTable(2);
            dataTable.WidthPercentage = 100;

            PdfPCell headerCell = new PdfPCell(new Phrase("Encounter Data : " + rowdata.Firstname + " , " + rowdata.Lastname));
            headerCell.Colspan = 2;
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            dataTable.AddCell(headerCell);

            foreach (var property in rowdata.GetType().GetProperties())
            {
                PdfPCell nameCell = new PdfPCell(new Phrase(property.Name));
                nameCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                dataTable.AddCell(nameCell);

                PdfPCell valueCell = new PdfPCell(new Phrase(property.GetValue(rowdata)?.ToString()));
                dataTable.AddCell(valueCell);
            }

            document.Add(dataTable);
            document.Close();

            // Convert MemoryStream to byte array
            byte[] bytes = ms.ToArray();
            ms.Close();

            return bytes;
        }

        #endregion

        #region my profile
        public async Task<object> resetPasswordProvider(int id, string password)
        {
            Physician phy = await _physicianRepository.GetByIdAsync(id);
            AspNetUser user = await _aspnetuserRepository.GetByIdAsync(phy.Id);
            user.PasswordHash = _aspnetuserRepository.EncodePasswordToBase64(password);
            await _aspnetuserRepository.UpdateAsync(user);
            return user;
        }
        public async Task<object> MyProfile(int physicianID)
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
            model.Password = _aspnetuserRepository.DecodeFrom64(asp.PasswordHash);
            model.RoleId = (int)phy.RoleId;
            model.Role = _roleRepository.GetAll().Where(u => u.AccountType == 2).ToList();
            model.statusId = (int)phy.Status;
            model.status = _statusRepository.GetAll().Where(x => x.Statusid == 4 || x.Statusid == 2).ToList();
            model.IsAgreementDoc = phy.IsAgreementDoc == null ? false : true;
            model.isbackgroundcheck = phy.IsBackgroundDoc == null ? false : true;
            model.IsAgreementDocnondisclosure = phy.IsNonDisclosureDoc == null ? false : true;
            model.Ishippa = phy.IsTrainingDoc == null ? false : true;

            model.StateCheckbox = regions.Select(r => new RegionViewModel()
            {
                RegionId = r.RegionId,
                Name = r.Name,
                IsChecked = checkedRegions.ContainsKey(r.RegionId)
            }).ToList();
            return model;
        }

        public Physician getproviderEmail(int id)
        {
            return _physicianRepository.GetById(id);
        }
        #endregion

        #region Schaduling
        public SchedulingModel Scheduling(int physician)
        {
            SchedulingModel schedulingModel = new SchedulingModel();

            List<Resources> resources = new List<Resources>();
            List<Events> events = new List<Events>();

            Expression<Func<Physician, bool>> tableData = PredicateBuilder.New<Physician>();
            Expression<Func<ShiftDetail, bool>> ShiftDetailstableData = PredicateBuilder.New<ShiftDetail>();
            tableData = x => x.IsDeleted == null && x.PhysicianId == physician;
            ShiftDetailstableData = x => x.IsDeleted == null && x.Shift.PhysicianId == physician;

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
        public void AddShift(CreateShiftViewModel model, List<DayOfWeek> WeekDays, string providerId)
        {
            Physician phy = _physicianRepository.GetById(model.provider);

            Shift shift = new()
            {
                PhysicianId = model.provider,
                StartDate = model.ShiftDate,
                IsRepeat = new BitArray(1, model.toggle),
                WeekDays = WeekDays.ToString(),
                RepeatUpto = model.repeatEnd,
                CreatedBy = providerId.ToString(),
            };
            _shiftRepository.AddAsync(shift);

            ShiftDetail shiftDetail = new()
            {
                ShiftId = shift.ShiftId,
                ShiftDate = shift.StartDate,
                StartTime = model.startTime,
                EndTime = model.endTime,
                RegionId = phy.RegionId,
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
                        RegionId = phy.RegionId,
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

        #region Create Request By Admin
        public async Task CreateRequestByProvider(PatientRequestViewModel viewModel, int provider)
        {
            var data = _physicianRepository.GetById(provider);
            Request request = new Request
            {
                RequestTypeId = 2,
                FirstName = data.FirstName,
                LastName = data.LastName,
                PhoneNumber = data.Mobile,
                Email = data.Email,
                CreatedDate = DateTime.Now,
                Status = 2,
                PhysicianId = provider,

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
            else
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
                User user1 = new User
                {
                    Id = newaspNetUSer.Id,
                    FirstName = requestClient.FirstName,
                    LastName = requestClient.LastName,
                    Email = requestClient.Email,
                    Mobile = requestClient.PhoneNumber,
                    Street = requestClient.Street,
                    City = requestClient.City,
                    State = requestClient.State,
                    ZipCode = requestClient.ZipCode,

                    IntDate = requestClient.IntDate,
                    IntYear = requestClient.IntYear,
                    StrMonth = requestClient.StrMonth,
                    CreatedBy = data.Id,
                    CreatedDate = DateTime.Now,
                    RegionId = viewModel.RegionId
                };
                await _userRepository.AddAsync(user1);

                request.UserId = user1.UserId;
                await _requestRepository.UpdateAsync(request);
            }
        }
        #endregion

        #region Invoicing
        public QuarterSheet checkIfTimeSheet(string? startrange)
        {
            QuarterSheet check = _quaterSheetRepository.check(startrange);
            if (check == null)
            {
                return null;
            }
            else
            {
                return check;
            }
        }

        public TimeSheetViewModel Sheet(TimeSheetViewModel viewmodel, int id)
        {
            string[] part = viewmodel.range.Split(new string[] { " - " }, StringSplitOptions.None);

            DateOnly startdate = DateOnly.Parse(part[0]);
            DateOnly enddate = DateOnly.Parse(part[1]);
            QuarterSheet check = _quaterSheetRepository.GetAll().Where(x => x.StartDate == startdate).FirstOrDefault();
            TimeSheetViewModel model = new TimeSheetViewModel();
            model.physicianID = id;
           
            if (check != null)
            {
                List<TimeSheetViewModel> list = new List<TimeSheetViewModel>();
                Expression<Func<TimeSheetDetail, bool>> expression = PredicateBuilder.New<TimeSheetDetail>();
                expression = x => x.TimeSheetId == check.TimeSheetId;

                var result = _timeDetailsRepository.GetAllWithPagination(x => new TimeSheetViewModel
                {
                    totalHr = x.TotalHours == null ? x.NightShiftWeekend : x.TotalHours,
                    Housecalls = x.HouseCall == null ? x.HouseCallNightWeekend : x.HouseCall,
                    phoneConsult = x.PhoneConsult == null ? x.PhoneNightWeekend : x.PhoneConsult,
                    holiday = x.Holiday == null ? false : true,
                    item = x.Item,
                    amount = x.Amount,
                    billname = x.Bill == null ? null : x.Bill,
                }, expression, 1, 20, x => x.Date, true);
                foreach (var item in result)
                {
                    list.Add(item);
                }
                model.TimeSheets = list;

            }
          
            model.startDate = startdate;
            model.endDate = enddate;
            return model;
        }

        public TimeSheetViewModel TimeSheet(string? startrange, string? endrange, int id)
        {
            DateOnly startdate = DateOnly.Parse(startrange!);
            DateOnly enddate = DateOnly.Parse(endrange!);


            QuarterSheet check = _quaterSheetRepository.GetAll().Where(x => x.StartDate == startdate).FirstOrDefault()!;
            TimeSheetViewModel model = new TimeSheetViewModel();
            if (check != null)
            {

                List<TimeSheetViewModel> list = new List<TimeSheetViewModel>();
                Expression<Func<TimeSheetDetail, bool>> expression = PredicateBuilder.New<TimeSheetDetail>();
                expression = x => x.TimeSheetId == check.TimeSheetId;

                var result = _timeDetailsRepository.GetAllData(x => new TimeSheetViewModel
                {
                    totalHr = x.TotalHours,
                    Housecalls = x.HouseCall,
                    phoneConsult = x.PhoneConsult,
                    HousecallNightsWeekend = x.HouseCallNightWeekend,
                    NightShiftWeekend = x.NightShiftWeekend,
                    PhoneconsultNightWeekend = x.PhoneNightWeekend,
                    holiday = x.Holiday == null ? false : true,

                }, expression);
                foreach (var item in result)
                {
                    item.shiftCount = _shiftDetailsRepository.GetCount(x => x.ShiftDetailId, x => x.ShiftDate == startdate && x.Shift.PhysicianId == id);
                    list.Add(item);
                }
                foreach (var item in result)
                {
                    list.Add(item);
                }
                model.TimeSheets = list;

            }
            model.startDate = startdate;
            model.endDate = enddate;

            return model;
        }

        public TimeSheetViewModel ReimbursementSheet(string? startrange, string? endrange, int id,int CurrentPage)
        {
            DateOnly startdate = DateOnly.Parse(startrange!);
            DateOnly enddate = DateOnly.Parse(endrange!);


            QuarterSheet check = _quaterSheetRepository.GetAll().Where(x => x.StartDate == startdate).FirstOrDefault()!;
            TimeSheetViewModel model = new TimeSheetViewModel();

            List<TimeSheetViewModel> list = new List<TimeSheetViewModel>();
            Expression<Func<TimeSheetDetail, bool>> expression = PredicateBuilder.New<TimeSheetDetail>();
            expression = x => x.TimeSheetId == check.TimeSheetId && x.Bill != null;

            var result = _timeDetailsRepository.GetAllWithPagination(x => new TimeSheetViewModel
            {
                startDate = x.Date,
                item = x.Item,
                amount = x.Amount,
                billname = x.Bill,
            }, expression, CurrentPage, 5, x => x.Date, false);
            foreach (var item in result)
            {
                list.Add(item);
            }

            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = list.Count();
            int totalPage = (int)Math.Ceiling((double)totalCount / dataSize);
            int FirstItemIndex = Math.Min((CurrentPage - 1) * dataSize + 1, totalCount);
            int LastItemIndex = Math.Min(CurrentPage * dataSize, totalCount);

            return new TimeSheetViewModel
            {
                TimeSheets = list,
                TotalCount = totalCount,
                TotalPages = totalPage,
                CurrentPage = CurrentPage,
                PageSize = 3,
                FirstItemIndex = FirstItemIndex,
                LastItemIndex = LastItemIndex,
                startDate = startdate,
                endDate = enddate,
                physicianID=id,
            };


        }

        public void SaveTimeSheet(TimeSheetViewModel model)
        {
            QuarterSheet check = _quaterSheetRepository.GetAll().Where(x => x.StartDate == model.startDate).FirstOrDefault();

            if (check == null)
            {
                QuarterSheet sheet = new QuarterSheet();
                sheet.StartDate = model.startDate;
                sheet.EndDate = model.endDate;
                sheet.CreatedDate = DateTime.Now;
                sheet.PhysicianId = model.physicianID;

                _quaterSheetRepository.AddAsync(sheet);

                foreach (var item in model.TimeSheets)
                {
                    TimeSheetDetail table = new TimeSheetDetail();
                    table.Date = item.startDate;
                    table.TimeSheetId = sheet.TimeSheetId;
                    table.Item = item.item;
                    table.Amount = item.amount;
                    table.Bill = item.bill == null ? null : item.bill.FileName;
                    if (item.bill != null || item.item != null || item.amount != null)
                    {
                        table.IsReceipt = new BitArray(new bool[] { true });
                    }
                    if (item.holiday == true)
                    {
                        table.NightShiftWeekend = item.totalHr;
                        table.HouseCallNightWeekend = item.Housecalls;
                        table.PhoneNightWeekend = item.phoneConsult;
                        table.Holiday = new BitArray(new bool[] { true });
                    }
                    else
                    {
                        table.TotalHours = item.totalHr;
                        table.HouseCall = item.Housecalls;
                        table.PhoneConsult = item.phoneConsult;
                    }
                    if (item.bill != null)
                    {

                        var newName = $"{model.physicianID}_{item.startDate}_bill.pdf";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/Bill/", newName);

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        using var stream = System.IO.File.Create(filePath);
                        item.bill.CopyTo(stream);
                    }
                    _timeDetailsRepository.AddAsync(table);
                }

            }
            else
            {
                check.StartDate = model.startDate;
                check.EndDate = model.endDate;
                check.PhysicianId = model.physicianID;
                _quaterSheetRepository.UpdateAsync(check);

                foreach (var item in model.TimeSheets)
                {
                    TimeSheetDetail table = _timeDetailsRepository.GetAll().Where(x => x.Date == item.startDate).FirstOrDefault();
                    table.Date = item.startDate;
                    table.TimeSheetId = check.TimeSheetId;
                    table.Item = item.item;
                    table.Amount = item.amount;
                    table.Bill = item.bill == null ? null : item.bill.FileName;
                    if (item.bill != null || item.item != null || item.amount != null)
                    {
                        table.IsReceipt = new BitArray(new bool[] { true });
                    }
                    if (item.holiday == true)
                    {
                        table.NightShiftWeekend = item.totalHr;
                        table.HouseCallNightWeekend = item.Housecalls;
                        table.PhoneNightWeekend = item.phoneConsult;
                        table.Holiday = new BitArray(new bool[] { true });
                    }
                    else
                    {
                        table.TotalHours = item.totalHr;
                        table.HouseCall = item.Housecalls;
                        table.PhoneConsult = item.phoneConsult;
                    }
                    if (item.bill != null)
                    {
                        var newName = $"{model.physicianID}_{item.startDate}_bill.pdf";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/Bill/", newName);

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        using var stream = System.IO.File.Create(filePath);
                        item.bill.CopyTo(stream);
                    }
                    _timeDetailsRepository.UpdateAsync(table);
                }
            }
        }

        public void FinalizeSheet(DateOnly date)
        {
            QuarterSheet sheet = _quaterSheetRepository.GetAll().Where(x => x.StartDate == date).FirstOrDefault();
            if (sheet != null)
            {
                sheet.IsFinalized = new BitArray(new bool[] { true });
            }
            _quaterSheetRepository.UpdateAsync(sheet);
        }

        public void DeleteBill(DateOnly date)
        {
            TimeSheetDetail sheet = _timeDetailsRepository.GetAll().Where(x => x.Date == date).FirstOrDefault();
            if (sheet != null)
            {
                sheet.Item = null;
                sheet.Bill = null;
                sheet.Amount = null;

            }
            _timeDetailsRepository.UpdateAsync(sheet);
        }

        #endregion

    }
}
