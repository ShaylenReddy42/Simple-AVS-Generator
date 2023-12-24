namespace SimpleAVSGeneratorCore.Models;

public class InputFile
{
    public InputFileInfo FileInfo { get; set; } = new();

    public string HomeDir { get; set; } = string.Empty;
    public string OutputDir => Path.Combine(HomeDir, FileInfo.FileNameOnly);

    public string ScriptFile => Path.Combine(OutputDir, "Script.avs");

    // AVSMeter Properties
    public string AVSMeterScriptFile => Path.Combine(OutputDir, "AVSMeter.cmd");
    public static string AVSMeterScriptContent => @"AVSMeter64 ""%~dp0Script.avs"" -i -l";

    public VideoInfo Video { get; set; } = new();

    public AudioInfo Audio { get; set; } = new();

    public string? OutputContainer { get; set; }
}
