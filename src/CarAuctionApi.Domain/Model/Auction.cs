using System.ComponentModel.DataAnnotations;
using CarAuctionApi.Patterns;

namespace CarAuctionApi.Domain.Model
{
    public class Auction : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int VehicleId { get; set; }

        public bool IsActive { get; set; }
    }
}
