using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class UserAccessViewModel
    {
        public List<Admin> admins {  get; set; }
        public List<Physician> physician { get; set; }
        public int? AccountTypeId { get; set; }
       
        public List<Status> status { get; set; }

    }
}
