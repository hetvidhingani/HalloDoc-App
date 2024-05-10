using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class ChatDetailsViewModel : Chat
    {
        public string? RecieverName {  get; set; }
        public List<ChatDetailsTableModel>? Chats { get; set; }
        public string? SenderId { get; set; }
        public string? RecieverId { get; set; }
    }

    public class ChatDetailsTableModel
    {
        public string? ChatBoxClass { get; set; }
        public string? Message { get; set; }
        //public string? ChatDate { get; set; }
       
    }

    public class ChatModel
    {
        public string? SenderId { get; set; }
        public string? RecieverId { get; set; }
        public string? Message { get; set; }

    }
}
