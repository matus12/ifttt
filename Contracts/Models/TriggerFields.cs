using System;

namespace Contracts.Models
{
    public class TriggerFields
    {
        public string Project_id;
        public Meta Meta;
        public DateTime Created_at;
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
        public string Value5 { get; set; }
        public string Value6 { get; set; }
        public string Value7 { get; set; }
        public string Value8 { get; set; }
        public string Value9 { get; set; }
        public string Value10 { get; set; }

        public TriggerFields(string value)
        {
            Created_at = DateTime.Now;
            Meta = new Meta();
            Project_id = value;
        }
    }
}
