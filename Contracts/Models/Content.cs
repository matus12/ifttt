namespace Contracts.Models
{
    public class Content
    {
        public TriggerFields[] Data;

        public Content(int limit)
        {
            Data = new TriggerFields[limit];
            for (var i = 0; i < limit; i++)
            {
                Data[i] = new TriggerFields($"field{i+1}");
            }
        }

        public Content(TriggerFields triggerFields)
        {
            Data = new TriggerFields[1];
            Data[0] = triggerFields;
        }
    }
}