﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class AdminMyProfileViewModel
    {
        public string UserName { get; set; }
        public string Status { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AdminID { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public List<Region> State { get; set; }
        public string Zip { get; set; }
        public string Address2 { get; set; }
        public string BillingPhoneNumber { get; set; }
        public int RegionId { get; set; }
        public string ConfirmEmail { get; set; }
    }
}
