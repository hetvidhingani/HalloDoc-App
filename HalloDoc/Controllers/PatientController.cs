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
            var uID = _context.Users.Where(x => x.Id == myUser.Id).FirstOrDefault();
            if (myUser != null)
            {
                HttpContext.Session.SetString("UserSession", myUser.Email);
                TempData["UserDeshboardid"] = uID.UserId;
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
            var data = TempData["UserDeshboardid"];
            Request requests = _context.Requests.Where(x=>x.UserId==Convert.ToInt32( data)).FirstOrDefault();
            var requestClient = _context.RequestClients.Where(x=>x.RequestId==requests.RequestId).ToList();

            return View(requestClient);
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
                _context.Requests.Add(request);
                _context.SaveChanges();



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
                    Email = viewModel.Email
                };
                _context.RequestClients.Add(requestClient);
                _context.SaveChanges();

                RequestWiseFile requestwisefile = new RequestWiseFile
                {
                    RequestId = request.RequestId,
                    FileName = viewModel.DocumentPath,
                    CreatedDate = DateTime.Now
                };
                _context.RequestWiseFiles.Add(requestwisefile);
                _context.SaveChanges();


                AspNetUser userExist = _context.AspNetUsers.Where(x => x.Email == viewModel.Email).FirstOrDefault();
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

                    _context.AspNetUsers.Add(newaspNetUSer);
                    _context.SaveChanges();
                    TempData["id"] = request.RequestId;
                    return RedirectToAction("CreateAccountPatient");

                }
                return RedirectToAction("RegisterdPatientLogin");
            }
            return View(viewModel);
        }

        #endregion


        #region Family Freind Request
        [HttpPost]
        public async Task<IActionResult> FamilyFriendRequest(FamilyFriendRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
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
                _context.Requests.Add(request);
                _context.SaveChanges();

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
                    Email = viewModel.Email
                };
                _context.RequestClients.Add(requestClient);
                _context.SaveChanges();

                RequestWiseFile requestwisefile = new RequestWiseFile
                {
                    RequestId = request.RequestId,
                    FileName = viewModel.DocumentPath,
                    CreatedDate = DateTime.Now
                };
                _context.RequestWiseFiles.Add(requestwisefile);
                _context.SaveChanges();


                AspNetUser userExist = _context.AspNetUsers.Where(x => x.Email == viewModel.Email).FirstOrDefault();
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

                    _context.AspNetUsers.Add(newaspNetUSer);
                    _context.SaveChanges();
                    TempData["id"] = request.RequestId;
                    return RedirectToAction("CreateAccountPatient" );

                }
                return RedirectToAction("RegisterdPatientLogin");
            }
            return View(viewModel);
        }





        #endregion

        #region Business Request
        [HttpPost]
        public async Task<IActionResult> BusinessRequest(BusinessRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
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
                _context.Requests.Add(request);
                _context.SaveChanges();

                Business business = new Business
                {
                    Name = viewModel.ClientProperty,
                    RegionId = 1,
                    PhoneNumber = viewModel.ClientPhoneNumber,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "Admin",
                    Status = 1
                };
                _context.Businesses.Add(business);
                _context.SaveChanges();
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
                    Email = viewModel.Email
                };
                _context.RequestClients.Add(requestClient);
                _context.SaveChanges();

                RequestWiseFile requestwisefile = new RequestWiseFile
                {
                    RequestId = request.RequestId,
                    
                    CreatedDate = DateTime.Now
                };
                _context.RequestWiseFiles.Add(requestwisefile);
                _context.SaveChanges();


                AspNetUser userExist = _context.AspNetUsers.Where(x => x.Email == viewModel.Email).FirstOrDefault();
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

                    _context.AspNetUsers.Add(newaspNetUSer);
                    _context.SaveChanges();
                    TempData["id"] = request.RequestId;
                    return RedirectToAction("CreateAccountPatient");

                }
                return RedirectToAction("RegisterdPatientLogin");
            }
            return View(viewModel);
        }



        #endregion

        #region Concierge Request
        [HttpPost]
        public async Task<IActionResult> ConciergeRequest(ConciergeRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
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
                _context.Requests.Add(request);
                _context.SaveChanges();

                Concierge concierge = new Concierge
                {
                    ConciergeName=viewModel.ClientFirstName + " " + viewModel.ClientLastName,
                    CreatedDate=DateTime.Now,
                    Street=viewModel.ClientStreet,
                    State=viewModel.ClientState,
                    City=viewModel.ClientCity,
                    ZipCode=viewModel.ClientZipCode,
                    
                    RegionId=1

                };
                _context.Concierges.Add(concierge);
                _context.SaveChanges();
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
                    Email = viewModel.Email
                };
                _context.RequestClients.Add(requestClient);
                _context.SaveChanges();

                RequestWiseFile requestwisefile = new RequestWiseFile
                {
                    RequestId = request.RequestId,

                    CreatedDate = DateTime.Now
                };
                _context.RequestWiseFiles.Add(requestwisefile);
                _context.SaveChanges();


                AspNetUser userExist = _context.AspNetUsers.Where(x => x.Email == viewModel.Email).FirstOrDefault();
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

                    _context.AspNetUsers.Add(newaspNetUSer);
                    _context.SaveChanges();
                    TempData["id"] = request.RequestId;
                    return RedirectToAction("CreateAccountPatient");

                }
                return RedirectToAction("RegisterdPatientLogin");
            }
            return View(viewModel);
        }



        #endregion


        #region Create Account1
        public IActionResult CreateAccountRequest()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAccountRequest(CreateAccountViewModel createAccountViewModel)
        {
            if (ModelState.IsValid)
            {
                //int id = Url.RouteUrl.;
                AspNetUser aspNetuser = _context.AspNetUsers.Where(s => s.Email == createAccountViewModel.Email).FirstOrDefault();
                if (createAccountViewModel.PasswordHash == createAccountViewModel.ConfirmPassword)
                {
                    if (aspNetuser != null)

                    {
                        aspNetuser.PasswordHash = createAccountViewModel.PasswordHash;
                        _context.AspNetUsers.Update(aspNetuser);
                        _context.SaveChanges();

                        await _context.SaveChangesAsync();
                  
                RequestClient requestClient = _context.RequestClients.Where(s => s.RequestId == createAccountViewModel.RequestID).FirstOrDefault();
                       
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
                            StrMonth = requestClient.StrMonth,
                            IntYear = requestClient.IntYear,
                            IntDate = requestClient.IntDate,
                            CreatedBy = "Admin",
                            CreatedDate = DateTime.Now,
                            RegionId = 1
                        };
                        _context.Users.Add(user);
                        _context.SaveChanges();

                        Request req = _context.Requests.Where(s => s.RequestId == createAccountViewModel.RequestID).FirstOrDefault();
                        req.UserId = user.UserId;
                        _context.SaveChanges();


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
