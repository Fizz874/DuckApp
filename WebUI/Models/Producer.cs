using System.ComponentModel.DataAnnotations;

namespace Strzelecki_Baranowski.DuckApp.WebUI.Models
{
    public class Producer
    {
        public int ID { get; set; }

        [Display(Name = "Producer Name")]
        [Required(ErrorMessage = "Producer name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Website")]
        [Url(ErrorMessage = "Please enter a valid URL (e.g., https://example.com).")]
        public string? Website { get; set; }

        public List<Duck> Ducks { get; set; } = new List<Duck>();
    }
}