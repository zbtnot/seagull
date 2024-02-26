namespace Seagull.Service;

public interface IFileService
{
    public void CreateDirectory(string path);
    public void CreateTextFile(string path, string content);
    public string ReadTextFile(string path);
    public IEnumerable<string> ReadDirectoryContents(string path, string pattern = "");
    public bool DirectoryPathExists(string path);
}
