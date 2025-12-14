using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Strzelecki_Baranowski.DuckApp.CORE;

namespace Strzelecki_Baranowski.DuckApp.INTERFACES
{
    public interface IDataAccessObject
    {
        IEnumerable<IProducer> GetProducers();
        IEnumerable<IDuck> GetDucks();

        int AddProducer(IProducer producer/*string name,
            string website*/);

        int DeleteProducer(int id);

        int UpdateProducer(IProducer producer/*int id, string? name, string? website*/);

        int AddDuck(IDuck duck/*string name,
            int producerID,
            double price,
            string photo,
            string description,
            Category category*/);
        int DeleteDuck(int id);

        int UpdateDuck(IDuck duck/*int id,
            string? name,
            int? producerID,
            double? price,
            string? photo,
            string? description,
            Category? category
            */);

        IDuck GetNewDuck();
        IProducer GetNewProducer();

    }
}
