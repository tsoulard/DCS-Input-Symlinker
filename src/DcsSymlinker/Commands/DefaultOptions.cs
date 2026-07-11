using System.ComponentModel;
using Spectre.Console.Cli;

namespace DcsSymlinker.Commands;

public class DefaultOptions : CommandSettings
{
    [CommandOption("-p|--path")]
    [Description("The path to your DCS Saves")]
    public string? Path { get; init; }
    
    [CommandOption("-f|--folder")]
    [DefaultValue(InputFolder.Joystick)]
    [Description("Which folders to look in to create symlinks")]
    public InputFolder Folder { get; init; }

    [CommandOption("-l|--latest-ids")]
    [Description("If provided only look for IDs on files created in the last day otheriwse look for all IDs including symlinks")]
    public bool LatestIdsOnly { get; init; }
    
    [CommandOption("-c|--clean")]
    [Description("Should we remove all existing symlinks and then create new ones")]
    public bool CleanOldLinks { get; init; }
}