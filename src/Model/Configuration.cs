using YamlDotNet.Serialization;

namespace Seagull.Model;

public class Configuration
{
    [YamlMember(Alias = "title")]
    public string Title { get; set; } = string.Empty;
}
