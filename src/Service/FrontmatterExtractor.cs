using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using Seagull.Model;
using Seagull.Service.Contract;
using YamlDotNet.Serialization;

namespace Seagull.Service;

public class FrontmatterExtractor(IDeserializer deserializer) : IFrontmatterExtractor
{
    public Frontmatter? Extract(MarkdownDocument md)
    {
        var block = md.Descendants<YamlFrontMatterBlock>().FirstOrDefault();
        var yaml = block?.Lines.ToString();
        
        return yaml == null ? null : deserializer.Deserialize<Frontmatter>(yaml);
    }
}
