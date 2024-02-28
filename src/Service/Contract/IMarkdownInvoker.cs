using Markdig;
using Markdig.Syntax;

namespace Seagull.Service.Contract;

public interface IMarkdownInvoker
{
    public string InvokeHtml(MarkdownDocument document, MarkdownPipeline pipeline);
}
