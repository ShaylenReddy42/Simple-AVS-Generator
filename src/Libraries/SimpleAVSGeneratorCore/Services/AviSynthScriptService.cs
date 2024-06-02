using System.Text;
using SimpleAVSGeneratorCore.Models;

namespace SimpleAVSGeneratorCore.Services;

public class AviSynthScriptService
{
    public bool CreateAviSynthScript { get; private set; } = default;
    public string AVSScriptContent { get; private set; } = string.Empty;

    public Task SetScriptContentAsync(InputFileInfo fileInfo, VideoInfo video, AudioInfo audio)
    {
        var avsScriptBuilder = new StringBuilder();

        avsScriptBuilder.Append($"i = \"{fileInfo.FileName}\"\r\n\r\n");

        if (video.Enabled && !video.MuxOriginalVideo)
        {
            avsScriptBuilder.Append("v = LWLibavVideoSource(i, cachedir=\".\").ConvertBits(8).ConvertToYV12()\r\n\r\n");

            avsScriptBuilder.Append("#v = AutoCrop(v, 0)#.Crop(0, 0, -0, -0)\r\n\r\n");

            avsScriptBuilder.Append("#v = ShowFrameNumber(v)\r\n\r\n");

            avsScriptBuilder.Append("# Calculate the target height based on a target width\r\n");
            avsScriptBuilder.Append("aspectRatio  = float(Width(v)) / float(Height(v))\r\n");
            avsScriptBuilder.Append($"targetWidth  = {(video.NeedsToBeResized ? "640" : "Width(v)")}\r\n");
            avsScriptBuilder.Append("targetHeight = int(targetWidth / aspectRatio)\r\n");
            avsScriptBuilder.Append("targetHeight = targetHeight + ((targetHeight % 2 != 0) ? 1 : 0)\r\n\r\n");

            avsScriptBuilder.Append("v = Spline36Resize(v, targetWidth, targetHeight)\r\n\r\n");
        }

        if (audio.Enabled)
        {
            avsScriptBuilder.Append("a = LWLibavAudioSource(i, cachedir=\".\").ConvertAudioToFloat()\r\n\r\n");
            avsScriptBuilder.Append("a = Normalize(a, 1.0)\r\n\r\n");
        }

        if (video.Enabled && !video.MuxOriginalVideo && audio.Enabled)
        {
            avsScriptBuilder.Append("o = AudioDub(v, a)\r\n\r\n");
            avsScriptBuilder.Append("o = ConvertAudioTo16Bit(o)\r\n\r\n");
            avsScriptBuilder.Append('o');

            CreateAviSynthScript = true;
        }
        else if (video.Enabled && !video.MuxOriginalVideo && !audio.Enabled)
        {
            avsScriptBuilder.Append('v');

            CreateAviSynthScript = true;
        }
        else if ((audio.Enabled && !video.Enabled) || (audio.Enabled && video.Enabled && video.MuxOriginalVideo))
        {
            avsScriptBuilder.Append("a = ConvertAudioTo16Bit(a)\r\n\r\n");
            avsScriptBuilder.Append('a');

            CreateAviSynthScript = true;
        }

        AVSScriptContent = avsScriptBuilder.ToString();

        return Task.CompletedTask;
    }
}
