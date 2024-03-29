using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class AccountAccessViewModel
    {
        public string AdminName { get; set; }


        public int type { get; set; }


        public int roleinput { get; set; }


        public List<Role> roles { get; set; }


        public List<Menu> MenuNames { get; set; }


        public List<RoleMenu> rolemenus { get; set; }


        public List<int> RoleId { get; set; }


        [Required(ErrorMessage = " Please enter a Role Name.")]
        public string RoleName { get; set; }


        [Required(ErrorMessage = "Please select a account type.")]
        public short? accountTypeId { get; set; }
    }
}
