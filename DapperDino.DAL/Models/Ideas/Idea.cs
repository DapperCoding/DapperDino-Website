using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models.Ideas
{
    public class Idea:IEntity
    {
        public string ProductTitle { get; set; }
        public double SuggestedPrice { get; set; }
        public ProficiencyLevel KnowledgeLevel { get; set; }
        public int LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public Proficiency Language { get; set; }
        public int FrameworkId { get; set; }
        [ForeignKey("FrameworkId")]
        public Proficiency Framework { get; set; }
        public string Overview { get; set; }
        public IEnumerable<LibrarySuggestion> LibrarySuggestions { get; set; }
        public IEnumerable<Functionality> Functionalities { get; set; }
        public IEnumerable<IdeaWorker> Workers { get; set; }
    }

    public class IdeaWorker : IEntity
    {
        public int IdeaId { get; set; }
        [ForeignKey("IdeaId")]
        public Idea Idea { get; set; }
        public int DiscordUserId { get; set; }
        public DiscordUser DiscordUser { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public IdeaWorkerPermissions Permissions { get; set; }
    }

    public enum IdeaWorkerPermissions
    {
        All = 0,
        ReadOnly = 1,
        ReadEdit = 2
    }
}
