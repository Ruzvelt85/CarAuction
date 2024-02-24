namespace CarAuctionApi.Domain.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message)
            : this(message, null)
        {
        }

        public NotFoundException(string message, Exception? inner)
            : base(message, inner)
        {
        }
    }
}
