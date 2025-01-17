using System.Buffers.Binary;
using System.IO.Compression;
using System.Globalization;

using Microsoft.Extensions.Logging;

using CommandLine;

using NLog;
using NLog.Extensions.Logging;

using System.Diagnostics;

namespace ACEvo.Package.CLI;

public class Program
{
    public const string Version = "1.0.0";

    private static ILoggerFactory _loggerFactory;
    private static Microsoft.Extensions.Logging.ILogger _logger;

    static void Main(string[] args)
    {
        _loggerFactory = LoggerFactory.Create(builder => builder.AddNLog());
        _logger = _loggerFactory.CreateLogger<Program>();

        Console.WriteLine("-----------------------------------------");
        Console.WriteLine($"- ACEvo.Package.CLI {Version} by Nenkai");
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine("- https://github.com/Nenkai");
        Console.WriteLine("- https://twitter.com/Nenkaai");
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine("");

        var p = Parser.Default.ParseArguments<UnpackFileVerbs, UnpackVerbs, ListFilesVerbs>(args)
            .WithParsed<UnpackFileVerbs>(UnpackFile)
            .WithParsed<UnpackVerbs>(Unpack)
            .WithParsed<ListFilesVerbs>(ListFiles);
    }

    static void UnpackFile(UnpackFileVerbs verbs)
    {
        if (!File.Exists(verbs.InputFile))
        {
            _logger.LogError("File '{path}' does not exist", verbs.InputFile);
            return;
        }

        if (string.IsNullOrEmpty(verbs.OutputPath))
        {
            string inputFileName = Path.GetFileNameWithoutExtension(verbs.InputFile);
            verbs.OutputPath = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(verbs.InputFile)), $"{inputFileName}.extracted");
        }

        try
        {
            using var pack = PackFile.Open(verbs.InputFile, _loggerFactory);

            _logger.LogInformation("Starting unpack process.");
            if (!pack.ExtractFile(verbs.FileToUnpack, verbs.OutputPath))
                _logger.LogWarning("File '{file}' not found in pack.", verbs.FileToUnpack);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to unpack.");
        }
    }

    static void Unpack(UnpackVerbs verbs)
    {
        if (!File.Exists(verbs.InputFile))
        {
            _logger.LogError("File '{path}' does not exist", verbs.InputFile);
            return;
        }

        if (string.IsNullOrEmpty(verbs.OutputPath))
        {
            string inputFileName = Path.GetFileNameWithoutExtension(verbs.InputFile);
            verbs.OutputPath = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(verbs.InputFile)), $"{inputFileName}.extracted");
        }

        try
        {
            using var pack = PackFile.Open(verbs.InputFile, _loggerFactory);

            _logger.LogInformation("Starting unpack process.");
            pack.ExtractAll(verbs.OutputPath);
            _logger.LogInformation("Done.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to unpack.");
        }
    }

    static void ListFiles(ListFilesVerbs verbs)
    {
        if (!File.Exists(verbs.InputFile))
        {
            _logger.LogError("File '{path}' does not exist", verbs.InputFile);
            return;
        }

        try
        {
            using var pack = PackFile.Open(verbs.InputFile, _loggerFactory);

            string inputFileName = Path.GetFileNameWithoutExtension(verbs.InputFile);
            string outputPath = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(verbs.InputFile)), $"{inputFileName}_files.txt");
            pack.ListFiles(outputPath);
            _logger.LogInformation("Done. ({path})", outputPath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to read pack.");
        }
    }
}

[Verb("unpack", HelpText = "Unpacks a .kspkg file.")]
public class UnpackVerbs
{
    [Option('i', "input", Required = true, HelpText = "Input .kspkg file")]
    public string InputFile { get; set; }

    [Option('o', "output", HelpText = "Output directory. Optional, defaults to a folder named the same as the .kspkg file.")]
    public string OutputPath { get; set; }
}

[Verb("unpack-file", HelpText = "Unpacks a specific file from a .kspkg pack.")]
public class UnpackFileVerbs
{
    [Option('i', "input", Required = true, HelpText = "Input .kspkg pack")]
    public string InputFile { get; set; }

    [Option('f', "file", Required = true, HelpText = "File to unpack.")]
    public string FileToUnpack { get; set; }

    [Option('o', "output", HelpText = "Optional. Output directory.")]
    public string OutputPath { get; set; }
}

[Verb("list-files", HelpText = "List files in a .kspkg pack.")]
public class ListFilesVerbs
{
    [Option('i', "input", Required = true, HelpText = "Input .kspkg pack")]
    public string InputFile { get; set; }
}