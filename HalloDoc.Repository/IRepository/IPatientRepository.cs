using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalloDoc.Entities.Models;
using Microsoft.AspNetCore.Http;

namespace HalloDoc.Repository.IRepository
{
    public interface IPatientRepository
    {

        //Task<object> PatientRequest(int? userId);

        //Task<string> PatientRequest(PatientRequestViewModel viewModel);

       // Task<object> Dashboard(int? userId);

        Task<string> RegisterdPatientLogin(AspNetUser user);

    }
}
