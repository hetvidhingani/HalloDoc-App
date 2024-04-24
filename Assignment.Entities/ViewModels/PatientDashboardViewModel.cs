using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Entities.ViewModels
{
    public class PatientDashboardViewModel
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public int TotalCount { get; set; }
        public List<Dashboard> PagingData { get; set; }
        public int FirstItemIndex { get; set; }
        public int LastItemIndex { get; set; }

    }
    public class Dashboard
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public int Gender { get; set; }
        public string Disease { get; set; }
        public string Doctor { get; set; }

    }
}
