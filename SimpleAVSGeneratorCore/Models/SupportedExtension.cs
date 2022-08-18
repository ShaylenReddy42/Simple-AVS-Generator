namespace SimpleAVSGeneratorCore.Models;

public class SupportedExtension
{
    public string Extension { get; private set; }
    public string Type { get; private set; }
    public bool MP4BoxSupport { get; private set; }

    public SupportedExtension(
        string extension,
        string type,
        bool mp4boxSupport)
    {
        (Extension, Type, MP4BoxSupport) = (extension, type, mp4boxSupport);
    }
}
