using System.ComponentModel.DataAnnotations;
using Serilog;

namespace GarciaCore.Infrastructure.Logging.Serilog.Configurations
{
    public class FileLoggerConfiguration
    {
        [Required]
        public string Path { get; set; }
        public string OutputTemplate { get; set; } = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
        public int RetainedFileCountLimit { get; set; } = 31;
        public long FileSizeLimitBytes{get;set;} = 1073741824L;
        public RollingInterval Interval { get; set; } = RollingInterval.Infinite;
    }
}