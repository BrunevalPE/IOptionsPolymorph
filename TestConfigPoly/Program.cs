using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var configurationRoot = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var serviceCollection = new ServiceCollection();

serviceCollection.AddPolymorphicOptions<NotificationProviderConfig>(configurationRoot, "NotificationProvider");

var sp = serviceCollection.BuildServiceProvider();
var o = sp.GetService<IOptions<NotificationProviderConfig>>();
Console.WriteLine(o.Value.Name);

if (o.Value is EmailProviderConfig e)
{
    Console.WriteLine($"EmailProvider {e.SmtpServer}:{e.Port}");
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

