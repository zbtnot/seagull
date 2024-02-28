using Scriban;
using Seagull.Model;
using Seagull.Service.Contract;

namespace Seagull.Service;

// Wrapper class for the static Template.Parse method
public class HtmlTemplateParser : IHtmlTemplateParser
{
    public HtmlTemplate Parse(string htmlTemplate)
    {
        return new HtmlTemplate
        {
            Template = TemplateParse(htmlTemplate),
        };
    }

    public string Render(HtmlTemplate htmlTemplate, IDictionary<object, object> context)
    {
        return htmlTemplate.Template?.Render(context) ?? string.Empty;
    }

    protected virtual Template TemplateParse(string htmlTemplate)
    {
        return Template.Parse(htmlTemplate);
    }
}
