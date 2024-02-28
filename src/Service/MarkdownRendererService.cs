using Markdig;
using Seagull.Model;
using Seagull.Service.Contract;

namespace Seagull.Service;

public class MarkdownRendererService(
    MarkdownPipeline pipeline,
    IMarkdownParser markdownParser,
    IMarkdownInvoker invoker,
    IHtmlTemplateParser templateParser
) : IMarkdownRendererService
{
    public Page RenderAsPage(MarkdownFile file, IDictionary<string, string> templates, Configuration configuration)
    {
        var md = markdownParser.Parse(file.Contents, pipeline);
        var contentHtml = invoker.InvokeHtml(md, pipeline);
        var template = templateParser.Parse(templates.First().Value); // TODO source from Md file's front matter
        
        var templateVariables = new Dictionary<object, object>
        {
            ["title"] = configuration.Title,
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
