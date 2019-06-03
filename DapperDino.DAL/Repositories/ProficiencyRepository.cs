using DapperDino.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperDino.DAL.Repositories
{
    public class ProficiencyRepository : GenericRepository<Proficiency>
    {
        public ProficiencyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public IEnumerable<Proficiency> GetLanguages()
        {
            var languages = _dbContext.Proficiencies.Where(x => x.ProficiencyType == ProficiencyType.Language);

            return languages;
        }

        public IEnumerable<Proficiency> GetFrameworks()
        {
            var frameworks = _dbContext.Proficiencies.Where(x => x.ProficiencyType == ProficiencyType.Library);

            return frameworks;
        }
    }

    public class DiscordUserProficiencyRepository : GenericRepository<DiscordUserProficiency>
    {
        public DiscordUserProficiencyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<DiscordUserProficiency> GetByProficiencyAndDiscordUser(int proficiencyId, int discordUserId)
        {
            var proficiency = await _dbContext.DiscordUserProficiencies
                //.Find(new { ProficiencyId = proficiencyId , DiscordUserId  = discordUserId });
                .FirstOrDefaultAsync(x => x.ProficiencyId == proficiencyId && x.DiscordUserId == discordUserId);

            return proficiency;
        }

        public IEnumerable<DiscordUserProficiency> FindAllForDiscordUser(string discordUserId)
        {
            var proficiencies = _dbContext.DiscordUserProficiencies.Include(x => x.DiscordUser).Include(x => x.Proficiency).Where(x => x.DiscordUser.DiscordId == discordUserId);

            return proficiencies;
        }
    }
}
