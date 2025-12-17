using System.ComponentModel.DataAnnotations;

namespace Strzelecki_Baranowski.DuckApp.WebUI.Models
{
    public class Producer
    {
        public int ID { get; set; }

        [Display(Name = "Nazwa Firmy")]
        [Required(ErrorMessage = "Nazwa firmy jest wymagana.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Nazwa musi mieć od 2 do 100 znaków.")]
        public string Name { get; set; } = String.Empty;

        [Display(Name = "Strona WWW")]
        [Url(ErrorMessage = "Podaj poprawny adres URL (np. https://example.com)")]
        public string? Website { get; set; }

        // Ta lista posłuży tylko do wyświetlania w Details
        public List<Duck> Ducks { get; set; } = new List<Duck>();
    }
}
