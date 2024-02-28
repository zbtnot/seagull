using YamlDotNet.Serialization;

namespace Seagull.Model;

public class Configuration
{
    [YamlMember(Alias = "title")]
    public string Title { get; set; } = string.Empty;

    [YamlMember(Alias = "templates")] public Dictionary<string, string> Templates { get; set; } = new();
}
