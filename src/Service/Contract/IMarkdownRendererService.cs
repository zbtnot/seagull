using Seagull.Model;

namespace Seagull.Service.Contract;

public interface IMarkdownRendererService
{
    public Page RenderAsPage(MarkdownFile file, IDictionary<string, string> templates, Configuration configuration);
}
