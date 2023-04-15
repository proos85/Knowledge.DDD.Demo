using Microsoft.Extensions.DependencyInjection;

namespace Knowledge.DDD.Demo.Infra.Payment;

public static class ContainerAdapterExtensions
{
    public static void RegisterInfraPayment(this IServiceCollection serviceCollection)
    {
        RegisterMediator(serviceCollection);
    }
    
    private static void RegisterMediator(IServiceCollection instance)
    {
        instance.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ContainerAdapterExtensions).Assembly));
    }
}