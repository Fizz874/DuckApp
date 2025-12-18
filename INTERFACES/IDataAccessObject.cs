namespace Strzelecki_Baranowski.DuckApp.INTERFACES
{
    public interface IDataAccessObject
    {
        IEnumerable<IProducer> GetProducers();
        IEnumerable<IDuck> GetDucks();
        int AddProducer(IProducer producer);
        int DeleteProducer(int id);
        int UpdateProducer(IProducer producer);
        int AddDuck(IDuck duck);
        int DeleteDuck(int id);
        int UpdateDuck(IDuck duck);
        IDuck GetNewDuck();
        IProducer GetNewProducer();
    }
}
