using AutoMapper;
using CarAuctionApi.WebApi.Mappings;

namespace CarAuctionApi.Tests.Mappings
{
    public class MappingTests
    {
        private readonly IMapper _mapper;

        public MappingTests()
        {
            var assemblyWithMapperProfiles = typeof(InventoryProfile).Assembly;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(assemblyWithMapperProfiles);
                cfg.ShouldMapProperty = p => p.GetMethod?.IsPublic == true || p.GetMethod?.IsPrivate == true;
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void AutomapperConfigurationTest()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
