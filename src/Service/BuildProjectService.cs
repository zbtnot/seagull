using Seagull.Model;
using Seagull.Service.Contract;
using YamlDotNet.Serialization;

namespace Seagull.Service;

public class BuildProjectService(
    IDeserializer deserializer,
    IFileService fileService,
    IMarkdownRendererService markdownRendererService,
    IMarkdownFileFactory markdownFileFactory
)
{
    public void BuildProject(string src, string dest)
    {
        var configText = FindAndReadConfigFile(src);
        var config = deserializer.Deserialize<Configuration>(configText);
        var templates = ReadTemplates(config.Templates, src);
        if (templates.Count == 0)
        {
            throw new FileNotFoundException("No template files found. Please check your configuration.");
        }

        if (fileService.DirectoryPathExists(dest) == false)
        {
            fileService.CreateDirectory(dest);
        }

        var mdFiles = FindAndReadMdFiles(src);
        RenderMdFilesAndWrite(mdFiles, dest, templates, config);
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

        return mdFilePaths.Select(filePath => markdownFileFactory.Create(filePath, path));
    }

    protected void RenderMdFilesAndWrite(
        IEnumerable<MarkdownFile> mdFiles,
        string path,
        IDictionary<string, string> templates,
        Configuration configuration
    )
    {
        foreach (var mdFile in mdFiles)
        {
            var page = markdownRendererService.RenderAsPage(mdFile, templates, configuration);
            fileService.CreateTextFile(Path.Join(path, page.Path), page.Content);
        }
    }

    protected IDictionary<string, string> ReadTemplates(IDictionary<string, string> templates, string path)
    {
        var loadedTemplates = new Dictionary<string, string>();

        foreach (var template in templates)
        {
            loadedTemplates[template.Key] = fileService.ReadTextFile(Path.Join(path, template.Value));
        }

        return loadedTemplates;
    }
}
