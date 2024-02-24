namespace CarAuctionApi.Domain.Exceptions
{
    [Serializable]
    public class ValidationException : Exception
    {
        public ValidationException()
        {
        }

        public ValidationException(string message)
            : this(message, null)
        {
        }

        public ValidationException(string message, Exception? inner)
            : base(message, inner)
        {
        }
    }
}
