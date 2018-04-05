using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFTTT
{
    public class Content
    {
        public Guid Trigger_identity;
        public TriggerFields TriggerFields;
        public Content()
        {
            Trigger_identity = Guid.NewGuid();
            TriggerFields = new TriggerFields();
        }
    }
}