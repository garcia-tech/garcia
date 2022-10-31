namespace Garcia.Application
{
    public class UploadedFile
    {
        public UploadedFile(string name, string fileName)
        {
            Name = name;
            FileName = fileName;
        }

        public string Name { get; set; }
        public string FileName { get; set; }
    }
}
