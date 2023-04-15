using Microsoft.Extensions.DependencyInjection;

namespace Knowledge.DDD.Demo.Core.Contracts;

public static class ContainerAdapterExtensions
{
    public static void RegisterContracts(this IServiceCollection serviceCollection)
    {
        RegisterMediator(serviceCollection);
    }
    
    private static void RegisterMediator(IServiceCollection instance)
    {
        instance.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ContainerAdapterExtensions).Assembly));
    }
}