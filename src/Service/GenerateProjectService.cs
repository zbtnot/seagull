using Seagull.Model;
using YamlDotNet.Serialization;

namespace Seagull.Service;

public class GenerateProjectService(ISerializer serializer, IFileService fileService)
{
    public void GenerateProject(string path)
    {
        fileService.CreateDirectory(path);
        fileService.CreateTextFile(Path.Join(path, "seagull.yml"), GenerateDefaultConfiguration());
    }

    protected string GenerateDefaultConfiguration()
    {
        var config = new Configuration()
        {
            Title = "Project title",
        };
        
        return serializer.Serialize(config);
    }
}
