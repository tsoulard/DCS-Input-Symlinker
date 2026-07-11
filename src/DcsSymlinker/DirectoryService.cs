namespace DcsSymlinker;

public class DirectoryService : IDirectoryService
{
    public List<string> GetAllSubDirectories(string currentWorkingDirectory, InputFolder inputFolder)
    {
        var found = new List<string>();
        string[] directories;
        if (inputFolder == InputFolder.All)
        {
            directories = Directory.GetDirectories(currentWorkingDirectory, "*", SearchOption.AllDirectories);
        }
        else
        {
            directories = Directory.GetDirectories(currentWorkingDirectory, inputFolder.ToString(), SearchOption.AllDirectories);
        }

        foreach (var directory in directories)
        {
            found.Add(directory);
        }

        return found;
    }

    public List<FileInfo> GetAllLuaFiles(List<string> directories)
    {
        var files = new List<FileInfo>();
        foreach (var directory in directories)
        {
            var fileInfos = Directory.GetFiles(directory, "*.diff.lua").Select(x => new FileInfo(x));
            files.AddRange(fileInfos);
        }

        return files;
    }

    public List<string> GetSymbolicLinks(List<string> filePath)
    {
        var symbolicLinks = new List<string>();
        foreach (var path in filePath)
        {
            var fileInfo = new FileInfo(path);
            if (fileInfo.LinkTarget is not null)
            {
                symbolicLinks.Add(path);
            }
        }

        return symbolicLinks;
    }
}

public interface IDirectoryService
{
    List<string> GetAllSubDirectories(string currentWorkingDirectory, InputFolder inputFolder);
    List<FileInfo> GetAllLuaFiles(List<string> directories);
    List<string> GetSymbolicLinks(List<string> filePath);
}