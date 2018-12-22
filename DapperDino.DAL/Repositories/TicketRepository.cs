using DapperDino.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDino.DAL.Repositories
{
    public class TicketRepository : GenericRepository<Ticket>
    {
        public TicketRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
