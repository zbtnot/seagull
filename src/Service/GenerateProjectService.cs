using Seagull.Model;
using Seagull.Service.Contract;
using YamlDotNet.Serialization;

namespace Seagull.Service;

public class GenerateProjectService(ISerializer serializer, IFileService fileService)
{
    public void GenerateProject(string path)
    {
        fileService.CreateDirectory(path);
        fileService.CreateTextFile(Path.Join(path, "seagull.yml"), GenerateDefaultConfiguration());
        fileService.CreateTextFile(Path.Join(path, "layout.html"), GenerateDefaultHtmlLayout());
    }

    protected string GenerateDefaultConfiguration()
    {
        var config = new Configuration()
        {
            Title = "Project title",
            Templates = new Dictionary<string, string>
            {
                ["default"] = "layout.html",
            }
        };

        return serializer.Serialize(config);
    }

    protected string GenerateDefaultHtmlLayout()
    {
        return
            """
            <!DOCTYPE html>
            <html lang="en">
                <head>
                    <title>
                        {{ title }}
                    </title>
                </head>
                <body>
                {{ content }}
                </body>
            </html>
            """;
    }
}
