using Microsoft.AspNetCore.SignalR;

namespace HalloDoc.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string userId, string message)
        {
            var username = Context.ConnectionId;
            await Clients.All.SendAsync("ReceiveMessage", userId, message);
        }
    
    }
}