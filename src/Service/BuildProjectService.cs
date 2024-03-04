using Seagull.Model;
using Seagull.Service.Contract;
using YamlDotNet.Serialization;

namespace Seagull.Service;

public class BuildProjectService(
    IDeserializer deserializer,
    IFileService fileService,
    IMarkdownRendererService markdownRendererService,
    IMarkdownFileFactory markdownFileFactory,
    IIndexGenerator indexGenerator,
    IFileCopier fileCopier
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
        var pages = RenderMdFilesToPages(mdFiles, templates, config).ToList();
        pages.Sort((lhs, rhs) => rhs.Date.CompareTo(lhs.Date));
        var indexFile = indexGenerator.Generate(pages, templates.FirstOrDefault().Value, config);
        fileService.CreateTextFile(Path.Join(dest, "index.html"), indexFile);
        WritePagesToDisk(pages, dest);
        fileCopier.CopyFiles(src, dest);
    }

    private string FindAndReadConfigFile(string path)
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

    private IEnumerable<MarkdownFile> FindAndReadMdFiles(string path)
    {
        var mdFilePaths = fileService.ReadDirectoryContents(path, "*.md");

        return mdFilePaths.Select(filePath => markdownFileFactory.Create(filePath, path));
    }

    private IEnumerable<Page> RenderMdFilesToPages(
        IEnumerable<MarkdownFile> mdFiles,
        IDictionary<string, string> templates,
        Configuration configuration
    )
    {
        return mdFiles.Select(mdFile => markdownRendererService.RenderAsPage(mdFile, templates, configuration));
    }

    private void WritePagesToDisk(IEnumerable<Page> pages, string path)
    {
        foreach (var page in pages)
        {
            fileService.CreateTextFile(Path.Join(path, page.Path), page.Content);
        }
    }

    private IDictionary<string, string> ReadTemplates(IDictionary<string, string> templates, string path)
    {
        var loadedTemplates = new Dictionary<string, string>();

        foreach (var template in templates)
        {
            loadedTemplates[template.Key] = fileService.ReadTextFile(Path.Join(path, template.Value));
        }

        return loadedTemplates;
    }
}
