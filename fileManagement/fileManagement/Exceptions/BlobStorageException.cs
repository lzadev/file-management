namespace fileManagement.Exceptions
{
    public class BlobStorageException : Exception
    {
        public BlobStorageException(string message) : base(message)
        {

        }

        public BlobStorageException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
