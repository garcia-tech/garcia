using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garcia.Application
{
    public class ImageResizeSettings
    {
        public bool PreserveOriginalFile { get; set; }
        public int MaximumHeight { get; set; }
        public int DefaultHeight { get; set; }
        public string ResizedFileSuffix { get; set; }
    }
}
