using Markdig;
using Seagull.Model;

namespace Seagull.Service;

public class MarkdownRendererService(MarkdownPipeline pipeline, IMarkdownParser parser, IMarkdownInvoker invoker) : IMarkdownRendererService
{
    public Page RenderAsPage(MarkdownFile file)
    {
        var md = parser.Parse(file.Contents, pipeline);
        var contentHtml = invoker.InvokeHtml(md, pipeline); // TODO support template layouts
        var pathHtml = file.Path.Replace(".md", ".html");

        return new Page
        {
            Title = "A title goes here", // TODO extract front matter
            Content = contentHtml,
            Path = pathHtml,
        };
    }
}
