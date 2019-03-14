using System;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.Jobs
{
    public class DiscordBotHub : Hub
    {
        //private UserManager<ApplicationUser> _userManager;
        //private ApplicationDbContext _context;

        //public DiscordBotHub(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        //{
        //    _userManager = userManager;
        //    _context = context; 
        //}

        //public override async Task OnConnectedAsync()
        //{
        //    // Get user
        //    var user = await _userManager.GetUserAsync(Context.User);

        //    var isAdmin = await _userManager.IsInRoleAsync(user, RoleNames.Admin);
        //    var isH2h = await _userManager.IsInRoleAsync(user, RoleNames.HappyToHelp);

        //    // Is user admin
        //    if (isAdmin)
        //    {
        //        await Groups.AddToGroupAsync(Context.ConnectionId, RoleNames.Admin);
        //    }

        //    // Is user h2h member
        //    if (isH2h)
        //    {
        //        await Groups.AddToGroupAsync(Context.ConnectionId, RoleNames.HappyToHelp);
        //    }

        //    // Get tickets for users

        //    if (!isAdmin && !isH2h && user.DiscordUserId.HasValue)
        //    {
        //        var tickets = await _context.Tickets
        //                .Where(x => x.ApplicantId == user.DiscordUserId.Value)
        //               .ToArrayAsync();

        //        foreach (var ticket in tickets)
        //        {
        //            await Groups.AddToGroupAsync(Context.ConnectionId, $"Ticket{ticket.Id}");
        //        }
        //    }
            
        //    await base.OnConnectedAsync();
        //}

        //public override Task OnDisconnectedAsync(Exception exception)
        //{
        //    return base.OnDisconnectedAsync(exception);
        //}

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessage", message);
        }

        public Task SendSuggestionUpdate(Suggestion suggestion)
        {
            return Clients.All.SendAsync("SuggestionUpdate", suggestion);
        }
    }
}
