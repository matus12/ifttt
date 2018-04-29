using System;

namespace Contracts.Models
{
    public class Meta
    {
        public Guid Id;
        public long Timestamp;

        public Meta()
        {
            Id = Guid.NewGuid();
            Timestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
        }
    }
}