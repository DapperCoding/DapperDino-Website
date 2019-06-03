using DapperDino.DAL.Models;
using DapperDino.Core.Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Areas.Api.Models;

namespace DapperDino.Helpers
{
    public class TicketDiscordHelper
    {

        public TicketDiscordHelper()
        {
        }


        public async Task<TicketUserInformation> GetTicketDiscordUser(Bot bot, string userId)
        {
            var user = new TicketUserInformation();

            var client = bot.GetClient();
            var guild = await client.GetGuildAsync(446710084156915722);
            var discordUser = await guild.GetMemberAsync(ulong.Parse(userId));

            user.Status = discordUser?.Presence.Status ?? DSharpPlus.Entities.UserStatus.Offline;
            user.Username = discordUser.Username;


            return user;
        }

        public async Task<TicketReactionUserInformation> GetTicketReactionDiscordUser(Bot bot, string userId)
        {
            var user = new TicketReactionUserInformation();

            var client = bot.GetClient();
            var guild = await client.GetGuildAsync(446710084156915722);
            var discordUser = await guild.GetMemberAsync(ulong.Parse(userId));

            user.AvatarUrl = discordUser.AvatarUrl ?? "";

            user.Status = discordUser.Presence?.Status ?? DSharpPlus.Entities.UserStatus.Offline;
            user.Username = discordUser.Username;
            user.DiscordId = discordUser.Id.ToString();

            return user;
        }

        public async Task<DSharpPlus.Entities.DiscordUser> GetDiscordUser(string userId)
        {
            DSharpPlus.Entities.DiscordUser user = null;

            using (Bot bot = new Bot())
            {
                bot.RunAsync();

                var client = bot.GetClient();
                var guild = await client.GetGuildAsync(446710084156915722);
                user = await guild.GetMemberAsync(ulong.Parse(userId));

            }

            return user;
        }

        public async Task<IReadOnlyList<DSharpPlus.Entities.DiscordMessage>> GetPageFromChannelBefore(string channelName, ulong? before)
        {
            var list = null as IReadOnlyList<DSharpPlus.Entities.DiscordMessage>;
            using (Bot bot = new Bot())
            {
                bot.RunAsync();

                var client = bot.GetClient();
                var guild = await client.GetGuildAsync(446710084156915722);
                var channel = guild.Channels.SingleOrDefault(x => x.Name.ToLower() == channelName.ToLower());

                list = await channel.GetMessagesAsync(25, before);



            }

            return list;
        }

        public async Task<IReadOnlyList<DSharpPlus.Entities.DiscordMessage>> GetPageFromChannelBefore(ulong channelId, ulong? before)
        {
            var list = null as IReadOnlyList<DSharpPlus.Entities.DiscordMessage>;
            using (Bot bot = new Bot())
            {
                bot.RunAsync();

                var client = bot.GetClient();
                var guild = await client.GetGuildAsync(446710084156915722);
                var channel = guild.Channels.SingleOrDefault(x => x.Id == channelId);

                list = await channel.GetMessagesAsync(25, before);


            }

            return list;
        }
    }
}
