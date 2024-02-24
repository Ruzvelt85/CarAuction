using System.ComponentModel.DataAnnotations;
using CarAuctionApi.Patterns;

namespace CarAuctionApi.Domain.Model
{
    public class Bid : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int VehicleId { get; set; }

        public int CustomerId { get; set; }

        public decimal Value { get; set; }
    }
}
