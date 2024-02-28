using Seagull.Model;
using Seagull.Service.Contract;

namespace Seagull.Service;

public class MarkdownFileFactory(IFileService fileService) : IMarkdownFileFactory
{
    public MarkdownFile Create(string filePath, string parentPath)
    {
        return new()
        {
            Contents = fileService.ReadTextFile(filePath),
            Path = filePath.Replace(parentPath, string.Empty),
        };
    }
}
