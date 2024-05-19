using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.Models
{
    public class ChatViewModel
    {
        public int AdminId { get; set; }

        public int ProviderId { get; set; }

        public int RequestId { get; set; }

        public string? Message { get; set; }

        public string? ChatDate { get; set; }

        public int SentBy { get; set; }

        public string? ChatBoxClass { get; set; }

        public string? RecieverName { get; set; }

        public List<ChatViewModel> Chats { get; set; }
    }

    public class ChatDetailsViewModel
    {
        public string? RecieverName { get; set; }

        public string? SenderId { get; set; }
        public string? RecieverId { get; set; }

        public List<ChatDetailsTableModel> Chats { get; set; }
    }

    public class ChatDetailsTableModel
    {
        public string? ChatBoxClass { get; set; }

        public string? ChatDate { get; set; }

        public string? Message { get; set; }

    }

    public class ChatModel
    {
        public string SenderId { get; set; }
        public string RecieverId { get; set; }

        public string Message { get; set; }

    }
}
