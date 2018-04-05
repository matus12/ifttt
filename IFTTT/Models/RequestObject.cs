using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFTTT
{
    public class RequestObject
    {
        public string Trigger_identity;
        public TriggerFields TriggerFields;
        public IftttSource Ifttt_Source;
        public int limit = -1;
        public User User;
    }
}