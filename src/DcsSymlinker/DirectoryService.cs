using DcsSymlinker.Models;
using Spectre.Console;
using VdfParser;

namespace DcsSymlinker;

public class DirectoryService : IDirectoryService
{
    private const string SteamGameIndex = "223750";
    private const string SavedGamesPath = "Saved Games/DCS/Config/Input";
    private const string SteamLinuxLibraryFolderConfigPath = ".local/share/Steam/config/libraryfolders.vdf";
    private const string BottlesDefaultPath = ".var/app/com.usebottles.bottles/data/bottles/bottles";
    private const string BottlesDataFilePath = ".var/app/com.usebottles.bottles/data/bottles/data.yml";

    public string? GetSavedGamesDirectory()
    {
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        if (OperatingSystem.IsWindows())
        {
            var savedGames = Path.Combine(userProfile, SavedGamesPath);

            if (Path.Exists(savedGames))
            {
                return savedGames;
            }
        } 
        else if (OperatingSystem.IsLinux())
        {
            return GetLinuxSavedGamesDirectory(userProfile);
        }

        return null;
    }

    public List<string> GetAllSubDirectories(string currentWorkingDirectory, InputFolder inputFolder)
    {
        var searchPattern = inputFolder == InputFolder.All ? "*" : inputFolder.ToString().ToLower();
        return  Directory.GetDirectories(currentWorkingDirectory, searchPattern, SearchOption.AllDirectories).ToList();
    }

    public List<FileInfo> GetAllLuaFiles(List<string> directories)
    {
        var files = new List<FileInfo>();
        foreach (var directory in directories)
        {
            var fileInfos = Directory.GetFiles(directory, "*.diff.lua").Select(f => new FileInfo(f));
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

    private static string? GetLinuxSavedGamesDirectory(string userProfile)
    {
        // Look in the steam library
        var libraryFolderConfigPath = Path.Combine(userProfile, SteamLinuxLibraryFolderConfigPath);
        if (File.Exists(libraryFolderConfigPath))
        {
            var testFile = File.OpenRead(libraryFolderConfigPath);
            var deserializer = new VdfDeserializer();
            var libraryConfig = deserializer.Deserialize<SteamLibraryConfig>(testFile);

            var libraryInfo =
                libraryConfig.LibraryFolders.Where(l => l.Value.Apps.ContainsKey(SteamGameIndex));

            foreach (var library in libraryInfo)   
            {
                var fullPath = Path.Combine(library.Value.Path, "steamapps/compatdata", SteamGameIndex,
                    "pfx/drive_c/users/steamuser", SavedGamesPath);
                if (Path.Exists(fullPath))
                {
                    return fullPath;
                }
            }
        }
        
        // Check the bottles config files in the user directory for the bottles config and then retrieve the custom bottles path value if present
        var customBottlesPath = File.ReadAllLines(Path.Combine(userProfile, BottlesDataFilePath)).FirstOrDefault(s => s.StartsWith("custom_bottles_path"));
        if (!string.IsNullOrEmpty(customBottlesPath))
        {
            var path = customBottlesPath.Split(':', StringSplitOptions.TrimEntries);
            var fullPath = FindInPrefix(path.Last());
            if (!string.IsNullOrWhiteSpace(fullPath))
            {
                return fullPath;
            }
        }
        
        // Check the default bottles location
        var bottlesDefaultPath = Path.Combine(userProfile, BottlesDefaultPath);
        return FindInPrefix(bottlesDefaultPath);;
    }

    private static string? FindInPrefix(string bottlesPath)
    {
        if (Path.Exists(bottlesPath))
        {
            var bottlesDirectories = Directory.GetDirectories(bottlesPath, "*", SearchOption.TopDirectoryOnly);
            foreach (var directory in bottlesDirectories)
            {
                var fullPath = Path.Combine(directory, "drive_c/users/steamuser", SavedGamesPath);
                if (Path.Exists(fullPath))
                {
                    return fullPath;
                }
            }
        }
        
        return null;
    }
}

public interface IDirectoryService
{
    string? GetSavedGamesDirectory();
    List<string> GetAllSubDirectories(string currentWorkingDirectory, InputFolder inputFolder);
    List<FileInfo> GetAllLuaFiles(List<string> directories);
    List<string> GetSymbolicLinks(List<string> filePath);
}