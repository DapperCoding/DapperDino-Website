using DapperDino.DAL.Models.Forms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DapperDino.DAL.Repositories
{
    public class ArchitectFormRepository : GenericRepository<ArchitectForm>
    {
        public ArchitectFormRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public IQueryable<ArchitectForm> GetForDiscordUser(string discordId)
        {
            return _dbContext.ArchitectForms.Include(x=>x.Replies).Include(x => x.DiscordUser).Where(x => x.DiscordUser.DiscordId == discordId);
        }
    }
}
