namespace Contracts.Models
{
    public class ContentError
    {
        public Error[] Errors;

        public ContentError()
        {
            Errors = new Error[1];
            Errors[0] = new Error();
        }
    }
}