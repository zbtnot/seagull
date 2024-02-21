using CommandLine;

namespace Seagull.Model.Option;

[Verb("build", HelpText = "Build a project.")]
public class BuildProject
{
    [Value(0, MetaName = "src", Required = true, HelpText = "Path of the project to build.")]
    public string SourcePath { get; init; } = string.Empty;
    
    [Value(1, MetaName = "dst", Required = true, HelpText = "Path to store the built project.")]
    public string DestinationPath { get; init; } = string.Empty;
}
