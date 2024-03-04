using Seagull.Exception;
using Seagull.Service.Contract;

namespace Seagull.Service;

/**
 * Wrapper class for File management calls
 */
public class FileService : IFileService
{
    public void CreateDirectory(string path)
    {
        if (DirectoryExists(path))
        {
            throw new PathExistsException("The provided path already exists. Use -f to force.");
        }

        DirectoryCreate(path);
    }

    public void CreateTextFile(string path, string content)
    {
        var directoryPath = PathGetDirectoryName(path);
        if (!DirectoryExists(directoryPath))
        {
            CreateDirectory(directoryPath);
        }
        FileWriteText(path, content);
    }

    public string ReadTextFile(string path)
    {
        return FileReadText(path);
    }

    public IEnumerable<string> ReadDirectoryContents(string path, string pattern = "")
    {
        if (!DirectoryExists(path))
        {
            throw new PathExistsException("The path does not exist.");
        }

        return DirectoryEnumerateFiles(path, pattern);
    }

    public bool DirectoryPathExists(string path)
    {
        return DirectoryExists(path);
    }

    public void CopyFile(string src, string dest)
    {
        var directoryPath = PathGetDirectoryName(dest);
        if (!DirectoryExists(directoryPath))
        {
            CreateDirectory(directoryPath);
        }
        FileCopy(src, dest);
    }

    protected virtual string PathGetDirectoryName(string path)
    {
        var directoryName = Path.GetDirectoryName(path);
        if (directoryName == null)
        {
            throw new FileNotFoundException("Could not find a directory for the path specified.");
        }
        
        return directoryName;
    }

    protected virtual void FileWriteText(string path, string content)
    {
        File.WriteAllText(path, content);
    }

    protected virtual string FileReadText(string path)
    {
        return File.ReadAllText(path);
    }

    protected virtual void DirectoryCreate(string path)
    {
        Directory.CreateDirectory(path);
    }

    protected virtual bool DirectoryExists(string path)
    {
        return Directory.Exists(path);
    }

    protected virtual IEnumerable<string> DirectoryEnumerateFiles(string path, string pattern = "")
    {
        return Directory.EnumerateFiles(path, pattern, SearchOption.AllDirectories);
    }

    protected virtual void FileCopy(string src, string dest)
    {
        File.Copy(src, dest, true);
    }
}
