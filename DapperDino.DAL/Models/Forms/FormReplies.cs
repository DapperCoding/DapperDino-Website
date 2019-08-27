using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDino.DAL.Models.Forms
{


    public class ArchitectFormReply : FormReply<ArchitectForm>
    {

    }

    public class TeacherFormReply : FormReply<TeacherForm>
    {

    }


    public class RecruiterFormReply : FormReply<RecruiterForm>
    {

    }



    public class TeacherFormReplyModel : TeacherFormReply
    {
        public string DiscordId { get; set; }
    }
    public class ArchitectFormReplyModel : TeacherFormReply
    {
        public string DiscordId { get; set; }
    }
    public class RecruiterFormReplyModel : TeacherFormReply
    {
        public string DiscordId { get; set; }
    }
}
