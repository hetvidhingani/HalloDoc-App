using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class AdminDashboardViewModel
    {
       public int? RequstClientId { get; set; }
        public string PatientName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Requestor { get; set; }
        public DateTime RequestedDate { get; set; }
        public string PatientPhone { get; set; }
        public string? RequestorPhone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
      public string? PhysicianName { get; set; }
     public DateTime? DateOfService { get; set; }
       public string? Region { get; set; }



    }
}
