using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalloDoc.Entities.Models;
namespace HalloDoc.Repository.Repository
{
    public class PatientRepository:IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;

        }
       public string  PatientRequest(PatientRequestViewModel viewModel)
        {
            Request request = new Request
            {
                RequestTypeId = 1,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                PhoneNumber = viewModel.PhoneNumber,
                Email = viewModel.Email,
                CreatedDate = DateTime.Now,
                //RelationName = viewModel.RelationName,
                Status = 1
            };
            _context.Requests.Add(request);
            _context.SaveChanges();
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
            _context.RequestClients.Add(requestClient);
            _context.SaveChanges();



            if (viewModel.File != null && viewModel.File.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", viewModel.File.FileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    viewModel.File.CopyTo(stream);
                    var userCheck = _context.Requests.OrderBy(e => e.RequestId).LastOrDefault(e => e.Email == viewModel.Email);
                    RequestWiseFile newFile = new RequestWiseFile
                    {
                        RequestId = userCheck.RequestId,
                        FileName = viewModel.File.FileName,
                        CreatedDate = DateTime.Now,
                        DocType = 1,
                    };

                    _context.RequestWiseFiles.Add(newFile);
                    _context.SaveChanges();
                }
            }



            User user = _context.Users.Where(x => x.Email == viewModel.Email).FirstOrDefault();
            if (user != null)
            {
                request.UserId = user.UserId;
                _context.Requests.Update(request);
                _context.SaveChanges(true);

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

                _context.AspNetUsers.Add(newaspNetUSer);
                _context.SaveChanges();

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
                    //StrMonth = viewModel.DateOfBirth.Value.Month.ToString(),
                    //IntYear = viewModel.DateOfBirth.Value.Year,
                    //IntDate = viewModel.DateOfBirth.Value.Day,
                    DateOfBirth = viewModel.DateOfBirth,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    RegionId = 1
                };
                _context.Users.Add(user1);
                _context.SaveChanges();

                request.UserId = user1.UserId;
                _context.Requests.Update(request);
                _context.SaveChanges();


            }
              return "";
        }
        
    }
}
