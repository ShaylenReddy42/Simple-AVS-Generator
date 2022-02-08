namespace Simple_AVS_Generator.Core.Support
{
    internal class Video
    {
        public static object [,] sourceFPS =
        {
            { 24, "23.976 / 24" },
            { 25, "25"          },
            { 30, "29.97 / 30"  },
            { 60, "59.94"       }
        };

        public static object [,] keyframeInterval =
        {
            {  2, "2 Seconds"  },
            {  5, "5 Seconds"  },
            { 10, "10 Seconds" }
        };

        public static string [] outputVideoCodecs =
        {
            "HEVC",
            "AV1",
            "AVC",
            "WhatsApp",
            "Mux Original"
        };
    }
}
