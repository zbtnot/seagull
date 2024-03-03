using System.Text;
using Seagull.Model;
using Seagull.Service.Contract;

namespace Seagull.Service;

public class IndexGenerator(IHtmlTemplateParser parser) : IIndexGenerator
{
    public string Generate(IEnumerable<Page> pages, string template, Configuration config)
    {
        var parsedTemplate = parser.Parse(template);
        var content = pages.Aggregate(new StringBuilder(), Reducer).ToString();

        return parser.Render(parsedTemplate, new Dictionary<object, object>
        {
            ["title"] = config.Title,
            ["content"] = content,
        });
    }

    private static StringBuilder Reducer(StringBuilder acc, Page page)
    {
        var relativePath = Path.Join(".", page.Path);
        acc.Append(
            $"""
                 <article>
                     <a href="{relativePath}">{page.Title}</a>
                     {page.Description}
                 </article>
             """);

        return acc;
    }
}
