using CarAuctionApi.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionApi.Data
{
    public class InMemoryEfCoreContext : DbContext
    {
#nullable disable
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }

#nullable restore

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "CarAuctionDb");
        }
    }
}
