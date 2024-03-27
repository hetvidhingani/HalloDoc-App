using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class ProviderInfoViewModel
    {
        [Required(ErrorMessage = "Enter First Name Here")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter Last Name Here")]
        public string LastName { get; set; }
        public int RoleId { get; set; }
        public List<Role> Role { get; set; }
        public int? RegionId { get; set; }
        public List<Region>? Regions { get; set; }
        public int PhysicianId { get; set; }
        public List<Physician> physicians { get; set; }
        public List<ProviderInfoViewModel> Providers { get; set; }
        public List<PhysicianNotification> PhysicianNotifications { get; set; }

    }
}
