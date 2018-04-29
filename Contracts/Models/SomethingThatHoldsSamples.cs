namespace Contracts.Models
{
    public class SomethingThatHoldsSamples
    {
        public string Field;
        public HoldsTriggers Samples;
        public User Triggers; //not here

        public SomethingThatHoldsSamples()
        {
            Field = "Sample text";
            Triggers = new User();
            Samples = new HoldsTriggers();
        }
    }
}