using System.Reflection;
using AutoMapper;
using CarAuctionApi.Data;
using CarAuctionApi.Data.Repositories;
using CarAuctionApi.Dto.Responses;
using CarAuctionApi.Patterns;
using CarAuctionApi.ServiceInfrastructure.Filters;
using CarAuctionApi.WebApi.Commands;
using FluentValidation;
using FluentValidation.AspNetCore;
using CarAuctionApi.WebApi.Queries;

namespace CarAuctionApi.WebApi;

public sealed class Startup
{
    private Assembly ExecutingAssembly => Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDefaultServices();

        services.AddScoped<SaveChangesActionFilterAttribute>();
        
        services.AddDbContext<InMemoryEfCoreContext>();

        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IAuctionRepository, AuctionRepository>();
        services.AddScoped<IBidRepository, BidRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork<InMemoryEfCoreContext>>();
        
        services.AddScoped<IQueryHandler<GetVehicleListQuery, VehicleListResponseDto>, GetVehicleListQueryHandler>();
        services.AddScoped<ICommandHandler<CreateVehicleCommand, int>, CreateVehicleCommandHandler>();
        services.AddScoped<ICommandHandler<StartAuctionCommand, int>, StartAuctionCommandHandler>();
        services.AddScoped<ICommandHandler<CloseAuctionCommand, int>, CloseAuctionCommandHandler>();
        services.AddScoped<ICommandHandler<PlaceBidCommand, int>, PlaceBidCommandHandler>();

        ConfigureAutoMapper(services);
        ConfigureFluentValidation(services);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseDefaultAppConfig();
    }

    private void ConfigureAutoMapper(IServiceCollection services)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(ExecutingAssembly);
            cfg.ShouldMapProperty = p => p.GetMethod?.IsPublic == true || p.GetMethod?.IsPrivate == true;
        });

        services.AddSingleton(config.CreateMapper());
    }
    private void ConfigureFluentValidation(IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<Startup>();
    }
}
