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

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public int AccountTypeId { get; set; }

        public int TotalCount { get; set; }

        public int FirstItemIndex { get; set; }

        public int LastItemIndex { get; set; }

        public List<TableAccountAccess>? PagingData { get; set; }
    }
    public class TableAccountAccess
    {
        public string AccountType { get; set; } =string.Empty;
        public string Name { get; set; } = string.Empty;
        public int roleID { get; set; }

    }

}
