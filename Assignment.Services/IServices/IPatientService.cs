using Assignment.Entities.DataModels;
using Assignment.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Services.IServices
{
    public interface IPatientService
    {
        PatientDashboardViewModel PatientDetails(string PatientName, int CurrentPage,int Length);
        Patient AddPatientDetails(PatientDataViewModel model);
        PatientDataViewModel GetPatientData(int id);

        Patient EditPatientDetails(PatientDataViewModel model);
        void DeletePatient(int id);
        Doctor checkIfExist(string data);
    }
}
