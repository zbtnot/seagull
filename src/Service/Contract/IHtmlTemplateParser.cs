using Seagull.Model;

namespace Seagull.Service.Contract;

public interface IHtmlTemplateParser
{
    public HtmlTemplate Parse(string htmlTemplate);
    public string Render(HtmlTemplate htmlTemplate, IDictionary<object, object> context);
}
