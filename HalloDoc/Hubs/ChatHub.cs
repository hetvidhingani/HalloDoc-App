using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using HalloDoc.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HalloDoc.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IJwtService jwtServices;
        //public async Task SendMessage(string userId, string message)
        //{
        //    var username = Context.ConnectionId;
        //    await Clients.All.SendAsync("ReceiveMessage", userId, message);
        //}


        public ChatHub(IJwtService jwtServices)
        {
            this.jwtServices = jwtServices;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.Client(user).SendAsync("ReceiveMessage", user, message);
        }


    }
}