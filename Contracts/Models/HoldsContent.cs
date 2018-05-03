namespace Contracts.Models
{
    public class HoldsContent
    {
        public TriggerFields Content;

        public HoldsContent()
        {
            var triggerFields = new TriggerFields("76281b95-6515-4a95-a3ca-15fd940dba2f")
            {
                Content_item_codename = "codename",
                Project_id = "project Id"
            };
            Content = triggerFields;
        }
    }
}