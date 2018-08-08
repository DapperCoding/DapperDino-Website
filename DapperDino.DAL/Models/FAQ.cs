using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class FAQ
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public FAQQuestion Question { get; set; }
        public FAQAnswer Answer { get; set; }
    }
    

    public class FAQQuestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        // We might want to add a stack trace message
        public string StackTrace { get; set; }
        public IEnumerable<string> ErrorLinks { get; set; }
    }

    public class FAQAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Solution { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> UsefulLinks { get; set; }
    }
}
