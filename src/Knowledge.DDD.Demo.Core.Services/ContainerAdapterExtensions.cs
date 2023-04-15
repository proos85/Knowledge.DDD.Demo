using Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Repositories;
using Knowledge.DDD.Demo.Core.Domain.PurchaseOrder;
using Knowledge.DDD.Demo.Core.Services.PurchaseOrder;
using Knowledge.DDD.Demo.Core.Services.PurchaseOrder.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Knowledge.DDD.Demo.Core.Services;

public static class ContainerAdapterExtensions
{
    public static void RegisterServices(this IServiceCollection serviceCollection)
    {
        RegisterDomainServices(serviceCollection);
        RegisterRepositories(serviceCollection);
        RegisterMediator(serviceCollection);
    }

    private static void RegisterDomainServices(IServiceCollection instance)
    {
        instance.AddScoped<ISnackOrderPayment, SnackOrderPayment>();
    }
    
    private static void RegisterRepositories(IServiceCollection instance)
    {
        instance.AddSingleton<ISnackMachineRepository, SnackMachineRepository>();
    }
    
    private static void RegisterMediator(IServiceCollection instance)
    {
        instance.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ContainerAdapterExtensions).Assembly));
    }
}