using Spectre.Console;

namespace DcsSymlinker;

public class FileNameParser : IFileNameParser
{
    public List<string> GetExistingIds(List<FileInfo> files)
    {
        var ids = new List<string>();
        foreach (var file in files)
        {
            if (GetNamePart(file.Name, FileNamePart.Id) is {} id)
            {
                // Only care about unique IDs
                if (!ids.Contains(id))
                {
                    ids.Add(id);
                }
            };
        }

        return ids;
    }

    public string GetDeviceName(FileInfo fileInfo)
    {
        return GetNamePart(fileInfo.Name, FileNamePart.DeviceName) ?? string.Empty;
    }

    public List<string> GetNewNames(FileInfo fileInfo, List<string> ids)
    {
        var newNames = new List<string>();
        var deviceName = GetNamePart(fileInfo.Name, FileNamePart.DeviceName);
        if (deviceName is null)
        {
            return [];
        }
        
        foreach (var id in ids)
        {
            newNames.Add($"{deviceName} {{{id}}}.diff.lua");
        }

        return newNames;
    }

    private static string? GetNamePart(string name, FileNamePart part)
    {
        var splitName = name.Split(['{', '}'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (splitName.Length != 3)
        {
            return null;
        }
        
        return splitName[(int)part];
    }
}

public interface IFileNameParser
{
    List<string> GetExistingIds(List<FileInfo> files);
    string GetDeviceName(FileInfo fileInfo);
    List<string> GetNewNames(FileInfo fileInfo, List<string> ids);
}
