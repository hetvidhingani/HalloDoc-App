using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.Models;
using HalloDoc.Repository.IRepository;
using HalloDoc.Repository.Repository;
using HalloDoc.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Services.Services
{
    public class PatientService:IPatientService
    {
        private readonly IAspNetUserRepository _aspnetuserRepository;
        private readonly IUserRepository _userRepository;
       
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestClientRepository _requestclientRepository;
        private readonly IRequestWiseFilesRepository _requestwisefileRepository;

        public PatientService(IUserRepository userRepository, IAspNetUserRepository aspnetuserRepository,
            IRequestRepository requestRepository, IRequestClientRepository requestclientRepository, IRequestWiseFilesRepository requestwisefileRepository) 
        {
            _userRepository = userRepository;
            _aspnetuserRepository = aspnetuserRepository;
            _requestRepository = requestRepository;
            _requestclientRepository = requestclientRepository;
            _requestwisefileRepository = requestwisefileRepository;
        }

        #region PatientRequest
        //public async Task<object> PatientRequest(int? userId)
        //{

        //    if (userId != null)
        //    {
        //        User user =await _userRepository.GetUser(userId);
        //        PatientRequestViewModel viewModel = new PatientRequestViewModel();
        //        if (user != null)
        //        {
        //            viewModel.FirstName = user.FirstName;
        //            viewModel.LastName = user.LastName;
        //            viewModel.City = user.City;
        //            viewModel.State = user.State;
        //            viewModel.Street = user.Street;
        //            viewModel.Email = user.Email;
        //            viewModel.PhoneNumber = user.Mobile;
        //            viewModel.ZipCode = user.ZipCode;
        //            viewModel.DateOfBirth = user.DateOfBirth;

        //            return viewModel;
        //        }
        //    }
        //    return "PatientRequest";
        //}
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
            await _requestRepository.AddRequest (request);

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
            await _requestclientRepository.AddRequest(requestClient);

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

                    await _requestwisefileRepository.AddRequestWiseFiles(newFile);
                }
            }

            User user = await _userRepository.CheckUserByEmail(viewModel.Email);
            if (user != null)
            {
                request.UserId = user.UserId;
                await _requestRepository.UpdateRequest(request);
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

                await _aspnetuserRepository.AddAspnetUser(newaspNetUSer);

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
                await _userRepository.AddUser(user1);

                request.UserId = user1.UserId;
               await _requestRepository.UpdateRequest(request);

            }

            return "RegisterdPatientLogin";
        }


        #endregion

    }
}
