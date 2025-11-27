using CORE;
using INTERFACES;


namespace Strzelecki_Baranowski.DuckApp.DAO
{
    public class DAO : IDataAccessObject
    {

        private List<Duck> Ducks;
        private List<Producer> Producers;

        public DAO()
        {
            //To jest  mock -> tutaj ładowanie danych
            Producers = new List<Producer>() 
            { 
                new Producer() { Name = "Inne", ID= 0, Website= "https://en.wikipedia.org/wiki/Rubber_duck" },
                new Producer() { Name = "Tubbz", ID= 1, Website= "https://tubbz.com/en-eu" } 
            };

            Ducks = new List<Duck>() 
            { 
                new Duck() { Name= "Minecraf - Zombie (Mini)", ID=1, ProducerID=1, Price=9.99,
                    Description="He crawls straight out of the pixelated world of Minecraft and into your bath paradise: the Mini Bath Duck Minecraft Zombie! With his angular design, green complexion and typical blocky look, this undead creature is probably the most charming bath guest you've ever had. Instead of brains, he prefers to hunt for soap bubbles – but be careful, his duck friends might still get a fright!",
                    Photo="https://www.duckshop.de/media/image/ec/98/6b/Minecraft_-_Zombie_Mini_173430744_200x200.jpg",
                    Category=Category.VideoGames 
                }
            };

        }

        public int AddDuck(string name = "No name",
            int iD=-1, int producerID=0, 
            double price = 0,
            string photo = "No photo",
            string description = "No description",
            Category category = Category.None)
        {

            if (iD == -1) 
                iD = Ducks?.Max(x => x.ID) + 1 ?? 1;
           
            Duck newDuck = new() 
            { 
                Name = name,
                ID = iD,
                ProducerID = producerID,
                Price = price,
                Photo = photo,
                Description = description,
                Category = category
            };
            
            Ducks!.Add(newDuck);

            return 0;
        }

        public int AddProducer(string name= "No name", string website="No website", int id=-1)
        {
            if (id == -1)
                id =  Producers?.Max(x => x.ID) + 1 ?? 1;

            Producers?.Add(new Producer
            {
                Name = name,
                ID = id,
                Website = website,
            });

            return 0;
        }

        public int DeleteDuck(int id)
        {
            Duck? duck = Ducks.LastOrDefault(x => x.ID == id);
            if (duck == null) return 1;

            Ducks.Remove(duck);
            return 0;
        }

        public int DeleteProducer(int id)
        {
            Producer? duckP = Producers.LastOrDefault(x => x.ID == id);
            if (duckP == null) return 1;

            Producers.Remove(duckP);
            return 0;
        }

        public IEnumerable<IDuck> GetDucks()
        {
            return Ducks;
        }

        public IEnumerable<IProducer> GetProducers()
        {
            return Producers;
        }

        public int UpdateDuck(int id,
            string? name = null, 
            int? producerID = null,
            double? price = null,
            string? photo = null,
            string? description = null,
            Category? category = null
            )
        {
            Duck? duck = Ducks.FirstOrDefault(x => x.ID == id);
            if (duck == null) return 1;

            if (name != null) duck.Name = name;
            if (producerID.HasValue) duck.ProducerID = producerID.Value;
            if (price.HasValue) duck.Price = price.Value;
            if (photo != null) duck.Photo = photo;
            if (description != null) duck.Description = description;
            if (category != null) duck.Category = (Category)category;

            return 0;

        }

        public int UpdateProducer(int id, string? name = null, string? website = null)
        {
            Producer? duckP = Producers.FirstOrDefault(x => x.ID == id);
            if (duckP == null) return 1;

            if (name != null) duckP.Name = name;
            if (website != null) duckP.Website = website;

            return 0;
        }
    }
}