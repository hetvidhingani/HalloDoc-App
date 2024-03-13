using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class CloseCaseViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ConfirmationNumber { get; set; }
        public string RequestID { get; set; }
        public string FileName { get; set; }
        public string UploadDate { get; set; }
        public int RequstId { get; set; }
        public string? CreatedDate { get; set; }
  
     
        public string Username { get; set; }
      
       
        
        public int RequestClientID { get; set; }

        public int RequestWiseFileID { get; set; }
        public bool isdeleted { get; set; }
        public List<RequestWiseFile> RequestWiseFiles { get; set; }

    }
}
