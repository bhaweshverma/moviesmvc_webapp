using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace MoviesMVC.Hubs
{
    public class Chathub : Hub
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessageMethod", user, message);
        }
      
    }
}