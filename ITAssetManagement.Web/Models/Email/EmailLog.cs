using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Web.Models.Email
{
    public class EmailLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ToEmail { get; set; } = string.Empty;

        [Required]
        public string Subject { get; set; } = string.Empty;

        public string? Body { get; set; }

        [Required]
        public DateTime SentDate { get; set; } = DateTime.Now;

        public bool IsSuccess { get; set; }

        public string? ErrorMessage { get; set; }

        public string? RelatedEntityType { get; set; } // "Assignment", "Laptop" etc.

        public int? RelatedEntityId { get; set; }

        public int? RetryCount { get; set; }
    }
}