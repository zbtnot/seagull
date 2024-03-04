using System.Text.RegularExpressions;
using Seagull.Service.Contract;

namespace Seagull.Service;

public partial class FileCopier(IFileService fileService) : IFileCopier
{
    private readonly Regex _filteredExtensions = FilterExtensionsRegex();
    
    public void CopyFiles(string src, string dest)
    {
        var files = fileService.ReadDirectoryContents(src);
        var filteredFiles = files.Where(file => _filteredExtensions.Match(file).Success);
        foreach (var file in filteredFiles)
        {
            var path = Path.Join(dest, file.Replace(src, string.Empty));
            fileService.CopyFile(file, path);
        }
    }

    [GeneratedRegex(@".+\.(?!(md|yml|scriban)$).+")]
    private static partial Regex FilterExtensionsRegex();
}
