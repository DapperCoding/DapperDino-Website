
using DapperDino.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DapperDino.Core
{
    public class DiscordEmbedHelper
    {
        public DiscordEmbed ConvertEmbed(DSharpPlus.Entities.DiscordEmbed discordEmbed)
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

            if (discordEmbed.Image != null)
            {
                embed.Image = ConvertImage(discordEmbed.Image);
            }

            if (discordEmbed.Video != null)
            {
                embed.Video = ConvertVideo(discordEmbed.Video);
            }

            return embed;
        }

        public DiscordEmbedVideo ConvertVideo(DSharpPlus.Entities.DiscordEmbedVideo discordVideo)
        {
            var video = new DiscordEmbedVideo();

            video.Height = discordVideo.Height;
            video.Url = discordVideo.Url?.AbsoluteUri ?? String.Empty;
            video.Width = discordVideo.Width;

            return video;
        }

        public DiscordEmbedImage ConvertImage(DSharpPlus.Entities.DiscordEmbedImage discordImage)
        {
            var image = new DiscordEmbedImage();

            image.Height = discordImage.Height;
            image.ProxyUrl = discordImage.ProxyUrl?.AbsoluteUri ?? String.Empty;
            image.Url = discordImage.Url?.AbsoluteUri ?? String.Empty;
            image.Width = discordImage.Width;

            return image;
        }

        public DiscordEmbedThumbnail ConvertThumbnail(DSharpPlus.Entities.DiscordEmbedThumbnail discordThumbnail)
        {
            var thumbnail = new DiscordEmbedThumbnail();

            thumbnail.Height = discordThumbnail.Height;
            thumbnail.Width = discordThumbnail.Width;
            thumbnail.Url = discordThumbnail.Url?.AbsoluteUri ?? String.Empty;
            thumbnail.ProxyUrl = discordThumbnail.ProxyUrl?.AbsoluteUri ?? String.Empty;

            return thumbnail;
        }

        public DiscordEmbedProvider ConvertProvider(DSharpPlus.Entities.DiscordEmbedProvider discordProvider)
        {
            var provider = new DiscordEmbedProvider();

            provider.Name = discordProvider.Name;
            provider.Url = discordProvider.Url?.AbsoluteUri ?? String.Empty;

            return provider;
        }

        public DiscordEmbedFooter ConvertFooter(DSharpPlus.Entities.DiscordEmbedFooter discordFooter)
        {
            var footer = new DiscordEmbedFooter();

            footer.IconUrl = discordFooter.IconUrl?.AbsoluteUri ?? String.Empty;
            footer.ProxyIconUrl = discordFooter.ProxyIconUrl?.AbsoluteUri ?? String.Empty;
            footer.Text = discordFooter.Text;

            return footer;
        }

        public DiscordColor ConvertDiscordColor(DSharpPlus.Entities.DiscordColor discordColor)
        {
            var color = new DiscordColor();

            color.R = discordColor.R;
            color.G = discordColor.G;
            color.B = discordColor.B;
            color.Value = discordColor.Value;

            return color;
        }

        public List<DiscordAttachment> ConvertAttachments(IReadOnlyList<DSharpPlus.Entities.DiscordAttachment> attachments)
        {
            var list = new List<DiscordAttachment>();


            foreach (var attachment in attachments)
            {
                list.Add(ConvertAttachment(attachment));
            }


            return list;
        }

        public DiscordAttachment ConvertAttachment(DSharpPlus.Entities.DiscordAttachment discordAttachment)
        {
            var attachment = new DiscordAttachment();

            attachment.DiscordAttachmentId = discordAttachment.Id.ToString();
            attachment.FileName = discordAttachment.FileName;
            attachment.FileSize = discordAttachment.FileSize;
            attachment.Height = discordAttachment.Height;
            attachment.Width = discordAttachment.Width;
            attachment.Url = discordAttachment.Url;
            attachment.ProxyUrl = discordAttachment.ProxyUrl;

            return attachment;
        }

        public DiscordEmbedAuthor ConvertAuthor(DSharpPlus.Entities.DiscordEmbed discordEmbed)
        {
            var author = new DiscordEmbedAuthor();

            author.IconUrl = discordEmbed.Author.IconUrl?.AbsoluteUri ?? String.Empty;
            author.Name = discordEmbed.Author.Name;
            author.ProxyIconUrl = discordEmbed.Author.ProxyIconUrl?.AbsoluteUri ?? String.Empty;
            author.Url = discordEmbed.Author.Url?.AbsoluteUri ?? String.Empty;

            return author;
        }


        public List<DiscordEmbedField> ConvertFields(DSharpPlus.Entities.DiscordEmbed discordEmbed)
        {
            var fields = new List<DiscordEmbedField>();

            foreach (var field in discordEmbed.Fields)
            {
                fields.Add(ConvertField(field));
            }

            return fields;
        }

        public DiscordEmbedField ConvertField(DSharpPlus.Entities.DiscordEmbedField embedField)
        {
            var field = new DiscordEmbedField();

            field.Inline = embedField.Inline;
            field.Name = embedField.Name;
            field.Value = embedField.Value;

            return field;
        }
    }
}
