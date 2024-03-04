using System.Linq.Expressions;
using Moq;
using Seagull.Service;
using Seagull.Service.Contract;

namespace test;

[TestClass]
public class FileCopierTest
{
    private readonly Mock<IFileService> _fileService = new();

    [TestMethod]
    public void TestFileCopy()
    {
        var files = new List<string>
        {
            "foo.md",
            "layout.scriban",
            "seagull.yml",
            "cat.png",
        };

        Expression<Func<IFileService, IEnumerable<string>>> readDirectory = fs =>
            fs.ReadDirectoryContents(It.IsAny<string>(), It.IsAny<string>());
        _fileService.Setup(readDirectory).Returns(files);

        Expression<Action<IFileService>> copyFile = fs => fs.CopyFile(It.IsAny<string>(), It.IsAny<string>());
        _fileService.Setup(copyFile);

        var fileCopier = new FileCopier(_fileService.Object);
        fileCopier.CopyFiles("foo", "bar");
        
        _fileService.Verify(readDirectory, Times.Once);
        _fileService.Verify(copyFile, Times.Once);
    }
}
