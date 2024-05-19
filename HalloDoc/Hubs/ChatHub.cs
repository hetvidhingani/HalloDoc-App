using HalloDoc.Entities.DataModels;
using HalloDoc.Entities.ViewModels;
using HalloDoc.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;

namespace HalloDoc.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IJwtService jwtServices;
        private readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
        private static Dictionary<string, List<string>> userConnectionMap = new Dictionary<string, List<string>>();
        private static string userConnectionMapLocker = string.Empty;
        //public async Task SendMessage(string userId, string message)
        //{
        //    var username = Context.ConnectionId;
        //    await Clients.All.SendAsync("ReceiveMessage", userId, message);
        //}
        public override async Task OnConnectedAsync()
        {
            string AspnetUserID = jwtServices.GetTokenData(_httpContextAccessor.HttpContext.Request.Cookies["jwt"]).AspNetUserId;
            if (!AspnetUserID.IsNullOrEmpty())
            {
                KeepUserConnection(Context.ConnectionId);
            }
            await base.OnConnectedAsync();
        }

        public static List<string> GetUserConnections(string aspNetUserId)
        {
            var conn = new List<string>();
            lock (userConnectionMapLocker)
            {
                if (userConnectionMap.ContainsKey(aspNetUserId))
                {
                    conn = userConnectionMap[aspNetUserId];
                }
            }
            return conn;
        }

        public void KeepUserConnection(string connectionId)
        {
            lock (userConnectionMapLocker)
            {
                string? aspNetUserId = jwtServices.GetTokenData(_httpContextAccessor.HttpContext.Request.Cookies["jwt"]).AspNetUserId;
                if (!userConnectionMap.ContainsKey(aspNetUserId))
                {
                    userConnectionMap[aspNetUserId] = new List<string>();
                }
                userConnectionMap[aspNetUserId].Add(connectionId);
            }
        }


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