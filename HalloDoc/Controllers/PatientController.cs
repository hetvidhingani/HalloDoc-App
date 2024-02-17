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


        public IActionResult PatientSubmitRequest()
        {
            return View();
        }
        public IActionResult ReviewAgreement()
        {
            return View();
        }

        public IActionResult BusinessRequest()
        {
            return View();
        }


        public IActionResult CreateAccountPatient()
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

        #region check email
        [Route("/Patient/checkemail/{email}")]
        [HttpGet]
        public IActionResult CheckEmail(string email)
        {
            var emailExists = _context.AspNetUsers.Any(u => u.Email == email);
            return Json(new { exists = emailExists });
        }
        #endregion

        #region Login

        public IActionResult RegisterdPatientLogin()
        {
            if (HttpContext.Session.GetInt32("UserSession") != null)
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
                HttpContext.Session.SetString("UserName", myUser.UserName);
                User uID = _context.Users.Where(x => x.Email == myUser.Email).FirstOrDefault();
                HttpContext.Session.SetInt32("UserSession", uID.UserId);


                return RedirectToAction("DashBoard");
            }
            else
            {
                ViewBag.Message = "Login Failed!";
            }

            return View();
        }



        #endregion

        #region Forgot Password
        public IActionResult PatientForgotPassword()
        {
            return View();

        }
        [HttpPost]
        public IActionResult PatientForgotPassword(CreateAccountViewModel createAccountViewModel)
        {
            var myUser = _context.AspNetUsers.Where(x => x.Email == createAccountViewModel.Email).FirstOrDefault();

            if (myUser != null)
            {
                //ViewBag.Message = "Reset Password Link is sent to your registerd Email.";
                if (createAccountViewModel.PasswordHash == createAccountViewModel.ConfirmPassword)
                {
                    myUser.PasswordHash = createAccountViewModel.ConfirmPassword;
                    _context.AspNetUsers.Update(myUser);
                    _context.SaveChangesAsync();
                    ViewBag.Message = "Password Updated";

                }
                else
                {
                    ViewBag.Message = "Password and Confirm Password must be same!!!";

                }

            }
            else
            {
                ViewBag.Message = "Invalid User Name";
            }
            return View();
        }
        #endregion

        #region Dashboard
        public IActionResult Dashboard()
        {
            ViewBag.MySession = HttpContext.Session.GetString("UserName");
            int? userID = HttpContext.Session.GetInt32("UserSession");
            var tabledashboard =
                (
                    from r in _context.Requests
                    join rfile in _context.RequestWiseFiles

                     on r.RequestId equals rfile.RequestId into files from file in files.DefaultIfEmpty()
                    where r.UserId == userID
                    select new
                    {
                        r.RequestId,
                        r.CreatedDate,
                        r.Status,
                        FileName=file!=null?file.FileName:null
                    }

                ).ToList();
            List<DashboardViewModel> list = new List<DashboardViewModel>();
            foreach (var r in tabledashboard)
            {
                DashboardViewModel dash = new DashboardViewModel();
                dash.RequstId = r.RequestId;
                dash.CreatedDate = r.CreatedDate.ToShortDateString();
                dash.Status = r.Status;
                dash.FileName = r.FileName;
                list.Add(dash);
            }


            return View(list);

        }
        [HttpPost]
        public IActionResult DashBoard()
        {
            if (HttpContext.Session.GetInt32("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetInt32("UserSession");
            }
            else
            {
                return RedirectToAction("RegisterdPatientLogin");
            }
            return View();
        }


        #endregion

        #region create new request
        public IActionResult SubmitInformationSomeoneElse()
        {
            return View();

        }
        [HttpPost]
        public IActionResult SubmitInformationSomeoneElse(PatientRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                 int ? userId = HttpContext.Session.GetInt32("UserSession");

                User userExist = _context.Users.Where(x=>x.UserId == userId).FirstOrDefault();   
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
                        StrMonth = viewModel.DateOfBirth.Value.Month.ToString(),
                        IntYear = viewModel.DateOfBirth.Value.Year,
                        IntDate = viewModel.DateOfBirth.Value.Day,

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

                return RedirectToAction("Dashboard", viewModel);
            }
            return View();
        }
     
        #endregion


        #region View Document
        [HttpPost]
        public IActionResult ViewDocument(IFormFile a, int Id)
        {
            //int? requestid = HttpContext.Session.GetInt32("request_id");
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
                };
                _context.RequestWiseFiles.Add(newRequestWiseFile);
                _context.SaveChanges();
                return RedirectToAction("ViewDocument");
            }
            return View("ViewDocument");
        }

        public IActionResult ViewDocument(int Id)
        {
            ViewBag.MySession = HttpContext.Session.GetString("UserName");

            var tableData = (from r in _context.Requests
                             join rwf in _context.RequestWiseFiles
                             on r.RequestId equals rwf.RequestId
                             where r.RequestId == Id
                             //where r.UserId == userid 
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
                model.CreatedDate = e.CreatedDate.ToShortDateString();
                model.FileName = e.FileName;
                model.Status = e.Status;
                list.Add(model);
            }

            return View(list);
        }
        public FileResult DownloadFile(string name, string filename)
        {
            RequestWiseFile reqw = _context.RequestWiseFiles.Where(x => x.FileName == name).FirstOrDefault();
            if (reqw != null)
            {
                filename = reqw.FileName;
            }

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads", filename);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }
        #endregion

        #region Patient My Profile
        public IActionResult Profile(PatientRequestViewModel requestViewModel)
        {
            int? userId = HttpContext.Session.GetInt32("UserSession");
            var user = _context.Users.Where(x => x.UserId == userId).FirstOrDefault();
            if (user != null)
            {
                requestViewModel.FirstName = user.FirstName;
                requestViewModel.LastName = user.LastName;
                requestViewModel.City = user.City;
                requestViewModel.State = user.State;
                requestViewModel.Street = user.Street;
                requestViewModel.Email = user.Email;
                requestViewModel.PhoneNumber = user.Mobile;
                requestViewModel.ZipCode = user.ZipCode;
                string storedStrMonth = user.StrMonth;
                int? storedIntYear = user.IntYear;
                int? storedIntDate = user.IntDate;
                //        DateOnly storedDob = new DateOnly((int)storedIntYear, int.Parse(storedStrMonth), (int)storedIntDate);

                //      requestViewModel.DateOfBirth = storedDob;

                //requestViewModel.PDob = dob;
                return View(requestViewModel);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(PatientRequestViewModel patientRequestViewModel)
        {
            int? userId = HttpContext.Session.GetInt32("UserSession");
            var user = _context.Users.Where(x => x.UserId == userId).FirstOrDefault();
            if (user != null)
            {
                user.FirstName = patientRequestViewModel.FirstName;
                user.LastName = patientRequestViewModel.LastName;
                user.Email = patientRequestViewModel.Email;
                user.Street = patientRequestViewModel.Street;
                user.City = patientRequestViewModel.City;
                user.Mobile = patientRequestViewModel.PhoneNumber;
                user.State = patientRequestViewModel.State;
                user.ZipCode = patientRequestViewModel.ZipCode;

                user.StrMonth = patientRequestViewModel.DateOfBirth.Value.ToString();
                user.IntDate = patientRequestViewModel.DateOfBirth.Value.Day;
                user.IntYear = patientRequestViewModel.DateOfBirth.Value.Year;
                user.ModifiedDate = DateTime.Now;




                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return View("Profile");
            }
            return View("Profile");
        }

        #endregion

        #region PatientRequest
        [HttpGet]
        public IActionResult PatientRequest()
        {
            int? userId = HttpContext.Session.GetInt32("UserSession");
            if ( userId != null)
            {
                var user = _context.Users.Where(x => x.UserId == userId).FirstOrDefault();
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
                    viewModel.DateOfBirth=user.DateOfBirth;
                   
                    return View(viewModel);
                }
            }


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
                    Email = viewModel.Email

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
                        StrMonth = viewModel.DateOfBirth.Value.Month.ToString(),
                        IntYear = viewModel.DateOfBirth.Value.Year,
                        IntDate = viewModel.DateOfBirth.Value.Day,

                        CreatedBy = "Admin",
                        CreatedDate = DateTime.Now,
                        RegionId = 1
                    };
                    _context.Users.Add(user1);
                    _context.SaveChanges();

                    request.UserId = user1.UserId;
                    _context.Requests.Update(request);
                    _context.SaveChanges();

                    return RedirectToAction("PatientSite", viewModel);

                }

                return RedirectToAction("RegisterdPatientLogin", viewModel);
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

                if (viewModel.File != null && viewModel.File.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", viewModel.File.FileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        viewModel.File.CopyTo(stream);
                        //var userCheck = _context.Requests.OrderBy(e => e.RequestId).LastOrDefault(e => e.Email == viewModel.Email);
                        RequestWiseFile newFile = new RequestWiseFile
                        {
                            RequestId = request.RequestId,
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
                    //TempData["id"] = request.RequestId;
                    return RedirectToAction("CreateAccountPatient");

                }
                return RedirectToAction("RegisterdPatientLogin");
            }
            return View(viewModel);
        }

        #endregion

        #region Business Request
        [HttpPost]
        public async Task<IActionResult> BusinessRequest(FamilyFriendRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
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

                User user = _context.Users.Where(x => x.Email == viewModel.Email).FirstOrDefault();
                if (user != null)
                {
                    request.UserId = user.UserId;
                    _context.Requests.Update(request);
                    _context.SaveChanges(true);

                }
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
                    return RedirectToAction("CreateAccountPatient");

                }
                return RedirectToAction("RegisterdPatientLogin");
            }
            return View(viewModel);
        }



        #endregion

        #region Concierge Request
        [HttpPost]
        public async Task<IActionResult> ConciergeRequest(FamilyFriendRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
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
                _context.Requests.Add(request);
                _context.SaveChanges();

                Concierge concierge = new Concierge
                {
                    ConciergeName = viewModel.ClientFirstName + " " + viewModel.ClientLastName,
                    CreatedDate = DateTime.Now,
                    Street = viewModel.ClientStreet,
                    State = viewModel.ClientState,
                    City = viewModel.ClientCity,
                    ZipCode = viewModel.ClientZipCode,

                    RegionId = 1

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
                User user = _context.Users.Where(x => x.Email == viewModel.Email).FirstOrDefault();
                if (user != null)
                {
                    request.UserId = user.UserId;
                    _context.Requests.Update(request);
                    _context.SaveChanges(true);

                }

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
                    return RedirectToAction("CreateAccountPatient");

                }
                return RedirectToAction("RegisterdPatientLogin");
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

                        RequestClient requestClient = _context.RequestClients.Where(s => s.Email == createAccountViewModel.Email).FirstOrDefault();

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

                        Request req = _context.Requests.Where(s => s.RequestId == requestClient.RequestId).FirstOrDefault();
                        req.UserId = user.UserId;
                        _context.Requests.Update(req);
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
