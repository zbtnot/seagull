using CommandLine;
using Seagull.Model.Option;

namespace Seagull;

internal static class Program
{
    public static int Main(string[] args)
    {
        try
        {
            Parser.Default
                .ParseArguments<BuildProject, GenerateProject>(args)
                .WithParsed(RunOptions);
        }
        catch (Exception e)
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
                break;
            case GenerateProject option:
                break;
        }
    }
}
