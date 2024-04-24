using Assignment.Entities.DataModels;
using Assignment.Entities.ViewModels;
using Assignment.Repository.IRepository;
using Assignment.Services.IServices;
using Humanizer;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Assignment.Services.Services
{
    public class PatientService : IPatientService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        public PatientService(IDoctorRepository doctorRepository, IPatientRepository patientRepository)
        {
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

        public PatientDashboardViewModel PatientDetails(string PatientName, int CurrentPage,int Length=5)
        {
            Expression<Func<Patient, bool>> tableData = PredicateBuilder.New<Patient>();

            tableData = x => true;
            if (!string.IsNullOrWhiteSpace(PatientName))
            {
                tableData = tableData.And(e => e.FirstName.ToLower().Contains(PatientName.ToLower()) || e.LastName.ToLower().Contains(PatientName.ToLower()));
            }

            List<Dashboard> vendorTable = new List<Dashboard>();

            var data = _patientRepository.GetAllWithPagination(x => new Dashboard
            {
                PatientId = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Age = x.Age,
                Disease = x.Disease,
                Doctor = x.Doctor.Specialist,
                Gender = x.Gender,
                PhoneNumber = x.PhoneNo,

            }, tableData, CurrentPage, Length, x => x.Id, true);

            foreach (Dashboard requiredData in data)
            {
                vendorTable.Add(requiredData);
            }

            if (CurrentPage == 0) { CurrentPage = 1; }
            int dataSize = 5;
            int totalCount = _patientRepository.GetTotalCount(tableData);
            int totalPage = (int)Math.Ceiling((double)totalCount / dataSize);
            int FirstItemIndex = Math.Min((CurrentPage - 1) * dataSize + 1, totalCount);
            int LastItemIndex = Math.Min(CurrentPage * dataSize, totalCount);

            return new PatientDashboardViewModel
            {
                PagingData = vendorTable,
                TotalCount = totalCount,
                TotalPages = totalPage,
                CurrentPage = CurrentPage,
                PageSize = 3,
                FirstItemIndex = FirstItemIndex,
                LastItemIndex = LastItemIndex,
            };
        }

        public Patient AddPatientDetails(PatientDataViewModel model)
        {
            Doctor doctor = _doctorRepository.GetAll().Where(x=>x.Specialist == model.Doctor).FirstOrDefault();
            if (doctor == null)
            {
                Doctor addDoctor = new Doctor();
                addDoctor.Specialist = model.Doctor;
                _doctorRepository.Add(addDoctor);
            }
           Doctor newAdded = _doctorRepository.GetAll().Where(x => x.Specialist == model.Doctor).FirstOrDefault();
            Patient newPatient = new Patient();
            
            newPatient.FirstName = model.FirstName;
            newPatient.LastName = model.LastName;
            newPatient.Email = model.Email;
            newPatient.PhoneNo = model.PhoneNumber;
            newPatient.Age = model.Age;
            newPatient.Gender = model.Gender;
            newPatient.Specialist = newAdded.Specialist;
            newPatient.Disease = model.Disease;
            newPatient.DoctorId = newAdded.Doctorid;

            _patientRepository.Add(newPatient);

            return newPatient;
        }
        public PatientDataViewModel GetPatientData(int id)
        {
            Patient patient = _patientRepository.GetById(id);
            PatientDataViewModel model = new PatientDataViewModel();
            model.PatientId = id;
            model.FirstName = patient.FirstName;
            model.LastName = patient.LastName;
            model.Email = patient.Email;
            model.PhoneNumber = patient.PhoneNo;
            model.Age = patient.Age;
            model.Gender = patient.Gender;
            model.Disease= patient.Disease;
            model.Doctor = patient.Specialist;
            return model;

        }
        public Patient EditPatientDetails(PatientDataViewModel model)
        {
            Doctor doctor = _doctorRepository.GetAll().Where(x => x.Specialist == model.Doctor).FirstOrDefault();
            if (doctor == null)
            {
                Doctor addDoctor = new Doctor();
                addDoctor.Specialist = model.Doctor;
                _doctorRepository.Add(addDoctor);
            }
            Doctor newAdded = _doctorRepository.GetAll().Where(x => x.Specialist == model.Doctor).FirstOrDefault();

            Patient newPatient = _patientRepository.GetById(model.PatientId);
            newPatient.FirstName = model.FirstName;
            newPatient.LastName = model.LastName;
            newPatient.Email = model.Email;
            newPatient.PhoneNo = model.PhoneNumber;
            newPatient.Age = model.Age;
            newPatient.Gender = model.Gender;
            newPatient.Specialist = newAdded.Specialist;
            newPatient.Disease = model.Disease;
            newPatient.DoctorId = newAdded.Doctorid;

            _patientRepository.Update(newPatient);
            return newPatient;
        }

        public void DeletePatient(int id)
        {
            Patient patient = _patientRepository.GetById(id);
            _patientRepository.Remove(patient);
        }

        public Doctor checkIfExist(string data)
        {
            Doctor doctor = _doctorRepository.GetAll().Where(x => x.Specialist == data).FirstOrDefault();
            if(doctor == null)
            {
                Doctor addDoctor = new Doctor();
                addDoctor.Specialist = data;
                _doctorRepository.Add(addDoctor);
            }
            return doctor;

        }
    }
}
