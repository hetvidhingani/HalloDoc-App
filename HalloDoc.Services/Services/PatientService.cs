using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using HalloDoc.Repository.IRepository;
using HalloDoc.Repository.Repository;
using HalloDoc.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Services.Services
{
    public class PatientService : IPatientService
    {
        #region constuctor
        private readonly IAspNetUserRepository _aspnetuserRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestClientRepository _requestclientRepository;
        private readonly IRequestWiseFilesRepository _requestwisefileRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IConciergeRepository _conciergeRepository;
        private readonly IAspNetUserRolesRepository _userRolesRepository;
        private readonly IRegionRepository _regionRepository;

        public PatientService(IAspNetUserRepository aspnetuserRepository, IUserRepository userRepository,
                                IRequestRepository requestRepository, IRequestClientRepository requestclientRepository,
                                IRequestWiseFilesRepository requestwisefileRepository, IBusinessRepository businessRepository,
                                IConciergeRepository conciergeRepository, IAspNetUserRolesRepository userRolesRepository, IRegionRepository regionRepository)
        {
            _userRepository = userRepository;
            _aspnetuserRepository = aspnetuserRepository;
            _requestRepository = requestRepository;
            _requestclientRepository = requestclientRepository;
            _requestwisefileRepository = requestwisefileRepository;
            _businessRepository = businessRepository;
            _conciergeRepository = conciergeRepository;
            _userRolesRepository = userRolesRepository;
            _regionRepository = regionRepository;
        }
        #endregion

        #region comman services
        public async Task<T> GetTempData<T>(string key)
        {
            return await Task.FromResult(_aspnetuserRepository.GetTempData<T>(key));
        }
        public async Task<AspNetUser> GetAspNetUser(string email)
        {
            AspNetUser user = await _aspnetuserRepository.CheckUserByEmail(email);
            return user;
        }
        public async Task<RequestWiseFile> FindFile(string fileName)
        {
            RequestWiseFile file = await _requestwisefileRepository.FindFile(fileName);
            return file;
        }
        public async Task<object> RegionList()
        {
            OtherRequestViewModel viewModel = new OtherRequestViewModel();
            viewModel.State = await _regionRepository.GetRegions();
            return viewModel;
        }
        public async Task<object> RegionListUser()
        {
            PatientRequestViewModel viewModel = new PatientRequestViewModel();
            viewModel.State = await _regionRepository.GetRegions();
            return viewModel;
        }
        #endregion

        #region PatientRequest
        public async Task<object> PatientRequest(int? userId)
        {
            PatientRequestViewModel viewModel = new PatientRequestViewModel();
            viewModel.State = await _regionRepository.GetRegions();
            if (userId != null)
            {
                User user = await _userRepository.GetByIdAsync(userId);
                if (user != null)
                {
                    viewModel.FirstName = user.FirstName;
                    viewModel.LastName = user.LastName;
                    viewModel.City = user.City;
                    viewModel.State = await _regionRepository.GetRegions();
                    viewModel.Street = user.Street;
                    viewModel.Email = user.Email;
                    viewModel.PhoneNumber = user.Mobile;
                    viewModel.ZipCode = user.ZipCode;
                    viewModel.DateOfBirth = user.DateOfBirth;
                    return viewModel;
                }
            }

            return viewModel;
        }
        public async Task<string> PatientRequest(PatientRequestViewModel viewModel)
        {
            Request request = new Request
            {
                RequestTypeId = 1,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                PhoneNumber = viewModel.PhoneNumber,
                Email = viewModel.Email,
                CreatedDate = DateTime.Now,
                Status = 1
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
                DateOfBirth = viewModel.DateOfBirth,
                Address = viewModel.Street + "," + viewModel.City +"," + viewModel.ZipCode

            };
            await _requestclientRepository.AddAsync(requestClient);

            if (viewModel.File != null && viewModel.File.Length > 0)
            {
                    RequestWiseFile newFile = new RequestWiseFile
                    {
                        RequestId = request.RequestId,
                        FileName = viewModel.File.FileName,
                        CreatedDate = DateTime.Now,
                        DocType = 1,
                    };

                    await _requestwisefileRepository.AddAsync(newFile);
                
            }

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
                    PasswordHash = viewModel.Password,
                    PhoneNumber = viewModel.PhoneNumber,
                    Email = viewModel.Email,
                    CreatedDate = DateTime.Now
                };
                await _aspnetuserRepository.AddAsync(newaspNetUSer);
                AspNetUserRole userRole = new AspNetUserRole
                {
                    UserId = newaspNetUSer.Id,
                    RoleId="2"

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

                    DateOfBirth = viewModel.DateOfBirth,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    RegionId = viewModel.RegionId
                };
                await _userRepository.AddAsync(user1);

                request.UserId = user1.UserId;
                await _requestRepository.UpdateAsync(request);

            }

            return "RegisterdPatientLogin";
        }


        #endregion

        #region Family Freind Request

        public async Task<string> FamilyFriendRequest(OtherRequestViewModel viewModel)
        {

            Request request = new Request
            {
                RequestTypeId = 2,
                FirstName = viewModel.ClientFirstName,
                LastName = viewModel.ClientLastName,
                PhoneNumber = viewModel.ClientPhoneNumber,
                Email = viewModel.CLientEmail,
                CreatedDate = DateTime.Now,
                Status = 1
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
                DateOfBirth = viewModel.DateOfBirth,
                Email = viewModel.Email,
                Address = viewModel.Street + "," + viewModel.City + "," + viewModel.ZipCode

            };
            await _requestclientRepository.AddAsync(requestClient);


            if (viewModel.File != null && viewModel.File.Length > 0)
            {
                RequestWiseFile newFile = new RequestWiseFile
                {
                    RequestId = request.RequestId,
                    FileName = viewModel.File.FileName,
                    CreatedDate = DateTime.Now,
                    DocType = 1,
                };

                await _requestwisefileRepository.AddAsync(newFile);

            }

          

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
                return "PatientSite";

            }
            return "RegisterdPatientLogin";

        }

        #endregion

        #region BusinessRequest
        public async Task<string> BusinessRequest(OtherRequestViewModel viewModel)
        {
            Request request = new Request
            {

                RequestTypeId = 4,
                FirstName = viewModel.ClientFirstName,
                LastName = viewModel.ClientLastName,
                PhoneNumber = viewModel.ClientPhoneNumber,
                Email = viewModel.CLientEmail,
                CreatedDate = DateTime.Now,
                Status = 1
            };
            await _requestRepository.AddAsync(request);

            Business business = new Business
            {
                Name = viewModel.ClientProperty,
                RegionId = viewModel.RegionId,
                PhoneNumber = viewModel.ClientPhoneNumber,
                CreatedDate = DateTime.Now,
                CreatedBy = "Admin",
                Address1 = viewModel.ClientProperty,
                Status = 1
            };
            await _businessRepository.AddAsync(business);

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
                DateOfBirth = viewModel.DateOfBirth,
                Address = viewModel.Street + "," + viewModel.City + "," + viewModel.ZipCode


            };
            await _requestclientRepository.AddAsync(requestClient);

            if (viewModel.File != null && viewModel.File.Length > 0)
            {
                RequestWiseFile newFile = new RequestWiseFile
                {
                    RequestId = request.RequestId,
                    FileName = viewModel.File.FileName,
                    CreatedDate = DateTime.Now,
                    DocType = 1,
                };

                await _requestwisefileRepository.AddAsync(newFile);

            }


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
                return "";

            }
            return "RegisterdPatientLogin";
        }
        #endregion

        #region ConciergeRequest
        public async Task<string> ConciergeRequest(OtherRequestViewModel viewModel)
        {
            Request request = new Request
            {
                RequestTypeId = 3,
                FirstName = viewModel.ClientFirstName,
                LastName = viewModel.ClientLastName,
                PhoneNumber = viewModel.ClientPhoneNumber,
                Email = viewModel.CLientEmail,
                CreatedDate = DateTime.Now,
                Status = 1
            };
            await _requestRepository.AddAsync(request);

            Concierge concierge = new Concierge
            {
                ConciergeName = viewModel.ClientFirstName + " " + viewModel.ClientLastName,
                CreatedDate = DateTime.Now,
                RegionId = viewModel.RegionId,
                Address = viewModel.ClientProperty,
                Street = viewModel.ClientStreet,
                State = await _regionRepository.FindState(viewModel.RegionId),
                City = viewModel.ClientCity,
                ZipCode = viewModel.ClientZipCode,

            };
            await _conciergeRepository.AddAsync(concierge);

            RequestClient requestClient = new RequestClient
            {
                RequestId = request.RequestId,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                PhoneNumber = viewModel.PhoneNumber,
                RegionId = viewModel.RegionId,
                Street = concierge.Street,
                City = concierge.City,
                State = concierge.State,
                ZipCode = concierge.ZipCode,
                Notes = viewModel.Symptoms,
                Email = viewModel.Email,
                DateOfBirth = viewModel.DateOfBirth,
                Address = concierge.Street + "," + concierge.City + "," + concierge.ZipCode

            };
            await _requestclientRepository.AddAsync(requestClient);

            if (viewModel.File != null && viewModel.File.Length > 0)
            {
                RequestWiseFile newFile = new RequestWiseFile
                {
                    RequestId = request.RequestId,
                    FileName = viewModel.File.FileName,
                    CreatedDate = DateTime.Now,
                    DocType = 1,
                };

                await _requestwisefileRepository.AddAsync(newFile);

            }

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
                return "";

            }

            return "PatientSite";


        }
        #endregion

        #region check email

        public string? CheckEmail(string email)
        {
            var tmp = _aspnetuserRepository.CheckUserByEmail(email);
           if(tmp.Result == null)
            {
                return "";
            }
            return tmp.Result.Email;

        }
        #endregion

        #region Login
        public async Task<AspNetUser> checkEmailPassword(string email,string password)
        {

            return await _aspnetuserRepository.Login(email, password); 
        }
        public async Task<User> GetUser(string email)
        {
            User user = await _userRepository.CheckUserByEmail(email);
            return user;
        }
        #endregion

        #region PatientForgotPassword
        public async Task<string> PatientForgotPassword(CreateAccountViewModel createAccountViewModel)
        {
            AspNetUser myUser = await _aspnetuserRepository.CheckUserByEmail(createAccountViewModel.Email);

            if (myUser != null)
            {
                if (createAccountViewModel.PasswordHash == createAccountViewModel.ConfirmPassword)
                {
                    myUser.PasswordHash = createAccountViewModel.ConfirmPassword;
                    await _aspnetuserRepository.UpdateAsync(myUser);
                    _aspnetuserRepository.SetTempData("Message", "Password Updated");
                    return "RegisterdPatientLogin";
                }
                else
                {
                    _aspnetuserRepository.SetTempData("Message", "Password and Confirm Password must be same!!!");

                }
            }
            else
            {
                _aspnetuserRepository.SetTempData("Message", "Invalid User Name");
            }
            return "PatientForgotPassword";

        }
        #endregion

        #region Dashboard
        public async Task<object> Dashboard(int? userId)
        {
            int? userID = userId;
            var tabledashboard = (
            from r in _requestRepository.GetAll()
            where r.UserId == userID
            select new DashboardViewModel
            {
                RequstId = r.RequestId,
                CreatedDate = r.CreatedDate.ToShortDateString(),
                Status = r.Status,
                FileName = (
                    from file in _requestwisefileRepository.GetAll()
                    where file.RequestId == r.RequestId
                    select file.FileName
                ).FirstOrDefault(),
                FileCount = (
            from file in _requestwisefileRepository.GetAll()
            where file.RequestId == r.RequestId
            select file.FileName
        ).Count()
            }).ToList();
            return tabledashboard;
        }
        #endregion

        #region create new request(someoneElse)
        public async Task<string> SubmitInformationSomeoneElse(PatientRequestViewModel viewModel, int? userId)
        {

            User userExist = await _userRepository.GetByIdAsync(userId);
            Request request = new Request
            {
                RequestTypeId = 1,
                FirstName = userExist.FirstName,
                LastName = userExist.LastName,
                PhoneNumber = userExist.Mobile,
                Email = userExist.Email,
                CreatedDate = DateTime.Now,
                RelationName = viewModel.RelationName,
                Status = 1
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
                DateOfBirth = viewModel.DateOfBirth,
                Email = viewModel.Email,
            };
            await _requestclientRepository.AddAsync(requestClient);

            if (viewModel.File != null && viewModel.File.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", viewModel.File.FileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    viewModel.File.CopyTo(stream);
                    RequestWiseFile newFile = new RequestWiseFile
                    {
                        RequestId = request.RequestId,
                        FileName = viewModel.File.FileName,
                        CreatedDate = DateTime.Now,
                        DocType = 1,
                    };

                    await _requestwisefileRepository.AddAsync(newFile);
                }
            }

            User user = await _userRepository.CheckUserByEmail(viewModel.Email);
            if (user != null)
            {
                request.UserId = user.UserId;
                await _requestRepository.UpdateAsync(request);

            }

            AspNetUser IsuserExist = await _aspnetuserRepository.CheckUserByEmail(viewModel.Email);
            if (IsuserExist == null)
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



            }
            return "Dashboard";
        }

        #endregion

        #region Create Account

        public async Task<string> CreateAccountRequest(CreateAccountViewModel createAccountViewModel)
        {
            AspNetUser aspNetuser = await _aspnetuserRepository.CheckUserByEmail(createAccountViewModel.Email);
            if (createAccountViewModel.PasswordHash == createAccountViewModel.ConfirmPassword)
            {
                if (aspNetuser != null)

                {
                    aspNetuser.PasswordHash = createAccountViewModel.PasswordHash;
                    await _aspnetuserRepository.UpdateAsync(aspNetuser);

                    RequestClient requestClient = await _requestclientRepository.CheckUserByEmail(createAccountViewModel.Email);

                    User user = new User
                    {
                        Id = aspNetuser.Id,
                        FirstName = requestClient.FirstName,
                        LastName = requestClient.LastName,
                        Email = requestClient.Email,
                        Mobile = requestClient.PhoneNumber,
                        Street = requestClient.Street,
                        City = requestClient.City,
                        State = requestClient.State,
                        ZipCode = requestClient.ZipCode,
                        DateOfBirth = requestClient.DateOfBirth,
                        CreatedBy = "Admin",
                        CreatedDate = DateTime.Now,
                        RegionId = 1
                    };
                    await _userRepository.AddAsync(user);

                    Request req = await _requestRepository.GetByIdAsync(requestClient.RequestId);
                    req.UserId = user.UserId;
                    await _requestRepository.UpdateAsync(req);


                    return "RegisterdPatientLogin";
                }
                else
                {
                    _aspnetuserRepository.SetTempData("errormsg", "Entered Email is wrong");
                    return "CreateAccountPatient";

                }
            }
            else
            {
                _aspnetuserRepository.SetTempData("errormsg", "Password and Confirm Password should be same");
                return "CreateAccountPatient";

            }

        }



        #endregion

        #region Patient My Profile
        public async Task<object> Profile(PatientRequestViewModel requestViewModel, int? userId)
        {
            User user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                requestViewModel.FirstName = user.FirstName;
                requestViewModel.LastName = user.LastName;
                requestViewModel.City = user.City;
                requestViewModel.State = await _regionRepository.GetRegions();
                requestViewModel.Street = user.Street;
                requestViewModel.Email = user.Email;
                requestViewModel.PhoneNumber = user.Mobile;
                requestViewModel.ZipCode = user.ZipCode;
              
                return requestViewModel;

            }
            return "";
        }

        public async Task<string> EditUser(PatientRequestViewModel patientRequestViewModel, int? userId)
        {
            User user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.FirstName = patientRequestViewModel.FirstName;
                user.LastName = patientRequestViewModel.LastName;
                user.Email = patientRequestViewModel.Email;
                user.Street = patientRequestViewModel.Street;
                user.City = patientRequestViewModel.City;
                user.Mobile = patientRequestViewModel.PhoneNumber;
                user.State = await _regionRepository.FindState(patientRequestViewModel.RegionId);
                user.ZipCode = patientRequestViewModel.ZipCode;
                user.StrMonth = patientRequestViewModel.DateOfBirth.Value.ToString();
                user.IntDate = patientRequestViewModel.DateOfBirth.Value.Day;
                user.IntYear = patientRequestViewModel.DateOfBirth.Value.Year;
                user.ModifiedDate = DateTime.Now;

                await _userRepository.UpdateAsync(user);

            }
            return "Profile";
        }

        #endregion

        #region View Document
        public List<DashboardViewModel> ViewDocument(int Id)
        {

            var tableData = (from r in _requestRepository.GetAll()
                             join rwf in _requestwisefileRepository.GetAll()
                             on r.RequestId equals rwf.RequestId
                             where r.RequestId == Id
                             select new
                             {
                                 r.FirstName,
                                 r.LastName,
                                 r.RequestId,
                                 r.CreatedDate,
                                 r.Status,
                                 rwf.FileName
                             }).ToList();

            List<DashboardViewModel> list = new List<DashboardViewModel>();
            foreach (var e in tableData)
            {
                DashboardViewModel model = new DashboardViewModel();
                model.Username = e.FirstName + " " + e.LastName;
                model.RequstId = e.RequestId;
                model.CreatedDate = DateTime.Now.ToShortDateString();
                model.FileName = e.FileName;
                model.Status = e.Status;
                _aspnetuserRepository.SetTempData("reqID", model.RequstId);

                list.Add(model);
            }


            return list;
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



        public async Task<byte[]> DownloadAllByChecked(IEnumerable<string> selectedFiles)
        {
            selectedFiles = selectedFiles.Distinct();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in selectedFiles)
                    {
                        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/uploads/", file);
                        byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);

                        var zipEntry = zipArchive.CreateEntry(file);
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
        public async Task<byte[]> DownloadAll(IEnumerable<string> selectedFiles,int? requestid)
        {
            var filesRow =await  _requestwisefileRepository.FindFileByRequestID(requestid).ToListAsync();
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

    
        #endregion

    }
}
