using DapperDino.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Areas.Admin.Models
{
    public class ApplicantEditViewModel
    {
        public string DiscordId { get; set; }
        public Applicant Applicant { get; set; }
    }
}
