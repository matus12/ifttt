namespace Contracts.Models
{
    public class HoldsContent
    {
        public TriggerFields Content;

        public HoldsContent()
        {
            Content = new TriggerFields("abc");
        }
    }
}