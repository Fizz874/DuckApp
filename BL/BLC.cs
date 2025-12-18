
using System.Reflection;
using Strzelecki_Baranowski.DuckApp.INTERFACES;

namespace Strzelecki_Baranowski.DuckApp.BL
{
    public class BLC
    {
        private readonly IDataAccessObject _dao;

        public BLC(string? configPath, string? type) {
            if (string.IsNullOrWhiteSpace(configPath))
                throw new ArgumentNullException(nameof(configPath), "Assembly path cannot be empty.");

            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentNullException(nameof(type), "Type name cannot be empty.");

            if (!System.IO.File.Exists(configPath))
                throw new FileNotFoundException($"The file from the assembly path doesn't exist. {configPath}");

            Assembly asm;
            try
            {
                asm = Assembly.UnsafeLoadFrom(configPath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load assembly from {configPath}", ex);
            }

            var ti = asm.GetType(type);
            if (ti == null)
                throw new TypeLoadException($"Type '{type}' not found in assembly '{configPath}'.");

            var instance = Activator.CreateInstance(ti);

            if (instance is not IDataAccessObject dao)
                throw new InvalidCastException($"Type {type} does not implement IDataAccessObject");

            _dao = dao;
        }

        public IEnumerable<IDuck> GetAllDucks()
        {
            return _dao.GetDucks();
        }

        public IEnumerable<IProducer> GetAllProducers()
        {
            return _dao.GetProducers();
        }

        public void UpdateDuck(IDuck duck) 
        {
            _dao.UpdateDuck(duck);
        }

        public void UpdateProducer(IProducer producer)
        {
            _dao.UpdateProducer(producer);
        }

        public int AddNewDuck(IDuck duck)
        {
            return _dao.AddDuck(duck);
        }

        public int AddNewProducer(IProducer producer)
        {
            return _dao.AddProducer(producer);
        }

        public void DeleteDuck(int id)
        {
            _dao.DeleteDuck(id);
        }

        public void DeleteProducer(int id)
        {
            _dao.DeleteProducer(id);
        }

        public IDuck GetNewDuck() 
        {  
            return _dao.GetNewDuck(); 
        }

        public IProducer GetNewProducer()
        {
            return _dao.GetNewProducer();
        }
    }
}