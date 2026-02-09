using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var configurationRoot = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var serviceCollection = new ServiceCollection();

serviceCollection.AddPolymorphicOptions<GlobalSettings>(configurationRoot, "GlobalSettings");

var sp = serviceCollection.BuildServiceProvider();
var o = sp.GetService<IOptions<GlobalSettings>>();
Console.WriteLine(o.Value.SomethingEnum);

if (o.Value.MainNotificationProvider is EmailProviderConfig e)
{
    Console.WriteLine($"EmailProvider {e.SmtpServer}:{e.Port}");
}

[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Providers
{
    None = 0,
    Email = 1,
    Sms = 2,
    Smtp = 4,
    All = Email | Sms | Smtp
}

public class GlobalSettings
{
    public NotificationProviderConfig MainNotificationProvider { get; set; }
    
    public NotificationProviderConfig[] ProvidersList { get; set; }
    
    public Dictionary<Providers, NotificationProviderConfig> ProvidersDictionary { get; set; }
    
    public Providers SomethingEnum { get; set; }
}
        
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(EmailProviderConfig), "Email")]
[JsonDerivedType(typeof(SmsProviderConfig), "Sms")]
public abstract class NotificationProviderConfig
{
    public string Name { get; set; }
    // The "Type" property is automatically handled by the attributes
}

public class EmailProviderConfig : NotificationProviderConfig
{
    public string SmtpServer { get; set; }
    public int Port { get; set; }
}

public class SmsProviderConfig : NotificationProviderConfig
{
    public string ApiKey { get; set; }
}

