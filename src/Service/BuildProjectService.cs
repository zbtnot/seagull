using Seagull.Model;
using YamlDotNet.Serialization;

namespace Seagull.Service;

public class BuildProjectService(
    IDeserializer deserializer,
    IFileService fileService,
    MarkdownFileRendererService markdownFileRendererService
)
{
    public void BuildProject(string src, string dest)
    {
        var configText = FindAndReadConfigFile(src);
        var config = deserializer.Deserialize<Configuration>(configText);
        if (fileService.DirectoryPathExists(dest) == false)
        {
            fileService.CreateDirectory(dest);
        }
        
        var mdFiles = FindAndReadMdFiles(src);
        RenderMdFilesAndWrite(mdFiles, dest);
    }

    protected string FindAndReadConfigFile(string path)
    {
        var filePaths = fileService.ReadDirectoryContents(path).ToArray();
        foreach (var filePath in filePaths)
        {
            if (filePath.EndsWith("/seagull.yml"))
            {
                return fileService.ReadTextFile(filePath);
            }
        }

        throw new FileNotFoundException("Could not locate seagull.yml in the provided path.");
    }

    protected IEnumerable<MarkdownFile> FindAndReadMdFiles(string path)
    {
        var mdFilePaths = fileService.ReadDirectoryContents(path, "*.md");

        MarkdownFile MdFileMapper(string mdFilePath)
        {
            return new()
            {
                Contents = fileService.ReadTextFile(mdFilePath),
                Path = mdFilePath.Replace(path, string.Empty),
            };
        }

        return mdFilePaths.Select(MdFileMapper);
    }

    protected void RenderMdFilesAndWrite(IEnumerable<MarkdownFile> mdFiles, string path)
    {
        foreach (var mdFile in mdFiles)
        {
            var page = markdownFileRendererService.RenderAsPage(mdFile);
            fileService.CreateTextFile(Path.Join(path, page.Path), page.Content);
        }
    }
}
