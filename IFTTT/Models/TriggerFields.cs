using System;

namespace IFTTT
{
    public class TriggerFields
    {
        public string SomeField;
        public Meta Meta;
        public DateTime Created_at;
        public string Some_trigger;
        public TriggerFields(string value)
        {
            SomeField = value;
            Created_at = DateTime.Now;
            Meta = new Meta();
            Some_trigger = "ahoj";
        }
    }
}