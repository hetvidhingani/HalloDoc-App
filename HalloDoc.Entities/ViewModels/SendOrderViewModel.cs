using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class SendOrderViewModel
    {
        public List<HealthProfessionalType> Profession { get; set;}
        public List<HealthProfessional> Business { get; set;}
        public string Contact { get; set;}
        public string Email { get; set;}
        public string? FaxNumber { get; set; }
        public string OrderDetails { get; set; }
        public int? Refill { get; set; }
        public int RequestID { get; set; }
        public int RequestClientID { get; set; }
        public int professionId { get; set; }
        public int BusinessID { get; set; }




    }
}
