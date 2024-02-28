using Markdig.Syntax;
using Seagull.Model;

namespace Seagull.Service.Contract;

public interface IFrontmatterExtractor
{
    public Frontmatter? Extract(MarkdownDocument md);
}
