using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDino.DAL.Models.Forms
{
    public class ArchitectForm : FormBase
    {
        public List<FormReply<ArchitectForm>> Replies { get; set; }
        public string DevelopmentExperience { get; set; }
        public string PreviousIdeas { get; set; }
        public int Age { get; set; }
    }

    public class TeacherForm : FormBase
    {
        public List<FormReply<TeacherForm>> Replies { get; set; }
        public string DevelopmentExperience { get; set; }
        public string GithubLink { get; set; }
        public string TeachingExperience { get; set; }
        public string ProjectLinks { get; set; }
        public int Age { get; set; }
    }

    public class RecruiterForm : FormBase
    {
        public List<FormReply<RecruiterForm>> Replies { get; set; }
        public string DevelopmentExperience { get; set; }
        public string RecruitingExperience { get; set; }
        public string DevelopmentReviewingExperience { get; set; }
        public string GithubLink { get; set; }
        public string ProjectLinks { get; set; }
        public int Age { get; set; }

    }
}
