namespace Strzelecki_Baranowski.DuckApp.WebUI.Models
{
    using System.ComponentModel.DataAnnotations; 
    using System.ComponentModel.DataAnnotations.Schema;
    using Strzelecki_Baranowski.DuckApp.CORE;

    public class Duck
        {
            public int ID { get; set; }

            [Display(Name = "Duck name")] 
            [Required(ErrorMessage = "Enter the duck name")] 
            [StringLength(100, MinimumLength = 1, ErrorMessage = "Name is too long")]
            public string Name { get; set; } = string.Empty;

            [Display(Name = "Cena (€)")]
            [Required]
            [Range(0.00 , 10000.00, ErrorMessage = "Price must be greater than 0.")]
            [RegularExpression(@"^\d+([\.,\,]\d{0,2})?$", ErrorMessage = "Enter the correct amount (max 2 decimal places).")]
            [DataType(DataType.Currency)]
            public decimal Price { get; set; }

            [Display(Name = "Category")]
            [Required]
            public Category Category { get; set; }

            [Display(Name = "Description")]
            [DataType(DataType.MultilineText)]
            public string? Description { get; set; } = string.Empty ;

            [Display(Name = "Photo")]
            public string? Photo { get; set; } = string.Empty;

            [NotMapped]
            public string PhotoFileName
            {
                get
                {
                if (string.IsNullOrEmpty(Photo))
                {
                    return "photos/0.png";
                }
                try
                {
                    return System.IO.Path.GetFileName(Photo);
                }
                catch
                {
                    return "photos/0.png";
                }
            }
            }

            [NotMapped]
            [Display(Name = "Change photo")]
            public IFormFile? PhotoUpload { get; set; }

            [Display(Name = "Producer")]
            [Required(ErrorMessage = "Select producer.")]
            public int ProducerID { get; set; }

            public virtual Producer? Producer { get; set; }
        }
    }
