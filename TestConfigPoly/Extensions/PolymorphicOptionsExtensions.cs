using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class PolymorphicOptionsExtensions
{
    public static IServiceCollection AddPolymorphicOptions<TOptions>(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName) where TOptions : class
    {
        services.Configure<TOptions>(o => {});
        services.AddSingleton<IOptionsFactory<TOptions>>(new PolymorphicOptionsFactory<TOptions>(configuration, sectionName));
        return services;
    }
}