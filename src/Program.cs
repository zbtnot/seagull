using CommandLine;
using Markdig;
using Markdig.Renderers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Seagull.Model.Option;
using Seagull.Service;
using Seagull.Service.Contract;
using YamlDotNet.Serialization;

namespace Seagull;

internal static class Program
{
    private static IHost? _host;

    public static int Main(string[] args)
    {
        try
        {
            // assemble the service container
            var builder = Host.CreateEmptyApplicationBuilder(null);
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<ISerializer, Serializer>();
            builder.Services.AddScoped<IDeserializer, Deserializer>();
            builder.Services.AddScoped<IIndexGenerator, IndexGenerator>();
            builder.Services.AddScoped<IFileCopier, FileCopier>();
            builder.Services.AddScoped<IHtmlTemplateParser, HtmlTemplateParser>();
            builder.Services.AddScoped<IFrontmatterExtractor, FrontmatterExtractor>();
            builder.Services.AddScoped<IMarkdownFileFactory, MarkdownFileFactory>();
            builder.Services.AddScoped<IMarkdownInvoker, MarkdownInvoker>();
            builder.Services.AddScoped<IMarkdownParser, MarkdownParser>();
            builder.Services.AddScoped<IMarkdownRendererService, MarkdownRendererService>();
            builder.Services.AddScoped<HtmlRenderer>(_ => new HtmlRenderer(new StringWriter()));
            builder.Services.AddScoped<MarkdownPipelineBuilder>(_ => new MarkdownPipelineBuilder().UseYamlFrontMatter());
            builder.Services.AddScoped<MarkdownPipeline>(provider =>
            {
                var pipelineBuilder = provider.GetService<MarkdownPipelineBuilder>();
                var renderer = provider.GetService<HtmlRenderer>();
                if (pipelineBuilder is null || renderer is null)
                {
                    throw new ArgumentException("Pipeline dependencies are missing");
                }
                
                var pipeline = pipelineBuilder.Build();
                pipeline.Setup(renderer);
                return pipeline;
            });
            
            // add command services
            builder.Services.AddScoped<GenerateProjectService>();
            builder.Services.AddScoped<BuildProjectService>();
            _host = builder.Build();

            if (_host is null)
            {
                throw new ArgumentException("Could not build the service container");
            }
            
            // run the console application
            Parser.Default
                .ParseArguments<BuildProject, GenerateProject>(args)
                .WithParsed(RunOptions);
        }
        catch (System.Exception e)
        {
            Console.WriteLine($"Failed to execute the command: {e.Message}");
            return 1;
        }

        return 0;
    }

    static void RunOptions(object optionType)
    {
        switch (optionType)
        {
            case BuildProject option:
                var buildProjectService = _host!.Services.GetService<BuildProjectService>();
                buildProjectService?.BuildProject(option.SourcePath, option.DestinationPath);
                break;
            case GenerateProject option:
                var generateProjectService = _host!.Services.GetService<GenerateProjectService>();
                generateProjectService?.GenerateProject(option.Path);
                break;
        }
    }
}
