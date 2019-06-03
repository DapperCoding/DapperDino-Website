using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Core.Discord;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Core.Discord;
using DSharpPlus.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.Areas.Admin.Controllers
{
    [Route("Admin/SyncMessages")]
    public class SyncDiscordMessages : BaseController
    {
        private ApplicationDbContext _context;

        public SyncDiscordMessages(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            var tickets = _context.Tickets.Select(x => x.Id).ToList();

            using (Bot bot = new Bot())
            {
                bot.RunAsync();

                var client = bot.GetClient();
                var guild = await client.GetGuildAsync(446710084156915722);
                var index = 0;
                await guild.GetChannelsAsync();

                foreach (var ticketId in tickets)
                {
                    try
                    {

                       
                        var channel = guild.Channels.FirstOrDefault(x => x.Name.ToLower() == $"ticket{ticketId}");

                        if (channel == null) continue;

                        var messages = await channel.GetMessagesAsync();

                        foreach (var message in messages)
                        {
                            var dbMessage = _context.DiscordMessages.Include(x=>x.Embeds).Include(x=>x.Attachments).FirstOrDefault(x => x.MessageId == message.Id.ToString());

                            if (dbMessage == null)
                            {
                                dbMessage = new DAL.Models.DiscordMessage();

                                dbMessage.Timestamp = message.Timestamp.UtcDateTime;
                                dbMessage.ChannelId = message.ChannelId.ToString();
                                dbMessage.GuildId = guild.Id.ToString();
                                dbMessage.IsEmbed = message.Embeds.Any();
                                dbMessage.Message = message.Content;
                                dbMessage.MessageId = message.Id.ToString();

                                _context.Add(dbMessage);
                                _context.SaveChanges();
                            }

                            await UpdateMessage(dbMessage, message);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        Console.WriteLine($"index: ${index}");
                        index++;
                    }
                }



            }
            return View();
        }

        private async Task UpdateMessage(DAL.Models.DiscordMessage dbMessage, DSharpPlus.Entities.DiscordMessage discordMessage)
        {
            
            // Delete embeds from database
            if (dbMessage.Embeds != null && dbMessage.Embeds.Any())
            {
                foreach (var embed in dbMessage.Embeds)
                {
                    _context.Remove(embed);
                }
            }

            // Add embeds from bot
            if (discordMessage.Embeds != null && discordMessage.Embeds.Any())
            {
                if (dbMessage.Embeds == null) dbMessage.Embeds = new List<DAL.Models.DiscordEmbed>();

                foreach (var embed in discordMessage.Embeds)
                {
                    dbMessage.Embeds.Add(ConvertEmbed(embed));
                }
            }

            // Delete attachments from database
            if (dbMessage.Attachments != null && dbMessage.Attachments.Any())
            {
                foreach (var attachment in dbMessage.Attachments)
                {
                    _context.Remove(attachment);
                }
            }

            // Add attachments from bot
            if (discordMessage.Attachments != null && discordMessage.Attachments.Any())
            {
                dbMessage.Attachments = ConvertAttachments(discordMessage.Attachments);
            }

            _context.SaveChanges();
        }

        private DAL.Models.DiscordEmbed ConvertEmbed(DSharpPlus.Entities.DiscordEmbed discordEmbed)
        {
            var embed = new DAL.Models.DiscordEmbed();

            embed.Title = discordEmbed.Title;
            embed.Type = discordEmbed.Type;
            embed.Description = discordEmbed.Description;
            embed.Url = discordEmbed.Url?.AbsoluteUri ?? String.Empty;
            embed.Timestamp = discordEmbed.Timestamp;

            embed.Color = ConvertDiscordColor(discordEmbed.Color);

            if (discordEmbed.Author != null)
            {
                embed.Author = ConvertAuthor(discordEmbed);
            }

            if (discordEmbed.Fields != null && discordEmbed.Fields.Any())
            {
                embed.Fields = ConvertFields(discordEmbed);
            }

            if (discordEmbed.Footer != null)
            {
                embed.Footer = ConvertFooter(discordEmbed.Footer);
            }

            if (discordEmbed.Provider != null)
            {
                embed.Provider = ConvertProvider(discordEmbed.Provider);
            }

            if (discordEmbed.Thumbnail != null)
            {
                embed.Thumbnail = ConvertThumbnail(discordEmbed.Thumbnail);
            }

            if(discordEmbed.Image != null)
            {
                embed.Image = ConvertImage(discordEmbed.Image);
            }

            if (discordEmbed.Video != null)
            {
                embed.Video = ConvertVideo(discordEmbed.Video);
            }

            return embed;
        }

        private DAL.Models.DiscordEmbedVideo ConvertVideo(DSharpPlus.Entities.DiscordEmbedVideo discordVideo)
        {
            var video = new DAL.Models.DiscordEmbedVideo();

            video.Height = discordVideo.Height;
            video.Url = discordVideo.Url?.AbsoluteUri ?? String.Empty;
            video.Width = discordVideo.Width;

            return video;
        }

        private DAL.Models.DiscordEmbedImage ConvertImage(DSharpPlus.Entities.DiscordEmbedImage discordImage)
        {
            var image = new DAL.Models.DiscordEmbedImage();

            image.Height = discordImage.Height;
            image.ProxyUrl = discordImage.ProxyUrl?.AbsoluteUri ?? String.Empty;
            image.Url = discordImage.Url?.AbsoluteUri ?? String.Empty;
            image.Width = discordImage.Width;

            return image;
        }

        private DAL.Models.DiscordEmbedThumbnail ConvertThumbnail(DSharpPlus.Entities.DiscordEmbedThumbnail discordThumbnail)
        {
            var thumbnail = new DAL.Models.DiscordEmbedThumbnail();

            thumbnail.Height = discordThumbnail.Height;
            thumbnail.Width = discordThumbnail.Width;
            thumbnail.Url = discordThumbnail.Url?.AbsoluteUri ?? String.Empty;
            thumbnail.ProxyUrl = discordThumbnail.ProxyUrl?.AbsoluteUri ?? String.Empty;

            return thumbnail;
        }

        private DAL.Models.DiscordEmbedProvider ConvertProvider(DSharpPlus.Entities.DiscordEmbedProvider discordProvider)
        {
            var provider = new DAL.Models.DiscordEmbedProvider();

            provider.Name = discordProvider.Name;
            provider.Url = discordProvider.Url?.AbsoluteUri ?? String.Empty;

            return provider;
        }

        private DAL.Models.DiscordEmbedFooter ConvertFooter(DSharpPlus.Entities.DiscordEmbedFooter discordFooter)
        {
            var footer = new DAL.Models.DiscordEmbedFooter();

            footer.IconUrl = discordFooter.IconUrl?.AbsoluteUri ?? String.Empty;
            footer.ProxyIconUrl = discordFooter.ProxyIconUrl?.AbsoluteUri ?? String.Empty;
            footer.Text = discordFooter.Text;

            return footer;
        }

        private DAL.Models.DiscordColor ConvertDiscordColor(DSharpPlus.Entities.DiscordColor discordColor)
        {
            var color = new DAL.Models.DiscordColor();

            color.R = discordColor.R;
            color.G = discordColor.G;
            color.B = discordColor.B;
            color.Value = discordColor.Value;

            return color;
        }

        private List<DAL.Models.DiscordAttachment> ConvertAttachments(IReadOnlyList<DSharpPlus.Entities.DiscordAttachment> attachments)
        {
            var list = new List<DAL.Models.DiscordAttachment>();


            foreach (var attachment in attachments)
            {
                list.Add(ConvertAttachment(attachment));
            }


            return list;
        }

        private DAL.Models.DiscordAttachment ConvertAttachment(DSharpPlus.Entities.DiscordAttachment discordAttachment)
        {
            var attachment = new DAL.Models.DiscordAttachment();

            attachment.DiscordAttachmentId = discordAttachment.Id.ToString();
            attachment.FileName = discordAttachment.FileName;
            attachment.FileSize = discordAttachment.FileSize;
            attachment.Height = discordAttachment.Height;
            attachment.Width = discordAttachment.Width;
            attachment.Url = discordAttachment.Url;
            attachment.ProxyUrl = discordAttachment.ProxyUrl;

            return attachment;
        }

        private DAL.Models.DiscordEmbedAuthor ConvertAuthor(DSharpPlus.Entities.DiscordEmbed discordEmbed)
        {
            var author = new DAL.Models.DiscordEmbedAuthor();

            author.IconUrl = discordEmbed.Author.IconUrl?.AbsoluteUri ?? String.Empty;
            author.Name = discordEmbed.Author.Name;
            author.ProxyIconUrl = discordEmbed.Author.ProxyIconUrl?.AbsoluteUri ?? String.Empty;
            author.Url = discordEmbed.Author.Url?.AbsoluteUri ?? String.Empty;

            return author;
        }


        private List<DAL.Models.DiscordEmbedField> ConvertFields(DSharpPlus.Entities.DiscordEmbed discordEmbed)
        {
            var fields = new List<DAL.Models.DiscordEmbedField>();

            foreach (var field in discordEmbed.Fields)
            {
                fields.Add(ConvertField(field));
            }

            return fields;
        }

        private DAL.Models.DiscordEmbedField ConvertField(DSharpPlus.Entities.DiscordEmbedField embedField)
        {
            var field = new DAL.Models.DiscordEmbedField();

            field.Inline = embedField.Inline;
            field.Name = embedField.Name;
            field.Value = embedField.Value;

            return field;
        }
    }
}