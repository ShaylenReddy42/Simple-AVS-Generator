using System.Text;
using SimpleAVSGeneratorCore.Models;

namespace SimpleAVSGeneratorCore;

public class AviSynthScript
{
    public bool CreateAviSynthScript { get; private set; } = default;
    public string AVSScriptFile { get; init; }
    public string AVSScriptContent { get; private set; } = string.Empty;

    public AviSynthScript(string scriptFile)
    {
        AVSScriptFile = scriptFile;
    }

    public void SetScriptContent(FileModel fileInfo, VideoModel video, AudioModel audio)
    {
        StringBuilder sb = new();
        
        sb.Append($"i = \"{fileInfo.FileName}\"\r\n\r\n");

        if (video.Enabled is true && video.MuxOriginalVideo is false)
        {
            sb.Append("v = LWLibavVideoSource(i).ConvertBits(8).ConvertToYV12()#.ShowFrameNumber()\r\n\r\n");
            
            sb.Append("# Calculate the target height based on a target width\r\n");
            sb.Append("aspectRatio  = float(Width(v)) / float(Height(v))\r\n");
            sb.Append($"targetWidth  = {(video.NeedsToBeResized ? "640" : "Width(v)")}\r\n");
            sb.Append("targetHeight = int(targetWidth / aspectRatio)\r\n");
            sb.Append("targetHeight = targetHeight + ((targetHeight % 2 != 0) ? 1 : 0)\r\n\r\n");

            sb.Append("v = Spline36Resize(v, targetWidth, targetHeight)\r\n\r\n");
        }

        if (audio.Enabled is true)
        {
            sb.Append("a = LWLibavAudioSource(i).ConvertAudioToFloat()\r\n\r\n");
            sb.Append("a = Normalize(a, 1.0)\r\n\r\n");
        }

        if ((video.Enabled is true && video.MuxOriginalVideo is false) && audio.Enabled is true)
        {
            sb.Append("o = AudioDub(v, a)\r\n\r\n");
            sb.Append("o = ConvertAudioTo16Bit(o)\r\n\r\n");
            sb.Append('o');

            CreateAviSynthScript = true;
        }
        else if ((video.Enabled is true && video.MuxOriginalVideo is false) && audio.Enabled is false)
        {
            sb.Append('v');

            CreateAviSynthScript = true;
        }
        else if ((audio.Enabled is true && video.Enabled is false) || (audio.Enabled is true && video.Enabled is true && video.MuxOriginalVideo is true))
        {
            sb.Append("a = ConvertAudioTo16Bit(a)\r\n\r\n");
            sb.Append('a');

            CreateAviSynthScript = true;
        }

        AVSScriptContent = sb.ToString();
    }
}
