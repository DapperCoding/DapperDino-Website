using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDino.DAL
{
    public static class Constants
    {
        
    }

    public static class RoleNames
    {
        public const string Admin = "Administrator";
        public const string User = "User";
        public const string HappyToHelp = "HappyToHelp";
        public const string DiscordRecruiter = "discord_recruiter";
    }

    public static class ApplicationFormTypeNames
    {
        public const string Architect = "architect";
        public const string Recruiter = "recruiter";
        public const string Teacher = "teacher";
    }

    public static class FormTypeNames
    {
        public const string CustomBot = "custombot";
    }

    public static class FormStatusUpdateNames
    {
        public const string Accepted = "accepted";
        public const string Rejected = "rejected";
        public const string JoinConversation = "joinconversation";
        public const string Reply = "reply";
        public const string ExitConversation = "exitconversation";
    }

    public static class CustomBotFormStatusUpdateNames
    {
        public const string Abandoned = "abandoned";
        public const string Deal = "deal";
        public const string Done = "done";
        public const string InProgress = "inprogress";
        public const string NoDeal = "nodeal";
        public const string NotLookedAt = "notlookedat";
        public const string TalkingTo = "talkingto";
    }
}
