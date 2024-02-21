using HalloDoc.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Services.IServices
{
    public interface IPatientService
    {
       //public Task<object> PatientRequest(int? userId);

        Task<string> PatientRequest(PatientRequestViewModel viewModel);
    }
}
