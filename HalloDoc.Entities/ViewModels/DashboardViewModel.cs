using HalloDoc.Entities.DataModels;

namespace HalloDoc.Entities.ViewModels
{
    public class DashboardViewModel
    {
        public int RequstId { get; set; }
        public string? CreatedDate { get; set; }
        public int? Status { get; set; }
        public string? FileName { get; set; }
        public string Username { get; set; }
        public string LastName { get; set; }
        public int? FileCount { get; set; }
        public string FirstName { get; set; }
        public int RequestWiseFileID { get; set; }
        public bool isdeleted { get; set; }
        public List<RequestWiseFile> RequestWiseFiles { get; set; }
        public string Email { get; set; }


    }
}
