using Microsoft.AspNetCore.Mvc.Rendering;
using Strzelecki_Baranowski.DuckApp.CORE; 

namespace Strzelecki_Baranowski.DuckApp.WebUI.Models
{
    public class DuckFilterViewModel
    {
        public int? ID { get; set; }
        public FilterMode IDMode { get; set; } = FilterMode.Equal;

        public decimal? Price { get; set; }
        public FilterMode PriceMode { get; set; } = FilterMode.Equal;

        public string? Name { get; set; }
        public FilterMode NameMode { get; set; } = FilterMode.Contains;

        public string? Description { get; set; }
        public FilterMode DescriptionMode { get; set; } = FilterMode.Contains;

        public Category? Category { get; set; }
        public int? ProducerID { get; set; }
    }

    public class DuckIndexViewModel
    {
        public List<Duck> Ducks { get; set; } = new List<Duck>();
        public DuckFilterViewModel Filter { get; set; } = new DuckFilterViewModel();
        public SelectList? ProducersList { get; set; }
    }
}
