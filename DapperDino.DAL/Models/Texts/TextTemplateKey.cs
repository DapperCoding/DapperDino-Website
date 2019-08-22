using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDino.DAL.Models.Texts
{
    public class TextTemplateKeys:IEntity
    {
        public int TextId { get; set; }
        public TextTemplate Text { get; set; }
        public string Key { get; set; }
    }
}
