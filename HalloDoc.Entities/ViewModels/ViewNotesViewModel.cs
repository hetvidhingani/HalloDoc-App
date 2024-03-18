using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class ViewNotesViewModel
    {
        public int requestID { get; set; }
        public List<TransferNotesViewModel>? TransferNotes { get; set; }
        public class TransferNotesViewModel
        {
            public int RequestId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime CreatedDate { get; set; }
            public string PhysicianName { get; set; }
            public string Note { get; set; }
        }
        public string? AdditionalNotes { get; set; }

        public string? AdminNotes { get; set; }
        public string? PhysicianNotes { get; set; }

    }
}
