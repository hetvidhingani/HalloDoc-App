using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class EmailLogViewModel
    {
        public List<Role> role {  get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string reciverName { get; set; }
         public string CreatedDate { get; set; }
        public string SentDate { get; set;}
        public int RequestId { get; set; }
        public List<EmailLog> EmailLogs { get; set; }
        public int emailLogId { get; set; }

    }
}
