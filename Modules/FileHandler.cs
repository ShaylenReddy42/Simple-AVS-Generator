namespace Simple_AVS_Generator.Modules
{
    internal class FileHandler
    {
        private bool isSupportedByMP4Box = default;
        
        public string FileName { get; private set; }
        public string FileExt { get; private set; }
        public string FileNameOnly { get; private set; }
        public int FileType { get; private set; }
        
        public FileHandler(string fileName)
        {
            FileName = fileName;
            FileExt = Path.GetExtension(FileName);
            FileNameOnly = Path.GetFileNameWithoutExtension(FileName);

            SupportedExts se = new();
            FileType = se.DetermineInputFileType(FileExt);
        }

        public bool IsSupportedByMP4Box
        {
            get => isSupportedByMP4Box;
            private set
            {
                string[] supportedExts =
                {
                    //Raw video extensions
                    ".M1V", ".M2V", //MPEG-1-2 Video
                    ".CMP", ".M4V", //MPEG-4 Video
                    ".263", ".H263", //H263 Video
                    ".H264", ".H26L", ".264", ".26L", ".X264", ".SVC", //AVC Video
                    ".HEVC", ".H265", ".265", ".HVC", ".SHVC", ".LHVC", ".MHVC", //HEVC Video
                    ".IVF", //AV1 and VP9 Video
                    ".OBU", //AV1 Video

                    //Containers
                    ".AVI",
                    ".MPG", ".MPEG", ".VOB", ".VCD", ".SVCD", //MPEG-2 Program Streams
                    ".TS", ".M2T", ".M2TS", //MPEG-2 Transport Streams
                    ".QCP",
                    ".OGG",
                    ".MP4", ".3GP", ".3G2" //Some ISO Media Extensions
                };

                foreach (string ext in supportedExts)
                {
                    if (FileExt.Equals(ext, StringComparison.CurrentCultureIgnoreCase))
                    {
                        IsSupportedByMP4Box = true;
                        break;
                    }
                }
            }
        }
    }
}
