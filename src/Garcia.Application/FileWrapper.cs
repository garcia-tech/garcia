namespace Garcia.Application
{
    public class FileWrapper
    {
        public string FileName { get; private set; }
        public string BasePath { get; private set; }
        public string FullPath { get; private set; }
        public string Url { get; private set; }
        public string Extension { get; private set; }

        public FileWrapper(string fileName, string basePath, string fullPath, string url, string extension)
        {
            FileName = fileName;
            BasePath = basePath;
            FullPath = fullPath;
            Url = url;
            Extension = extension;
        }

        public FileWrapper()
        {
        }
    }
}
