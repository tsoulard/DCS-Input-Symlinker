using Spectre.Console;
using Spectre.Console.Cli;

namespace DcsSymlinker.Commands;

public class DefaultCommand(IDirectoryService directoryService, IFileNameParser fileNameParser)
    : Command<DefaultOptions>
{
    protected override int Execute(CommandContext context, DefaultOptions settings,
        CancellationToken cancellationToken)
    {
        var savedGamesDirectory = settings.Path ?? directoryService.GetSavedGamesDirectory() ?? Directory.GetCurrentDirectory();
        AnsiConsole.MarkupLine($"[green]Accessing Directory: {savedGamesDirectory}[/]");
        
        var subDirectories = directoryService.GetAllSubDirectories(savedGamesDirectory, settings.Folder);
        if (subDirectories.Count == 0)
        {
            return 1;
        }

        var files = directoryService.GetAllLuaFiles(subDirectories);

        var ids = fileNameParser.GetExistingIds(files);

        var realFiles = files.Where(f => f.LinkTarget is null).ToList();
        var symLinks = files.Where(f => f.LinkTarget is not null).ToList();

        // This flow might be quite complicated
        if (settings.LatestIdsOnly)
        {
            var latestFiles = GetMostRecentFilesByDeviceName(realFiles);
            var createdToday = latestFiles.Where(f => f.CreationTimeUtc > DateTime.UtcNow.Date).ToList();

            if (createdToday.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No latest files found.[/]");
                return 1;
            }

            ids = fileNameParser.GetExistingIds(createdToday);
        }

        if (settings.CleanOldLinks)
        {
            foreach (var file in symLinks)
            {
                file.Delete();
            }
        }

        foreach (var realFile in realFiles)
        {
            var toCreate = fileNameParser.GetNewNames(realFile, ids);
            foreach (var symbolicLink in toCreate)
            {
                var symbolicLinkPath = Path.Combine(realFile.Directory!.FullName, symbolicLink);
                if (symbolicLinkPath.Equals(realFile.FullName, StringComparison.Ordinal))
                {
                    continue;
                }

                if (File.Exists(symbolicLinkPath))
                {
                    File.Delete(symbolicLinkPath);
                }

                File.CreateSymbolicLink(symbolicLinkPath, realFile.FullName);
            }
        }

        return 0;
    }

    private List<FileInfo> GetMostRecentFilesByDeviceName(List<FileInfo> files)
    {
        var latestFiles = new Dictionary<string, FileInfo>();
        foreach (var file in files)
        {
            var deviceName = fileNameParser.GetDeviceName(file);
            if (!string.IsNullOrEmpty(deviceName) && !latestFiles.TryAdd(deviceName, file))
            {
                var fileInfo = latestFiles[deviceName];
                if (fileInfo.CreationTimeUtc < file.CreationTimeUtc)
                {
                    latestFiles[deviceName] = file;
                }
            }
        }

        return latestFiles.Values.ToList();
    }
}