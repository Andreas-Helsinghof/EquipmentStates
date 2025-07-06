using Microsoft.Extensions.DependencyInjection;

namespace EquipmentStates.HMI.MachineStateChangeHandler
{
    public static class Register
    {
        public static IServiceCollection AddMachineEvents(this IServiceCollection services)
        {
            // Register MachineStateChangeHandler as singleton and as hosted service and IMachineEvents
            services.AddSingleton<MachineStateChangeHandler>();
            services.AddHostedService(sp => sp.GetRequiredService<MachineStateChangeHandler>());
            services.AddSingleton<IMachineEvents>(sp => sp.GetRequiredService<MachineStateChangeHandler>());
            return services;
        }
    }
}
