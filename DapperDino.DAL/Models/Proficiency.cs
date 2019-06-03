using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class DiscordUserProficiency: IEntity
    {
        public int DiscordUserId { get; set; }
        [ForeignKey("DiscordUserId")]
        public DiscordUser DiscordUser { get; set; }
        public int ProficiencyId { get; set; }
        [ForeignKey("ProficiencyId")]
        public Proficiency Proficiency { get; set; }
        public ProficiencyLevel ProficiencyLevel { get; set; } = ProficiencyLevel.AbsoluteBeginner;
    }

    public class Proficiency: IEntity
    {

        public ProficiencyType ProficiencyType { get; set; } = ProficiencyType.Language;
        public string Name { get; set; }
    }

    public enum ProficiencyLevel
    {
        AbsoluteBeginner = 0,
        JustStarted = 1,
        Medior = 2,
        Senior = 3,
        Expert =4
    }

    public enum ProficiencyType
    {
        Language = 0,
        Library = 1
    }
}
