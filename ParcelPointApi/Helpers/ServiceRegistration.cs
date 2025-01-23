using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public static class ServiceRegistration
{
    public static void RegisterServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Find all classes implementing an interface in the assembly
        var typesWithInterfaces = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Select(t => new
            {
                Implementation = t,
                Interface = t.GetInterfaces().FirstOrDefault() // Assumes only one interface per class
            })
            .Where(t => t.Interface != null);

        foreach (var type in typesWithInterfaces)
        {
            services.AddScoped(type.Interface, type.Implementation);
        }
    }
}
