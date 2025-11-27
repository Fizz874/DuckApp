using CORE;
using INTERFACES;

namespace Strzelecki_Baranowski.DuckApp.DAO
{
    public class DAO : IDataAccessObject
    {
        DataContext _context;
        public DAO() {
        
            _context = new DataContext();
            //TODO zaimplementować metody
            //TODO zaimplementować relację 1:N między kaczkami a producentami <?>
        }


        public int AddDuck(string name, int iD, int producerID, double price, string photo, string description, Category category)
        {
            throw new NotImplementedException();
        }

        public int AddProducer(string name, string website, int id)
        {
            throw new NotImplementedException();
        }

        public int DeleteDuck(int id)
        {
            throw new NotImplementedException();
        }

        public int DeleteProducer(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDuck> GetDucks()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IProducer> GetProducers()
        {
            throw new NotImplementedException();
        }

        public int UpdateDuck(int id, string? name, int? producerID, double? price, string? photo, string? description, Category? category)
        {
            throw new NotImplementedException();
        }

        public int UpdateProducer(int id, string? name, string? website)
        {
            throw new NotImplementedException();
        }
    }
}
