using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace DapperDino.Jobs
{
    public class DiscordBotHub:Hub
    {
        
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessage", message);
        }
    }
}
