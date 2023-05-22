namespace BLL.Exceptions
{
    public class EmailAlreadyUsedException : Exception
    {
        public EmailAlreadyUsedException() 
        { 
            
        }

        public EmailAlreadyUsedException(string message) : base(message)
        {
        }
    }
    
}
