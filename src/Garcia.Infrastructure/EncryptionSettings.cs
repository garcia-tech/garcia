using System;
using System.ComponentModel.DataAnnotations;

namespace Garcia.Infrastructure
{
    public class EncryptionSettings
    {
        [Required]
        [StringLength(16, ErrorMessage = "Init vector should be 16 characters long", 
            ErrorMessageResourceType = typeof(InvalidOperationException))]
        public string InitVector { get; set; }
        [Required]
        public string PassPharse { get; set; }
    }
}
