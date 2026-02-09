using System.Text.Json.Serialization;

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