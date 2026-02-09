using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

public class PolymorphicOptionsFactory<TOptions> : IOptionsFactory<TOptions> where TOptions : class
{
    private readonly IConfiguration _configuration;
    private readonly string _sectionName;
    private readonly JsonSerializerOptions _jsonOptions;

    public PolymorphicOptionsFactory(IConfiguration configuration, string sectionName)
    {
        _configuration = configuration;
        _sectionName = sectionName;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public TOptions Create(string name)
    {
        var section = _configuration.GetSection(_sectionName);
        var jsonString = ConvertSectionToJson(section);
        return JsonSerializer.Deserialize<TOptions>(jsonString, _jsonOptions)
               ?? throw new InvalidOperationException($"Failed to deserialize {typeof(TOptions).Name}");
    }

    private string ConvertSectionToJson(IConfigurationSection section)
    {
        var obj = ConvertToObject(section);
        return JsonSerializer.Serialize(obj, _jsonOptions);
    }

    private object? ConvertToObject(IConfigurationSection section)
    {
        var children = section.GetChildren().ToList();
        if (!children.Any())
        {
            var value = section.Value;
            if (value == null)
                return null;

            if (int.TryParse(value, out var intVal))
                return intVal;
            if (long.TryParse(value, out var longVal))
                return longVal;
            if (double.TryParse(value, out var doubleVal))
                return doubleVal;
            if (bool.TryParse(value, out var boolVal))
                return boolVal;

            return value;
        }

        if (children.All(c => int.TryParse(c.Key, out _)))
            return children.Select(ConvertToObject).ToList();

        var dict = new Dictionary<string, object?>();
        foreach (var child in children)
        {
            dict[child.Key] = ConvertToObject(child);
        }

        return dict;
    }
}