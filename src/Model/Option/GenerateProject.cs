using CommandLine;

namespace Seagull.Model.Option;

[Verb("new", HelpText = "Generate a new project.")]
public class GenerateProject
{
    [Value(0, MetaName = "path", Required = true, HelpText = "Path to create the project at.")]
    public string Path { get; init; } = string.Empty;

    [Option(
        'f',
        "force",
        Required = false,
        HelpText = "Attempt to create the project, regardless of the path."
    )]
    public bool Force { get; init; } = false;
}
