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
                User userExist = _context.Users.Where(x => x.Email == viewModel.Email).FirstOrDefault();
                if (userExist == null)
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


                        CreatedDate = DateTime.Now,

                        Status = 1
                    };
                    _context.Requests.Add(request);
                    _context.SaveChanges();

                    int RequesttblID = request.RequestId;
                    RequestClient requestClient = new RequestClient
                    {
                        RequestId = RequesttblID,
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                        PhoneNumber = viewModel.PhoneNumber,
                        RegionId = 1,
                        Street = viewModel.Street,
                        City = viewModel.City,
                        State = viewModel.State,
                        ZipCode = viewModel.ZipCode,
                        Notes = viewModel.Symptoms
                    };
                    _context.RequestClients.Add(requestClient);
                    _context.SaveChanges();
                    int RequesttblID1 = request.RequestId;
                    RequestWiseFile requestwisefile = new RequestWiseFile
                    {
                        RequestId = RequesttblID1,
                        FileName = viewModel.DocumentPath,
                        CreatedDate = DateTime.Now
                    };
                    _context.RequestWiseFiles.Add(requestwisefile);
                    _context.SaveChanges();



                    return RedirectToAction("PatientSite");

                }
                else
                {
                    User user = _context.Users.Where(s => s.Email == viewModel.Email).FirstOrDefault();


                    Request request = new Request
                    {
                        UserId = user.UserId,
                        RequestTypeId = 1,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.Mobile,
                        Email = user.Email,


                        CreatedDate = DateTime.Now,

                        Status = 1
                    };
                    _context.Requests.Add(request);
                    _context.SaveChanges();

                    int RequesttblID = request.RequestId;
                    RequestClient requestClient = new RequestClient
                    {
                        RequestId = RequesttblID,
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                        PhoneNumber = viewModel.PhoneNumber,
                        RegionId = 1,
                        Street = viewModel.Street,
                        City = viewModel.City,
                        State = viewModel.State,
                        ZipCode = viewModel.ZipCode,
                        Notes = viewModel.Symptoms
                    };
                    _context.RequestClients.Add(requestClient);
                    _context.SaveChanges();

                    int RequesttblID1 = request.RequestId;
                    RequestWiseFile requestwisefile = new RequestWiseFile
                    {
                        RequestId = RequesttblID1,
                        FileName = viewModel.DocumentPath,
                        CreatedDate = DateTime.Now
                    };
                    _context.RequestWiseFiles.Add(requestwisefile);
                    _context.SaveChanges();



                    return RedirectToAction("RegisterdPatientLogin");
                }
            }
            return View(viewModel);
        }
        #endregion


        #region Create Account
        public IActionResult CreateAccountRequest()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAccountRequest(CreateAccountViewModel createAccountViewModel)
        {
            if (ModelState.IsValid)
            {

                AspNetUser aspNetuser = _context.AspNetUsers.Where(s => s.Email == createAccountViewModel.Email).FirstOrDefault();

                if (createAccountViewModel.PasswordHash == createAccountViewModel.ConfirmPassword)
                {
                    if (aspNetuser != null)

                    {
                        aspNetuser.PasswordHash = createAccountViewModel.PasswordHash;
                        _context.AspNetUsers.Update(aspNetuser);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("RegisterdPatientLogin");
                    }
                    else
                    {
                        TempData["errormsg"] = "Entered Email is wrong";
                        return RedirectToAction("CreateAccountPatient");
                    }
                }
                else
                {
                    TempData["errormsg1"] = "Password and Confirm Password should be same";
                    return View("CreateAccountPatient", createAccountViewModel);
                }
               
                return RedirectToAction("Dashboard");



             
            }
            return View(createAccountViewModel);
        }


        #endregion

        #region Family Freind Request
        [HttpPost]
        public async Task<IActionResult> FamilyFriendRequest(FamilyFriendRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                User userExist = _context.Users.Where(x => x.Email == viewModel.Email).FirstOrDefault();
                if (userExist == null)
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
                        FirstName = viewModel.ClientFirstName,
                        LastName = viewModel.ClientLastName,
                        PhoneNumber = viewModel.ClientPhoneNumber,
                        Email = viewModel.CLientEmail,
                        RelationName = viewModel.RelationName,

                        CreatedDate = DateTime.Now,

                        Status = 1
                    };
                    _context.Requests.Add(request);
                    _context.SaveChanges();

                    int RequesttblID = request.RequestId;
                    RequestClient requestClient = new RequestClient
                    {
                        RequestId = RequesttblID,
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                        PhoneNumber = viewModel.PhoneNumber,
                        RegionId = 1,
                        Street = viewModel.Street,
                        City = viewModel.City,
                        State = viewModel.State,
                        ZipCode = viewModel.ZipCode,
                        Notes = viewModel.Symptoms
                    };
                    _context.RequestClients.Add(requestClient);
                    _context.SaveChanges();
                    int RequesttblID1 = request.RequestId;
                    RequestWiseFile requestwisefile = new RequestWiseFile
                    {
                        RequestId = RequesttblID1,
                        FileName = viewModel.DocumentPath,
                        CreatedDate = DateTime.Now
                    };
                    _context.RequestWiseFiles.Add(requestwisefile);
                    _context.SaveChanges();



                    return RedirectToAction("CreateAccountPatient");

                }
                else
                {
                    User user = _context.Users.Where(s => s.Email == viewModel.Email).FirstOrDefault();


                    Request request = new Request
                    {
                        UserId = user.UserId,
                        RequestTypeId = 1,
                        FirstName = viewModel.ClientFirstName,
                        LastName = viewModel.ClientLastName,
                        PhoneNumber = viewModel.ClientPhoneNumber,
                        Email = viewModel.CLientEmail,


                        CreatedDate = DateTime.Now,

                        Status = 1
                    };
                    _context.Requests.Add(request);
                    _context.SaveChanges();

                    int RequesttblID = request.RequestId;
                    RequestClient requestClient = new RequestClient
                    {
                        RequestId = RequesttblID,
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                        PhoneNumber = viewModel.PhoneNumber,
                        RegionId = 1,
                        Street = viewModel.Street,
                        City = viewModel.City,
                        State = viewModel.State,
                        ZipCode = viewModel.ZipCode,
                        Notes = viewModel.Symptoms
                    };
                    _context.RequestClients.Add(requestClient);
                    _context.SaveChanges();

                    int RequesttblID1 = request.RequestId;
                    RequestWiseFile requestwisefile = new RequestWiseFile
                    {
                        RequestId = RequesttblID1,
                        FileName = viewModel.DocumentPath,
                        CreatedDate = DateTime.Now
                    };
                    _context.RequestWiseFiles.Add(requestwisefile);
                    _context.SaveChanges();



                    return RedirectToAction("Dashboard");
                }
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
