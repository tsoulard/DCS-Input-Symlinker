// See https://aka.ms/new-console-template for more information
using DcsSymlinker;
using DcsSymlinker.Commands;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var services = new ServiceCollection();
services.AddScoped<IDeviceIdService, DeviceIdService>();
services.AddScoped<IDirectoryService, DirectoryService>();
services.AddScoped<IFileNameParser, FileNameParser>();
  
var registrar = new TypeRegistrar(services);
var app = new CommandApp<DefaultCommand>(registrar);
return app.Run(args);

// https://learn.microsoft.com/en-us/dotnet/api/system.io.file.createsymboliclink?view=net-8.0