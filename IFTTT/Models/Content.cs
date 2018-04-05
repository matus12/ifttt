using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFTTT
{
    public class Content
    {
        public TriggerFields[] Data;

        public Content(int limit)
        {
            Data = new TriggerFields[limit];
            for (int i = 0; i < limit; i++)
            {
                Data[i] = new TriggerFields($"field{i+1}");
            }
        }
    }
}