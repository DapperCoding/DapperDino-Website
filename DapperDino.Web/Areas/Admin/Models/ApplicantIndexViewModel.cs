using DapperDino.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Areas.Admin.Models
{
    public class ApplicantIndexViewModel
    {
        public IEnumerable<Applicant> Applicants { get; set; }
    }
}
