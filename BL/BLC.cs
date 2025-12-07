
using System.Reflection;
using CORE;
using Strzelecki_Baranowski.DuckApp.INTERFACES;

namespace Strzelecki_Baranowski.DuckApp.BL
{
    public class BLC
    {

        private readonly IDataAccessObject _dao;


        public BLC(string? config_path, string? type) {
            //Assembly DAO = Assembly.UnsafeLoadFrom(config_path);
            //Type ti = DAO.GetType(type);
            //var o = Activator.CreateInstance(ti, new object[] { "" });

            if (string.IsNullOrWhiteSpace(config_path))
                throw new ArgumentNullException(nameof(config_path), "Ścieżka do assembly nie może być pusta.");

            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentNullException(nameof(type), "Nazwa typu nie może być pusta.");

            if (!System.IO.File.Exists(config_path))
                throw new FileNotFoundException($"Nie znaleziono pliku assembly: {config_path}");

            // Ładowanie assembly
            Assembly asm;
            try
            {
                asm = Assembly.UnsafeLoadFrom(config_path);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Nie udało się załadować assembly z {config_path}", ex);
            }

            // Pobranie typu
            var ti = asm.GetType(type);
            if (ti == null)
                throw new TypeLoadException($"Nie znaleziono typu '{type}' w assembly '{config_path}'.");

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
            _dao.UpdateDuck(duck.ID, duck.Name, duck.ProducerID, duck.Price, duck.Photo, duck.Description, duck.Category);

        }

        public void UpdateProducer(IProducer producer)
        {
            _dao.UpdateProducer(producer.ID, producer.Name, producer.Website);

        }

        public int AddNewDuck(string name,
            int producerID,
            double price,
            string photo,
            string description,
            Category category)
        {

            return _dao.AddDuck(name, producerID, price, photo, description, category);

        }

        public int AddNewProducer(string name, string website)
        {
            return _dao.AddProducer(name, website);

        }


        public void DeleteDuck(int id)
        {
            _dao.DeleteDuck(id);
        }


        public void DeleteProducer(int id)
        {
            _dao.DeleteProducer(id);
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