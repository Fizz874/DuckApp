using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE;

namespace INTERFACES
{
    public interface IDataAccessObject
    {
        IEnumerable<IProducer> GetProducers();
        IEnumerable<IDuck> GetDucks();

        int AddProducer(string name,
            string website, 
            int id);

        int DeleteProducer(int id);

        int UpdateProducer(int id, string? name, string? website);

        int AddDuck(string name,
            int iD, int producerID,
            double price,
            string photo,
            string description,
            Category category);
        int DeleteDuck(int id);

        int UpdateDuck(int id,
            string? name,
            int? producerID,
            double? price,
            string? photo,
            string? description,
            Category? category
            );

    }
}
