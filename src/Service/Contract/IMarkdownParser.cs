using Markdig;
using Markdig.Syntax;

namespace Seagull.Service.Contract;

public interface IMarkdownParser
{
    MarkdownDocument Parse(string content, MarkdownPipeline pipeline);
}
