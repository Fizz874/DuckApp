
using System.Reflection;
using Strzelecki_Baranowski.DuckApp.CORE;
using Strzelecki_Baranowski.DuckApp.INTERFACES;

namespace Strzelecki_Baranowski.DuckApp.BL
{
    public class BLC
    {

        private readonly IDataAccessObject _dao;


        public BLC(string? configPath, string? type) {
            //Assembly DAO = Assembly.UnsafeLoadFrom(config_path);
            //Type ti = DAO.GetType(type);
            //var o = Activator.CreateInstance(ti, new object[] { "" });

            if (string.IsNullOrWhiteSpace(configPath))
                throw new ArgumentNullException(nameof(configPath), "Assembly path cannot be empty.");

            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentNullException(nameof(type), "Type name cannot be empty.");

            if (!System.IO.File.Exists(configPath))
                throw new FileNotFoundException($"Type name cannot be empty. {configPath}");

            // Ładowanie assembly
            Assembly asm;
            try
            {
                asm = Assembly.UnsafeLoadFrom(configPath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load assembly from {configPath}", ex);
            }


            // Pobranie typu
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


        public void UpdateDuck(IDuck duck) //TODO upewnić się że foto wskazuje w dobre miejsce
        {
            //_dao.UpdateDuck(duck.ID, duck.Name, duck.ProducerID, duck.Price, duck.Photo, duck.Description, duck.Category);
            _dao.UpdateDuck(duck);

        }

        public void UpdateProducer(IProducer producer)
        {
            //_dao.UpdateProducer(producer.ID, producer.Name, producer.Website);
            _dao.UpdateProducer(producer);
        }

        public int AddNewDuck(/*string name,
            int producerID,
            double price,
            string photo,
            string description,
            Category category*/IDuck duck)
        {

            //return _dao.AddDuck(duck.Name, duck.ProducerID, duck.Price, duck.Photo, duck.Description, duck.Category);
            return _dao.AddDuck(duck);
        }

        public int AddNewProducer(/*string name, string website*/IProducer producer)
        {
            return _dao.AddProducer(producer/*name, website*/);

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


        //public IProducer GetProducer(int id)
        //{
        //    var producers = _dao.GetProducers();
        //    var found = producers.FirstOrDefault(x => x.ID == id);

        //    if (found == null)
        //        throw new KeyNotFoundException($"Producer with ID {id} not found."); //TODO obsługa takich błędów w warstwie UI - co wyświetlić

        //    return found;
        //}

        //public IEnumerable<IDuck> getAllDucksFromProducer(int id)
        //{
        //    return _dao.GetDucks().Where(x => x.ID == id);
        //}



        //Dodać metody wstylu getAllDucksFromProducer(id)



    }
}