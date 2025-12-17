namespace Strzelecki_Baranowski.DuckApp.WebUI.Models
{
    using System.ComponentModel.DataAnnotations; // Ważne: To dodaje walidację
    using System.ComponentModel.DataAnnotations.Schema; // Dla mapowania bazy danych
    using Strzelecki_Baranowski.DuckApp.CORE;

    public class Duck
        {
            public int ID { get; set; }

            [Display(Name = "Duck name")] // Etykieta w HTML (<label>)
            [Required(ErrorMessage = "Proszę podać nazwę kaczki.")] // Pole wymagane
            [StringLength(100, MinimumLength = 1, ErrorMessage = "Nazwa jest zbyt długa.")]

            public string Name { get; set; } = String.Empty;

            [Display(Name = "Cena (€)")]
            [Required]
            [Range(0.00 , 10000.00, ErrorMessage = "Price must be greater than 0.")]
        [RegularExpression(@"^\d+([\.,\,]\d{0,2})?$", ErrorMessage = "Podaj poprawną kwotę (max 2 miejsca po przecinku).")]
        [DataType(DataType.Currency)] // Powie przeglądarce, żeby sformatować to jako walutę
            public decimal Price { get; set; }

            [Display(Name = "Kategoria")]
            [Required]
            public Category Category { get; set; } // Zakładam, że masz enum DuckCategory

            [Display(Name = "Opis")]
            [DataType(DataType.MultilineText)] // Wygeneruje <textarea> zamiast <input>
            public string? Description { get; set; } = String.Empty ;

        // --- ZDJĘCIE ---
        // W bazie trzymamy ścieżkę (string)
        [Display(Name = "Zdjęcie")]
        public string? Photo { get; set; } = String.Empty;

            [NotMapped]
            public string PhotoFileName
            {
                get
                {
                if (string.IsNullOrEmpty(Photo))
                {
                    return "photos/0.png"; // Jeśli brak zdjęcia, zwróć placeholder bezpiecznie
                }

                // 2. Dopiero jak mamy pewność, że jest tekst, wywołujemy GetFileName
                try
                {
                    return System.IO.Path.GetFileName(Photo);
                }
                catch
                {
                    // Jeśli ścieżka jest "śmieciem" (np. złe znaki), też zwróć placeholder, żeby nie wywalić strony
                    return "photos/0.png";
                }
            }
            }

            [NotMapped]
            [Display(Name = "Zmień zdjęcie")]
            public IFormFile? PhotoUpload { get; set; }

        // --- RELACJA Z PRODUCENTEM ---

        [Display(Name = "Producent")]
            [Required(ErrorMessage = "Wybierz producenta.")]
            public int ProducerID { get; set; } // Klucz obcy

            // Opcjonalnie: Obiekt producenta (jeśli DAO potrafi go zaciągnąć)
            // [ForeignKey("ProducerID")]
            public virtual Producer? Producer { get; set; }
        }

        //// Twój istniejący Enum (możesz go przekopiować lub użyć z Core)
        //public enum DuckCategory
        //{
        //    Rubber,
        //    Wood,
        //    Plastic,
        //    Premium
        //}
    }
