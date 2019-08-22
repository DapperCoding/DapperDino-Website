using DapperDino.DAL.Models.Forms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DapperDino.DAL.Repositories
{
    public class TeacherFormRepository : GenericRepository<TeacherForm>
    {
        public TeacherFormRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public IQueryable<TeacherForm> GetForDiscordUser(string discordId)
        {
            return _dbContext.TeacherForms.Include(x => x.Replies).Include(x => x.DiscordUser).Where(x => x.DiscordUser.DiscordId == discordId);
        }
    }
}
