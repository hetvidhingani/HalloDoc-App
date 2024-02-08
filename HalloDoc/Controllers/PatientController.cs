using HalloDoc.DataContext;
using HalloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using HalloDoc.Models;

namespace HalloDoc.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult PatientSite()
        {
            return View();
        }

        public IActionResult PatientForgotPassword()
        {
            return View();
        }
        public IActionResult PatientSubmitRequest()
        {
            return View();
        }

        public IActionResult FamilyFriendRequest()
        {
            return View();
        }
        public IActionResult ConciergeRequest()
        {
            return View();
        }
        public IActionResult BusinessRequest()
        {
            return View();
        }

        #region Create Account
        public IActionResult CreateAccountPatient()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult CreateAccountPatient(User user)
        //{

        //    return View();
        //}

        #endregion

        #region Login

        public IActionResult RegisterdPatientLogin()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("DashBoard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult RegisterdPatientLogin(AspNetUser user)
        {
            var myUser = _context.AspNetUsers.Where(x => x.Email == user.Email && x.PasswordHash == user.PasswordHash).FirstOrDefault();
            if (myUser != null)
            {
                HttpContext.Session.SetString("UserSession", myUser.Email);

                return RedirectToAction("DashBoard");
            }
            else
            {
                ViewBag.Message = "Login Failed!";
            }

            return View();
        }
        #endregion

        #region Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DashBoard()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("RegisterdPatientLogin");
            }
            return View();
        }
        #endregion

        #region PatientRequest
        public IActionResult PatientRequest()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PatientRequest(PatientRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                AspNetUser newaspNetUSer = new AspNetUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = viewModel.FirstName,
                    PhoneNumber = viewModel.PhoneNumber,
                    CreatedDate = DateTime.Now,
                    Email = viewModel.Email
                };

                var AspNewUID = newaspNetUSer.Id;
                _context.AspNetUsers.Add(newaspNetUSer);
                _context.SaveChanges();

                var AspNetUserIDFK = newaspNetUSer.Id;

                User user = new User
                {
                    Id = AspNetUserIDFK,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Email = viewModel.Email,
                    Mobile = viewModel.PhoneNumber,
                    Street = viewModel.Street,
                    City = viewModel.City,
                    State = viewModel.State,
                    ZipCode = viewModel.ZipCode,
                    DateOfBirth = viewModel.DateOfBirth,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    RegionId = 1
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                int UsertblId = user.UserId;
                Request request = new Request
                {
                    UserId = UsertblId,
                    RequestTypeId = 1,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    PhoneNumber = viewModel.PhoneNumber,
                    Email = viewModel.Email,
                    DocumentPath = viewModel.DocumentPath,
                    Symptoms = viewModel.Symptoms,
                    CreatedDate = DateTime.Now,

                    Status = 1
                };
                RequestWiseFile requestwisefile = new RequestWiseFile
                {
                    
                };
                _context.Requests.Add(request);
                _context.SaveChanges();
                return RedirectToAction("CreateAccountPatient");
            }
            return View(viewModel);
        }
        #endregion

        #region Family Freind Request
        [HttpPost]
        public async Task<IActionResult> FamilyFriendRequest(PatientRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                AspNetUser newaspNetUSer = new AspNetUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = viewModel.FirstName,
                    PhoneNumber = viewModel.PhoneNumber,
                    CreatedDate = DateTime.Now,
                    Email = viewModel.Email
                };

                var AspNewUID = newaspNetUSer.Id;
                _context.AspNetUsers.Add(newaspNetUSer);
                _context.SaveChanges();

                var AspNetUserIDFK = newaspNetUSer.Id;

                User user = new User
                {
                    Id = AspNetUserIDFK,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Email = viewModel.Email,
                    Mobile = viewModel.PhoneNumber,
                    Street = viewModel.Street,
                    City = viewModel.City,
                    State = viewModel.State,
                    ZipCode = viewModel.ZipCode,
                    DateOfBirth = viewModel.DateOfBirth,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    RegionId = 1
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                int UsertblId = user.UserId;
                Request request = new Request
                {
                    UserId = UsertblId,
                    RequestTypeId = 1,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    PhoneNumber = viewModel.PhoneNumber,
                    Email = viewModel.Email,
                    DocumentPath = viewModel.DocumentPath,
                    Symptoms = viewModel.Symptoms,
                    Status = 1
                };

                RequestClient requestClient = new RequestClient
                {
                    
                   
                };
                _context.Requests.Add(request);
                _context.SaveChanges();
                return RedirectToAction("CreateAccountPatient");
            }
            return View(viewModel);
        }
        #endregion

        #region Logout
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("RegisterdPatientLogin");
            }
            return View();
        }
        #endregion

    }
}
