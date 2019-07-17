using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class UserActionLog
    {
        public int DiscordUserId { get; set; }
        public Guid ApplicationUserId { get; set; }
        public string Before { get; set; }
        public string After { get; set; }
        public string ForDescription { get; set; }
        public string ForValue { get; set; }
        public UserAction Action { get; set; }
    }
    public enum UserAction
    {
        Login = 0,
        ForgotPassword = 1,
        Register = 2,
        ConnectDiscordAccount = 3,
        ApplyForHappyToHelp =4,
        DenyApplicationForHappyToHelp = 5,
        AcceptApplicationForHappyToHelp = 6,
        ConsiderApplicationForHappyToHelp = 7,
        AddTicketReaction = 8,
        CreateTicket = 9,
        RemoveTicketReaction = 10,
        CloseTicket = 11,
        RequestContact = 12,
        AddFaq = 13,
        RemoveFaq = 14,
        HostingEnquiry = 15
    }
}