using Seagull.Model;

namespace Seagull.Service.Contract;

public interface IMarkdownFileFactory
{
    public MarkdownFile Create(string filePath, string parentPath);
}
