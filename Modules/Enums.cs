namespace Simple_AVS_Generator.Modules
{
    internal class Enums
    {
        public enum ExtensionTypes
        {
            CONTAINER = 0,
            VIDEO = 1,
            AUDIO = 2
        }

        public enum VideoCodecs
        {
            HEVC = 0,
            AV1 = 1,
            AVC = 2,
            WhatsApp = 3,
            Original = 4
        }

        public enum AudioCodecs
        {
            AAC_LC = 0,
            AAC_HE = 1,
            OPUS = 2
        }

        public enum OutputContainers
        {
            MP4 = 0,
            MKV = 1
        }
    }
}
