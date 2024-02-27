using Markdig;
using Markdig.Syntax;

namespace Seagull.Service;

/**
 * Wrapper class for Markdig's Parser
 */
public class MarkdownParser : IMarkdownParser
{
    public MarkdownDocument Parse(string content, MarkdownPipeline pipeline)
    {
        return MarkdigParse(content, pipeline);
    }

    protected virtual MarkdownDocument MarkdigParse(string content, MarkdownPipeline pipeline)
    {
        return Markdown.Parse(content, pipeline);
    }
}
