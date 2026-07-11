using Spectre.Console.Cli;

namespace DcsSymlinker.Commands;

public class DefaultCommand(IDirectoryService directoryService, IFileNameParser fileNameParser)
    : Command<DefaultOptions>
{
    protected override int Execute(CommandContext context, DefaultOptions defaultOptions, CancellationToken cancellationToken)
    {
        var workingDir = defaultOptions.Path ?? Directory.GetCurrentDirectory();

        var subDirectories = directoryService.GetAllSubDirectories(workingDir, defaultOptions.Folder);
        if (subDirectories.Count == 0)
        {
            return 1;
        }

        var realFiles = new List<FileInfo>();
        var files = directoryService.GetAllLuaFiles(subDirectories);
        foreach (var file in files)
        {
            // This is a symlink
            if (file.LinkTarget is not null)
            {
                continue;
            }

            realFiles.Add(file);
        }

        if (defaultOptions.AutomaticIds)
        {
            // Get all IDs
        }

        // foreach (var item in realFiles)
        // {
        //     var toCreate = fileNameParser.GetNewNames(realFiles, );
        //     foreach (var symbolicLink in toCreate)
        //     {
        //         File.CreateSymbolicLink(item.Name, symbolicLink);
        //     }
        // }
        //
        return 0;
    }
}