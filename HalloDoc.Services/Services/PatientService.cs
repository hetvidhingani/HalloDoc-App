using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using HalloDoc.Repository.IRepository;
using HalloDoc.Repository.Repository;
using HalloDoc.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Services.Services
{
    public class PatientService : IPatientService
    {
        private readonly IAspNetUserRepository _aspnetuserRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestClientRepository _requestclientRepository;
        private readonly IRequestWiseFilesRepository _requestwisefileRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IConciergeRepository _conciergeRepository;

        #region constuctor
        public PatientService(IAspNetUserRepository aspnetuserRepository, IUserRepository userRepository,
                                IRequestRepository requestRepository, IRequestClientRepository requestclientRepository,
                                IRequestWiseFilesRepository requestwisefileRepository, IBusinessRepository businessRepository,
                                IConciergeRepository conciergeRepository)
        {
            _userRepository = userRepository;
            _aspnetuserRepository = aspnetuserRepository;
            _requestRepository = requestRepository;
            _requestclientRepository = requestclientRepository;
            _requestwisefileRepository = requestwisefileRepository;
            _businessRepository = businessRepository;
            _conciergeRepository = conciergeRepository;
        }
        #endregion

        #region PatientRequest
        public async Task<object> PatientRequest(int? userId)
        {
            if (userId != null)
            {
                User user = await _userRepository.GetByIdAsync(userId);
                PatientRequestViewModel viewModel = new PatientRequestViewModel();
                if (user != null)
                {
                    viewModel.FirstName = user.FirstName;
                    viewModel.LastName = user.LastName;
                    viewModel.City = user.City;
                    viewModel.State = user.State;
                    viewModel.Street = user.Street;
                    viewModel.Email = user.Email;
                    viewModel.PhoneNumber = user.Mobile;
                    viewModel.ZipCode = user.ZipCode;
                    viewModel.DateOfBirth = user.DateOfBirth;

                    return viewModel;
                }
            }
            return "";
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

            int id = request.RequestId;

            RequestClient requestClient = new RequestClient
            {
                RequestId = request.RequestId,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                PhoneNumber = viewModel.PhoneNumber,
                RegionId = 1,
                Street = viewModel.Street,
                City = viewModel.City,
                State = viewModel.State,
                ZipCode = viewModel.ZipCode,
                Notes = viewModel.Symptoms,
                Email = viewModel.Email,
                DateOfBirth = viewModel.DateOfBirth

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
                    RegionId = 1
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
                RegionId = 1,
                Street = viewModel.Street,
                City = viewModel.City,
                State = viewModel.State,
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
                RegionId = 1,
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
                RegionId = 1,
                Street = viewModel.Street,
                City = viewModel.City,
                State = viewModel.State,
                ZipCode = viewModel.ZipCode,
                Notes = viewModel.Symptoms,
                Email = viewModel.Email,
                DateOfBirth = viewModel.DateOfBirth

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
                _requestRepository.UpdateAsync(request);

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

                _aspnetuserRepository.AddAsync(newaspNetUSer);

                return "PatientSite";

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
                Address = viewModel.ClientProperty,
                Street = viewModel.ClientStreet,
                State = viewModel.ClientState,
                City = viewModel.ClientCity,
                ZipCode = viewModel.ClientZipCode,

                RegionId = 1

            };
            await _conciergeRepository.AddAsync(concierge);

            RequestClient requestClient = new RequestClient
            {
                RequestId = request.RequestId,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                PhoneNumber = viewModel.PhoneNumber,
                RegionId = 1,
                Street = viewModel.Street,
                City = viewModel.City,
                State = viewModel.State,
                ZipCode = viewModel.ZipCode,
                Notes = viewModel.Symptoms,
                Email = viewModel.Email,
                DateOfBirth = viewModel.DateOfBirth
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
                return "PatientSite";

            }

            return "RegisterdPatientLogin";


        }
        #endregion

        #region check email

        public async Task<AspNetUser> CheckEmail(string email)
        {
            return await _aspnetuserRepository.CheckUserByEmail(email);
           
        }
        #endregion

        public async Task<AspNetUser> checkEmailPassword(AspNetUser user)
        {

            return await _aspnetuserRepository.Login(user.Email, user.PasswordHash); ;
        }
        public string GetUser(string email)
        {
            string s= await _userRepository.CheckUserByEmail(email);
            return s;
            
        }

    }
}
