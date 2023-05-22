namespace MVCWebAppWithoutBLL.Exceptions
{
    public class IllegalOperationException : Exception
    {
        public IllegalOperationException()
        {
        }

        public IllegalOperationException(string message) : base(message)
        {
        }
    }
}
