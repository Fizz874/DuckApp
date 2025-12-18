using Microsoft.AspNetCore.Mvc.Rendering;
using Strzelecki_Baranowski.DuckApp.CORE;

namespace Strzelecki_Baranowski.DuckApp.WebUI.Models
{
    public class ProducerFilterViewModel
    {
        public int? ID { get; set; }
        public FilterMode IDMode { get; set; } = FilterMode.Equal;

        public string? Name { get; set; }
        public FilterMode NameMode { get; set; } = FilterMode.Contains;

        public string? Website { get; set; }
        public FilterMode WebsiteMode { get; set; } = FilterMode.Contains;
    }

    public class ProducerIndexViewModel
    {
        public List<Producer> Producers { get; set; } = new List<Producer>();
        public ProducerFilterViewModel Filter { get; set; } = new ProducerFilterViewModel();
    }
}