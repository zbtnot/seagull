using Markdig;
using Markdig.Syntax;

namespace Seagull.Service;

public interface IMarkdownInvoker
{
    public string InvokeHtml(MarkdownDocument document, MarkdownPipeline pipeline);
}
