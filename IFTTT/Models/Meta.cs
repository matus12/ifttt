using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFTTT
{
    public class Meta
    {
        public Guid Id;
        public long Timestamp;

        public Meta()
        {
            Id = Guid.NewGuid();
            DateTime foo = DateTime.UtcNow;
            Timestamp = ((DateTimeOffset)foo).ToUnixTimeSeconds();
        }
    }
}