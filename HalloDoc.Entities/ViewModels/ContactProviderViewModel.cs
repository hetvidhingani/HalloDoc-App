using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class ContactProviderViewModel
    {
        public int physicianId { get; set; }
        
        public string? email { get; set; }
        [Required(ErrorMessage ="Enter Message Here.")]
        public string? message { get; set; }
        public string? phone { get; set; }
    }
}
