namespace DcsSymlinker.Models;

public class SteamLibraryConfig
{
	public Dictionary<int, SteamLibraryInfo> LibraryFolders { get; set; }
}

public class SteamLibraryInfo
{
	public string Path { get; set; }
	public string Label { get; set; }
	public string ContentId { get; set; }
	public string TotalSize { get; set; }
	public Dictionary<string,string> Apps { get; set; }
}