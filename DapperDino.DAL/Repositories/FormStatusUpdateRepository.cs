using DapperDino.DAL.Models.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDino.DAL.Repositories
{
    public class FormStatusUpdateRepository : GenericRepository<FormStatusUpdate>
    {
        public FormStatusUpdateRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
