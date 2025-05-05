using Application.Core.Abstractions.Common;
using Microsoft.Extensions.DependencyInjection;
using Services.Common;

namespace Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IDateTime, MachineDateTime>();

        return services;
    }
}
