public class GlobalSettings
{
    public NotificationProviderConfig MainNotificationProvider { get; set; }
    
    public NotificationProviderConfig[] ProvidersList { get; set; }
    
    public Dictionary<Providers, NotificationProviderConfig> ProvidersDictionary { get; set; }
    
    public Providers SomethingEnum { get; set; }
}