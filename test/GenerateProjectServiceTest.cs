using System.Linq.Expressions;
using Moq;
using Seagull.Model;
using Seagull.Service;
using Seagull.Service.Contract;
using YamlDotNet.Serialization;

namespace test;

[TestClass]
public class GenerateProjectServiceTest
{
    private readonly Mock<ISerializer> _serializer = new();
    private readonly Mock<IFileService> _fileService = new();

    [TestMethod]
    public void TestGenerateProject()
    {
        var config = new Configuration()
        {
            Title = "Test",
        };
        var path = "/foo/bar";
        var fileContent = $"title: {config.Title}";
        var configFilePath = $"{path}/seagull.yml";
        var layoutFilePath = $"{path}/layout.scriban";

        Expression<Func<ISerializer, string>> serialize = s => s.Serialize(It.IsAny<Configuration>());
        _serializer.Setup(serialize).Returns(fileContent);

        Expression<Action<IFileService>> createDir = fs => fs.CreateDirectory(path);
        Expression<Action<IFileService>> createConfigFile = fs => fs.CreateTextFile(configFilePath, fileContent);
        Expression<Action<IFileService>> createLayoutFile = fs => fs.CreateTextFile(layoutFilePath, It.IsAny<string>());
        _fileService.Setup(createDir);
        _fileService.Setup(createConfigFile);
        _fileService.Setup(createLayoutFile);

        var generateProjectService = new GenerateProjectService(_serializer.Object, _fileService.Object);
        generateProjectService.GenerateProject(path);

        _serializer.Verify(serialize, Times.Once);
        _fileService.Verify(createDir, Times.Once);
        _fileService.Verify(createConfigFile, Times.Once);
        _fileService.Verify(createLayoutFile, Times.Once);
    }
}
