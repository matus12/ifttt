namespace Contracts.Models
{
    public class RequestObject
    {
        // ReSharper disable once InconsistentNaming
        public string Trigger_identity;
        public TriggerFields TriggerFields;
        // ReSharper disable once InconsistentNaming
        public IftttSource Ifttt_Source;
        // ReSharper disable once InconsistentNaming
        public int limit = -1;
        public User User;
    }
}