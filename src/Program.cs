using CommandLine;
using Markdig;
using Markdig.Renderers;
using Seagull.Model.Option;
using Seagull.Service;
using YamlDotNet.Serialization;

namespace Seagull;

internal static class Program
{
    private static readonly FileService _fileService;
    private static readonly Serializer _serializer;
    private static readonly Deserializer _deserializer;
    private static readonly BuildProjectService _buildProjectService;
    private static readonly GenerateProjectService _generateProjectService;
    private static readonly MarkdownPipeline _markdownPipeline;
    private static readonly MarkdownFileRendererService _markdownFileRendererService;

    static Program()
    {
        _fileService = new FileService();
        _serializer = new Serializer();
        _deserializer = new Deserializer();
        var mdPipelineBuilder = new MarkdownPipelineBuilder().UseYamlFrontMatter();
        _markdownPipeline = mdPipelineBuilder.Build();
        _markdownPipeline.Setup(new HtmlRenderer(new StringWriter()));
        _markdownFileRendererService = new MarkdownFileRendererService(_markdownPipeline, new MarkdownParser());
        _buildProjectService = new BuildProjectService(_deserializer, _fileService, _markdownFileRendererService);
        _generateProjectService = new GenerateProjectService(_serializer, _fileService);
    }

    public static int Main(string[] args)
    {
        try
        {
            Parser.Default
                .ParseArguments<BuildProject, GenerateProject>(args)
                .WithParsed(RunOptions);
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            return 1;
        }

        return 0;
    }

    static void RunOptions(object optionType)
    {
        switch (optionType)
        {
            case BuildProject option:
                _buildProjectService.BuildProject(option.SourcePath, option.DestinationPath);
                break;
            case GenerateProject option:
                _generateProjectService.GenerateProject(option.Path);
                break;
        }
    }
}
