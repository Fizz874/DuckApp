using Strzelecki_Baranowski.DuckApp.CORE;
using Strzelecki_Baranowski.DuckApp.INTERFACES;


namespace Strzelecki_Baranowski.DuckApp.DAO
{
    public class DAO : IDataAccessObject
    {

        private List<Duck> _ducks;
        private List<Producer> _producers;

        public DAO()
        {
            //To jest  mock -> tutaj ładowanie danych
            _producers = new List<Producer>() 
            { 
                new Producer() { Name = "Inne", ID= 0, Website= "https://en.wikipedia.org/wiki/Rubber_duck" },
                new Producer() { Name = "Tubbz", ID= 1, Website= "https://tubbz.com/en-eu" } 
            };

            _ducks = new List<Duck>() 
            { 
                new Duck() { Name= "Minecraf - Zombie (Mini)", ID=1, ProducerID=1, Price=9.99,
                    Description="He crawls straight out of the pixelated world of Minecraft and into your bath paradise: the Mini Bath Duck Minecraft Zombie! With his angular design, green complexion and typical blocky look, this undead creature is probably the most charming bath guest you've ever had. Instead of brains, he prefers to hunt for soap bubbles – but be careful, his duck friends might still get a fright!",
                    Photo= /*"https://www.duckshop.de/media/image/ec/98/6b/Minecraft_-_Zombie_Mini_173430744_200x200.jpg"*/ "photos/1.jpg" /*"photos/8847e930-c875-4b9d-9142-9ef317796d0e.jpg"*/,
                    Category=Category.VideoGames 
                }
            };

        }

        //public int AddDuck(string name = "No name",
        //    int producerID=0, 
        //    double price = 0,
        //    string photo = "No photo",
        //    string description = "No description",
        //    Category category = Category.None)
        //{


        //    int iD = Ducks?.Max(x => x.ID) + 1 ?? 1;

        //    Duck newDuck = new() 
        //    { 
        //        Name = name,
        //        ID = iD,
        //        ProducerID = producerID,
        //        Price = price,
        //        Photo = photo,
        //        Description = description,
        //        Category = category
        //    };

        //    Ducks!.Add(newDuck);

        //    return iD;
        //}

        public int AddDuck(IDuck duck)
        {
            int newId = _ducks.Any() ? _ducks.Max(x => x.ID) + 1 : 1;

            Duck newDuckEntity = new Duck()
            {
                ID = newId,
                Name = duck.Name,
                ProducerID = duck.ProducerID,
                Price = duck.Price,
                Photo = duck.Photo,
                Description = duck.Description,
                Category = duck.Category
            };

            _ducks.Add(newDuckEntity);
            return newId;
        }


        //public int AddProducer(string name= "No name", string website="No website")
        //{

        //    int id =  Producers?.Max(x => x.ID) + 1 ?? 1;

        //    Producers?.Add(new Producer
        //    {
        //        Name = name,
        //        ID = id,
        //        Website = website,
        //    });

        //    return id;
        //}

        public int AddProducer(IProducer producer)
        {
            int newId = _producers.Any() ? _producers.Max(x => x.ID) + 1 : 1;

            Producer newProducerEntity = new Producer()
            {
                ID = newId,
                Name = producer.Name,
                Website = producer.Website,
                //Ducks = new List<Duck>() // Pusta lista na start
            };

            _producers.Add(newProducerEntity);
            return newId;
        }


        public int DeleteDuck(int id)
        {
            Duck? duck = _ducks.LastOrDefault(x => x.ID == id);
            if (duck == null) return 1;

            _ducks.Remove(duck);
            return 0;
        }

        public int DeleteProducer(int id)
        {
            Producer? duckP = _producers.LastOrDefault(x => x.ID == id);
            if (duckP == null) return 1;
            _ducks.RemoveAll(d => d.ProducerID == id);
            _producers.Remove(duckP);
            return 0;
        }

        public IEnumerable<IDuck> GetDucks()
        {
            return _ducks;
        }

        public IEnumerable<IProducer> GetProducers()
        {
            return _producers;
        }

        //public int UpdateDuck(int id,
        //    string? name = null, 
        //    int? producerID = null,
        //    double? price = null,
        //    string? photo = null,
        //    string? description = null,
        //    Category? category = null
        //    )
        //{
        //    Duck? duck = Ducks.FirstOrDefault(x => x.ID == id);
        //    if (duck == null) return 1;

        //    if (name != null) duck.Name = name;
        //    if (producerID.HasValue) duck.ProducerID = producerID.Value;
        //    if (price.HasValue) duck.Price = price.Value;
        //    if (photo != null) duck.Photo = photo;
        //    if (description != null) duck.Description = description;
        //    if (category != null) duck.Category = (Category)category;

        //    return 0;

        //}

        public int UpdateDuck(IDuck duck)
        {
            var existingDuck = _ducks.FirstOrDefault(x => x.ID == duck.ID);
            if (existingDuck == null) return 1;

            existingDuck.Name = duck.Name;
            existingDuck.ProducerID = duck.ProducerID;
            existingDuck.Price = duck.Price;
            existingDuck.Photo = duck.Photo;
            existingDuck.Description = duck.Description;
            existingDuck.Category = duck.Category;

            return 0; 
        }


        //public int UpdateProducer(int id, string? name = null, string? website = null)
        //{
        //    Producer? duckP = Producers.FirstOrDefault(x => x.ID == id);
        //    if (duckP == null) return 1;

        //    if (name != null) duckP.Name = name;
        //    if (website != null) duckP.Website = website;

        //    return 0;
        //}

        public int UpdateProducer(IProducer producer)
        {
            var existingProducer = _producers.FirstOrDefault(x => x.ID == producer.ID);
            if (existingProducer == null) return 1;

            existingProducer.Name = producer.Name;
            existingProducer.Website = producer.Website;

            return 0;
        }


        public IDuck GetNewDuck()
        {
            return new Duck();
        }

        public IProducer GetNewProducer()
        { 
            return new Producer(); 
        }


    }
}