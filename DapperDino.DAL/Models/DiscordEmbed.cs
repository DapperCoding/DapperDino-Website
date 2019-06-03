using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class DiscordEmbed: IEntity
    {
        public int DiscordMessageId { get; set; }

        [ForeignKey("DiscordMessageId")]
        public DiscordMessage DiscordMessage { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public DiscordColor Color { get; set; }
        public DiscordEmbedFooter Footer { get; set; }
        public DiscordEmbedImage Image { get; set; }
        public DiscordEmbedThumbnail Thumbnail { get; set; }
        public DiscordEmbedVideo Video { get; set; }
        public DiscordEmbedProvider Provider { get; set; }
        public DiscordEmbedAuthor Author { get; set; }
        public List<DiscordEmbedField> Fields { get; set; }
    }

    public class DiscordEmbedField:IEntity
    {
        public int EmbedId { get; set; }
        [ForeignKey("EmbedId")]
        public  DiscordEmbed  Embed{ get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Inline { get; set; }
    }

    public class DiscordEmbedAuthor:IEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string IconUrl { get; set; }
        public string ProxyIconUrl { get; set; }
        public int DiscordEmbedId { get; set; }

        [ForeignKey("DiscordEmbedId")]
        public  DiscordEmbed DiscordEmbed { get; set; }
    }

    public class DiscordEmbedProvider:IEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int DiscordEmbedId { get; set; }

        [ForeignKey("DiscordEmbedId")]
        public  DiscordEmbed DiscordEmbed { get; set; }
    }

    public class DiscordEmbedVideo:IEntity
    {
        public string Url { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int DiscordEmbedId { get; set; }

        [ForeignKey("DiscordEmbedId")]
        public  DiscordEmbed DiscordEmbed { get; set; }
    }

    public class DiscordEmbedThumbnail:IEntity
    {
        public string Url { get; set; }
        public string ProxyUrl { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int DiscordEmbedId { get; set; }

        [ForeignKey("DiscordEmbedId")]
        public  DiscordEmbed DiscordEmbed { get; set; }
    }

    public class DiscordEmbedImage:IEntity
    {
        public string Url { get; set; }
        public string ProxyUrl { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int DiscordEmbedId { get; set; }

        [ForeignKey("DiscordEmbedId")]
        public  DiscordEmbed DiscordEmbed { get; set; }
    }

    public class DiscordEmbedFooter: IEntity
    {
        public string Text { get; set; }
        public string IconUrl { get; set; }
        public string ProxyIconUrl { get; set; }
        public int DiscordEmbedId { get; set; }

        [ForeignKey("DiscordEmbedId")]
        public  DiscordEmbed DiscordEmbed { get; set; }
    }

    public class DiscordColor: IEntity
    {
        public int DiscordEmbedId { get; set; }
        
        [ForeignKey("DiscordEmbedId")]
        public  DiscordEmbed DiscordEmbed { get; set; }

        public byte G { get; set; }
        public int Value { get; set; }
        public byte R { get; set; }
        public byte B { get; set; }
    }
}
