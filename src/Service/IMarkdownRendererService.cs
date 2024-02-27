using Seagull.Model;

namespace Seagull.Service;

public interface IMarkdownRendererService
{
    public Page RenderAsPage(MarkdownFile file);
}
