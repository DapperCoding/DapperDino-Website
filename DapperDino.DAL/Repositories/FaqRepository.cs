using DapperDino.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDino.DAL.Repositories
{
    public class FaqRepository : GenericRepository<FrequentlyAskedQuestion>
    {
        public FaqRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}
