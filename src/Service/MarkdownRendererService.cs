using Markdig;
using Seagull.Model;
using Seagull.Service.Contract;

namespace Seagull.Service;

public class MarkdownRendererService(
    MarkdownPipeline pipeline,
    IMarkdownParser markdownParser,
    IMarkdownInvoker invoker,
    IHtmlTemplateParser templateParser,
    IFrontmatterExtractor frontmatterExtractor
) : IMarkdownRendererService
{
    public Page RenderAsPage(MarkdownFile file, IDictionary<string, string> templates, Configuration configuration)
    {
        var md = markdownParser.Parse(file.Contents, pipeline);
        var contentHtml = invoker.InvokeHtml(md, pipeline);
        var template = templateParser.Parse(templates.First().Value);
        var frontmatter = frontmatterExtractor.Extract(md);
        
        var templateVariables = new Dictionary<object, object>
        {
            ["title"] = frontmatter?.Title ?? configuration.Title,
            ["date"] = frontmatter?.Date ?? DateTime.Now,
            ["description"] = frontmatter?.Description ?? string.Empty,
            ["keywords"] = frontmatter?.Keywords ?? Array.Empty<string>(),
            ["content"] = contentHtml,
        };
        
        var renderedTemplateHtml = templateParser.Render(template, templateVariables);
        var pathHtml = file.Path.Replace(".md", ".html");

        return new Page
        {
            Title = "A title goes here", // TODO extract front matter
            Content = renderedTemplateHtml,
            Path = pathHtml,
        };
    }
}
