using System.Text.Json.Serialization;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(EmailProviderConfig), "Email")]
[JsonDerivedType(typeof(SmsProviderConfig), "Sms")]
public abstract class NotificationProviderConfig
{
    public string Name { get; set; }
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
