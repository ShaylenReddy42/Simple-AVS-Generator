using System.Text;

namespace Simple_AVS_Generator.Core
{
    internal class AviSynthScript
    {
        private string? InputFile { get; set; }
        
        private bool Video { get; set; }
        private bool MuxOriginalVideo { get; set; }
        private bool NeedsToBeResized { get; set; }
        private bool Audio { get; set; }

        public string? ScriptFile { get; set; }
        public string? ScriptContent { get; private set; }

        private string I => $"i = \"{InputFile}\"\r\n\r\n";

        public AviSynthScript(Common common)
        {
            InputFile = common.FileName;
            Video = common.Video;
            MuxOriginalVideo = common.MuxOriginalVideo;
            NeedsToBeResized = common.NeedsToBeResized;
            Audio = common.Audio;

            ScriptFile = common.ScriptFile;
        }

        public void SetScriptContent()
        {
            StringBuilder sb = new();
            
            sb.Append(I);

            if (Video is true && MuxOriginalVideo is false)
            {
                sb.Append("v = LWLibavVideoSource(i).ConvertBits(8).ConvertToYV12()#.ShowFrameNumber()\r\n\r\n");
                sb.Append(ResizeVideo());
            }

            if (Audio)
            {
                sb.Append("a = LWLibavAudioSource(i).ConvertAudioToFloat()\r\n\r\n");
                sb.Append("a = Normalize(a, 1.0)\r\n\r\n");
                sb.Append("a = ConvertAudioTo16Bit(a)\r\n\r\n");
            }

            if ((Video is true && MuxOriginalVideo is false) && Audio)
            {
                sb.Append("o = AudioDub(v, a)\r\n\r\n");
                sb.Append("o = ConvertAudioTo16Bit(o)\r\n\r\n");
                sb.Append("o");
            }
            else if ((Video is true && MuxOriginalVideo is false) && Audio is false)
            {
                sb.Append("v");
            }
            else if (Audio is true && Video is false)
            {
                sb.Append("a");
            }

            ScriptContent = sb.ToString();
        }

        string ResizeVideo()
        {
            StringBuilder sb = new();

            sb.Append("# Calculate the target height based on a target width\r\n");
            sb.Append("aspectRatio  = float(Width(v)) / float(Height(v))\r\n");
            sb.Append($"targetWidth  = {(NeedsToBeResized ? "640" : "Width(v)")}\r\n");
            sb.Append("targetHeight = int(targetWidth / aspectRatio)\r\n");
            sb.Append("targetHeight = targetHeight + ((targetHeight % 2 != 0) ? 1 : 0)\r\n\r\n");
            
            sb.Append("v = Spline36Resize(v, targetWidth, targetHeight)\r\n\r\n");
            
            return sb.ToString();
        }
    }
}
