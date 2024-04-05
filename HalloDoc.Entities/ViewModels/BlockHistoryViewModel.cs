using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class BlockHistoryViewModel
    {
        public List<BlockTable> pagingData {  get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public int TotalCount { get; set; }
        public int FirstItemIndex { get; set; }
        public int LastItemIndex { get; set; }

    }
    public class BlockTable
    {
        public int BlockId { get; set; }
        public string PatientName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
            
        public string Note { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string IsActive { get; set; }


    }
}
