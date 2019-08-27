using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models.Ideas
{
    public class LibrarySuggestion:IEntity
    {
        public string Name { get; set; }
        public int IdeaId { get; set; }
        [ForeignKey("IdeaId")]
        public Idea Idea { get; set; }
        public IEnumerable<LibrarySuggestionLink> Links { get; set; }
    }

    public class LibrarySuggestionLink:LinkBase
    {
        public int LibrarySuggestionId { get; set; }
        [ForeignKey("LibrarySuggestionId")]
        public LibrarySuggestion LibrarySuggestion { get; set; }
    }
}
