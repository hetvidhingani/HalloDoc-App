using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Assignment.Entities.ViewModel;
using Assignment.Services.IServices;
using Assignment.Entities.ViewModels;

namespace Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IPatientService _patient;

        public HomeController(ILogger<HomeController> logger,IPatientService patientService)
        {
            _logger = logger;
            _patient = patientService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PatientDetails(string PatientName, int CurrentPage = 1,int Length =5)
        {
            var result = _patient.PatientDetails(PatientName, CurrentPage,Length);
            return PartialView("_TablePartialView",result);

        }
        public IActionResult AddPatient()
        {
            return PartialView("_AddPatient");
        }
        public IActionResult AddPatientDetails(PatientDataViewModel model)
        {
            var result = _patient.AddPatientDetails(model);
            return RedirectToAction("Index", result);
        }
        public IActionResult EditPatient(int id)
        {
            var result = _patient.GetPatientData(id);
            return PartialView("_EditPatient",result);
        }
        public IActionResult EditPatientDetails(PatientDataViewModel model)
        {
            var result = _patient.EditPatientDetails(model);
            return RedirectToAction("Index", result);
        }
        public IActionResult DeletePatient(int id)
        {
            _patient.DeletePatient(id);
            return RedirectToAction("Index");
        }

        public IActionResult checkIfExist(string data)
        {
            var result = _patient.checkIfExist(data);
            return Json(result);
        }
    }
}