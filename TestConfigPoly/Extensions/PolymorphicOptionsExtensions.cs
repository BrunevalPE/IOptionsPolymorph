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
        services.AddTransient<IOptionsFactory<TOptions>>(sp => new PolymorphicOptionsFactory<TOptions>(configuration, sectionName));
        
        services.AddSingleton<IOptionsChangeTokenSource<TOptions>>(
            _ => new ConfigurationChangeTokenSource<TOptions>(
                null,
                configuration.GetSection(sectionName)));

        return services;
    }
}