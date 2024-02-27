using System.Linq.Expressions;
using Moq;
using Seagull.Model;
using Seagull.Service;
using YamlDotNet.Serialization;

namespace test;

[TestClass]
public class BuildProjectServiceTest
{
    protected Mock<IDeserializer> _deserializer = new();
    protected Mock<IFileService> _fileService = new();
    protected Mock<IMarkdownRendererService> _markdownRendererService = new();

    [TestMethod]
    public void TestBuildProject()
    {
        var src = "/tmp/foo";
        var dest = "/tmp/bar";

        var srcFilesYml = new[]
        {
            $"{src}/seagull.yml",
        };
        var srcFilesMd = new[]
        {
            $"{src}/test.md",
        };
        var allSrcFiles = new List<string>();
        allSrcFiles.AddRange(srcFilesYml);
        allSrcFiles.AddRange(srcFilesMd);
        var destFilesHtml = new[]
        {
            $"{dest}/test.html",
        };
        var pageFile = new Page
        {
            Content = string.Empty,
            Path = destFilesHtml[0],
            Title = string.Empty,
        };
        
        // mocks
        Expression<Func<IFileService, IEnumerable<string>>> readDirectoryContents =
            fs => fs.ReadDirectoryContents(src, string.Empty);
        _fileService.Setup(readDirectoryContents).Returns(allSrcFiles);
        
        Expression<Func<IFileService, IEnumerable<string>>> readDirectoryContentsMdFiltered =
            fs => fs.ReadDirectoryContents(src, "*.md");
        _fileService.Setup(readDirectoryContentsMdFiltered).Returns(srcFilesMd);

        Expression<Action<IFileService>> createTextFile =
            fs => fs.CreateTextFile(It.IsAny<string>(), It.IsAny<string>());
        _fileService.Setup(createTextFile);

        Expression<Func<IFileService, string>> readTextFile = fs => fs.ReadTextFile(It.IsAny<string>());
        _fileService.Setup(readTextFile).Returns(string.Empty);

        Expression<Func<IDeserializer, Configuration>> deserialize = ds =>
            ds.Deserialize<Configuration>(It.IsAny<string>());
        _deserializer.Setup(deserialize).Returns(new Configuration());

        Expression<Func<IMarkdownRendererService, Page>> renderAsPage = mdrs =>
            mdrs.RenderAsPage(It.IsAny<MarkdownFile>());
        _markdownRendererService.Setup(renderAsPage).Returns(pageFile);
        
        var buildProjectService =
            new BuildProjectService(_deserializer.Object, _fileService.Object, _markdownRendererService.Object);
        buildProjectService.BuildProject(src, dest);

        _fileService.Verify(readDirectoryContents, Times.Once);
        _fileService.Verify(readDirectoryContentsMdFiltered, Times.Once);
        _fileService.Verify(createTextFile, Times.Once);
        _fileService.Verify(readTextFile, Times.Exactly(2));
        _deserializer.Verify(deserialize, Times.Once);
        _markdownRendererService.Verify(renderAsPage, Times.Once);
    }
}
