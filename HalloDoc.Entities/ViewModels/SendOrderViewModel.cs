using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class SendOrderViewModel
    {
        [Required(ErrorMessage = "Please Select Profession !")]
        public int professionId { get; set; }

        [Required(ErrorMessage = "Please Select Business!")]
        public int BusinessID { get; set; }

        [Required(ErrorMessage = "Please Provide Phone Number !")]
        public string Contact { get; set; }   

        [Required(ErrorMessage = "Please Provide Email !")]
        public string Email { get; set; }

        public string? FaxNumber { get; set; }
        [Required(ErrorMessage = "Please Provide Order Details !")]
        public string OrderDetails { get; set; }
        public int? Refill { get; set; }
        public int RequestID { get; set; }
        public int RequestClientID { get; set; }

       
        public List<HealthProfessionalType>? Profession { get; set;}

        public List<HealthProfessional>? Business { get; set;}



    }
}
