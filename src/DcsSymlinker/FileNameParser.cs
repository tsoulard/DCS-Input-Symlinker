namespace DcsSymlinker;

public class FileNameParser : IFileNameParser
{
    public List<string> GetExistingIds(List<FileInfo> files)
    {
        var ids = new List<string>();
        foreach (var file in files)
        {
            var id = GetNamePart(file.Name, FileNamePart.Id);


            ids.Add(id);
        }

        return ids;
    }

    public List<string> GetNewNames(FileInfo fileInfo, List<string> ids)
    {
        var newNames = new List<string>();
        var deviceName = GetNamePart(fileInfo.Name, FileNamePart.DeviceName);

        foreach (var id in ids)
        {
            newNames.Add($"{deviceName} {{{id}}}.diff.lua");
        }

        return newNames;
    }

    private static string GetNamePart(string name, FileNamePart part)
    {
        var splitName = name.Split(['{', '}'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (splitName.Length != 3)
        {
            throw new InvalidOperationException($"Unable to parse file name {name}");
        }

        return splitName[(int)part];
    }
}

public interface IFileNameParser
{
    List<string> GetExistingIds(List<FileInfo> files);
    List<string> GetNewNames(FileInfo fileInfo, List<string> ids);
}
