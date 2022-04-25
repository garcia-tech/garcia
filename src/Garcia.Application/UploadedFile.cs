using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
