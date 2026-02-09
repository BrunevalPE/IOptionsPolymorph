using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var configurationRoot = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).Build();
var serviceCollection = new ServiceCollection();

serviceCollection.AddOptions();

serviceCollection.AddPolymorphicOptions<GlobalSettings>(configurationRoot, "GlobalSettings");

var sp = serviceCollection.BuildServiceProvider();

var globalSettingsMonitor = sp.GetService<IOptionsMonitor<GlobalSettings>>();

if (globalSettingsMonitor.CurrentValue.MainNotificationProvider is EmailProviderConfig e)
{
    Console.WriteLine($"Main provider is EmailProvider {e.SmtpServer}:{e.Port}");
}

globalSettingsMonitor.OnChange(s => Console.WriteLine("Configuration changed : " + JsonSerializer.Serialize(s)));

// to debug live settings updates
// Task.Run(async() =>
// {
//     for (var i = 0; i < 100; i++)
//     {
//         Task.Delay(1000).Wait();
//         var current = sp.GetService<IOptionsSnapshot<GlobalSettings>>();
//         var current2 = configurationRoot["GlobalSettings:SomethingEnum"];
//         Console.WriteLine($"Current mode: {globalSettingsMonitor.CurrentValue.SomethingEnum} vs {current.Value.SomethingEnum} vs {current2}");
//     }
// });

Console.WriteLine("Configuration loaded");
Console.ReadLine();


