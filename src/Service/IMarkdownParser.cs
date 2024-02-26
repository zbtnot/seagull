using Markdig;
using Markdig.Syntax;

namespace Seagull.Service;

public interface IMarkdownParser
{
    MarkdownDocument Parse(string content, MarkdownPipeline pipeline);
}
