using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDino.DAL.Models.Texts
{
    public class TextTemplate:IEntity
    {
        public string Content { get; set; }
        public TextEnvironment Environment { get; set; }
        public List<TextTemplateKeys> Keys { get; set; }
    }
}
