using Markdig;
using Markdig.Syntax;
using Seagull.Service.Contract;

namespace Seagull.Service;

// Wrapper to handle the static methods on MarkdownDocument.
public class MarkdownInvoker : IMarkdownInvoker
{
    public string InvokeHtml(MarkdownDocument document, MarkdownPipeline pipeline)
    {
        return DocumentInvokeHtml(document, pipeline);
    }

    protected virtual string DocumentInvokeHtml(MarkdownDocument document, MarkdownPipeline pipeline)
    {
        return document.ToHtml(pipeline);
    }
}
