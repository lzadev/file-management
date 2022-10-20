namespace fileManagement.Exceptions
{
    public class InvalidFileException : Exception
    {
        public InvalidFileException(string message) : base(message)
        {

        }

        public InvalidFileException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
