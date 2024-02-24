namespace CarAuctionApi.Domain.Exceptions
{
    [Serializable]
    public class ConflictException : Exception
    {
        public ConflictException()
        {
        }

        public ConflictException(string message)
            : this(message, null)
        {
        }

        public ConflictException(string message, Exception? inner)
            : base(message, inner)
        {
        }
    }
}
