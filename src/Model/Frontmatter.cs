using YamlDotNet.Serialization;

namespace Seagull.Model;

/**
 * Represents a MarkdownFile's YAML frontmatter.
 */
public class Frontmatter
{
    [YamlMember(Alias = "title")] public string? Title { get; init; }

    [YamlMember(Alias = "description")] public string? Description { get; init; }

    [YamlMember(Alias = "date")] public DateTime? Date { get; init; }

    [YamlMember(Alias = "keywords")] public string[]? Keywords { get; init; }
}
