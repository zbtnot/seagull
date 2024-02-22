using Seagull.Service;

namespace test;

[TestClass]
public class FileServiceTest
{
    [TestMethod]
    public void TestCreateDirectory()
    {
        var fileService = new FileServiceWithMocks();

        fileService.CreateDirectory("/foo/bar");

        Assert.AreEqual(1, fileService.DirectoryExistsCalls);
        Assert.AreEqual(1, fileService.DirectoryCreateCalls);
    }

    [TestMethod]
    public void TestCreateFile()
    {
        var fileService = new FileServiceWithMocks();

        fileService.CreateTextFile("/foo/bar/baz.txt", "some text");

        Assert.AreEqual(1, fileService.FileWriteTextCalls);
    }
}

// Wrapper class for FileService that allows us to stub out the virtual method calls.
class FileServiceWithMocks : FileService
{
    public bool DirectoryExistsReturn { get; set; }
    public int DirectoryCreateCalls { get; set; }
    public int DirectoryExistsCalls { get; set; }
    public int FileWriteTextCalls { get; set; }

    protected override void DirectoryCreate(string path)
    {
        DirectoryCreateCalls++;
    }

    protected override bool DirectoryExists(string path)
    {
        DirectoryExistsCalls++;
        return DirectoryExistsReturn;
    }

    protected override void FileWriteText(string path, string content)
    {
        FileWriteTextCalls++;
    }
}
