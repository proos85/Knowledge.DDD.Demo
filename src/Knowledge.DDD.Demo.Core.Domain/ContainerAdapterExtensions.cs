using Microsoft.Extensions.DependencyInjection;

namespace Knowledge.DDD.Demo.Core.Domain;

public static class ContainerAdapterExtensions
{
    public static void RegisterDomain(this IServiceCollection serviceCollection)
    {
        RegisterMediator(serviceCollection);
    }
    
    private static void RegisterMediator(IServiceCollection instance)
    {
        instance.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ContainerAdapterExtensions).Assembly));
    }
}