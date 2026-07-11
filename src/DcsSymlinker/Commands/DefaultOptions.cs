using Spectre.Console.Cli;

namespace DcsSymlinker.Commands;

public class DefaultOptions : CommandSettings
{
    [CommandArgument(0, "<PATH>")]
    public string? Path { get; init; }
    
    [CommandOption("-f|--folder")]
    public InputFolder Folder { get; init; }

    [CommandOption("-a|--auto-ids")]
    public bool AutomaticIds { get; init; }
}