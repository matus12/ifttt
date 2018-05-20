namespace Contracts.Models
{
    public class TriggerData
    {
        public string Field;
        public Samples Samples;

        public TriggerData()
        {
            Field = "Sample text";
            Samples = new Samples();
        }
    }
}