using DapperDino.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDino.DAL.Repositories
{
    public class DiscordMessageRepository : GenericRepository<DiscordMessage>
    {
        public DiscordMessageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
