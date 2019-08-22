using DapperDino.DAL.Models.Forms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DapperDino.DAL.Repositories
{
    public class RecruiterFormRepository : GenericRepository<RecruiterForm>
    {
        public RecruiterFormRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public IQueryable<RecruiterForm> GetForDiscordUser(string discordId)
        {
            return _dbContext.RecruiterForms.Include(x=>x.Replies).Include(x => x.DiscordUser).Where(x => x.DiscordUser.DiscordId == discordId);
        }
    }
}
