using Strzelecki_Baranowski.DuckApp.CORE;

namespace Strzelecki_Baranowski.DuckApp.INTERFACES
{
    public interface IDuck
    {
        string Name { get; set; }
        int ID { get; set; }
        int ProducerID { get; set; }
        double Price { get; set; }
        string Photo {  get; set; }
        string Description { get; set; }
        Category Category { get; set; }
    }
}
