using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class SnowflakeObject
    {
        public ulong Id { get; }
        public DateTimeOffset CreationTimestamp { get; }
    }
}
