using Microsoft.EntityFrameworkCore;
using Strzelecki_Baranowski.DuckApp.INTERFACES;

namespace Strzelecki_Baranowski.DuckApp.DAO
{
    public class DAO : IDataAccessObject
    {
        public DAO()
        {
            using (var context = new DataContext())
            {
                context.Database.EnsureCreated();
                context.SeedData();
            }
        }

        public int AddDuck(IDuck duck)
        {
            using (var context = new DataContext())
            {
                var newEntity = new Duck
                {
                    Name = duck.Name,
                    Price = duck.Price,
                    Photo = duck.Photo,
                    Description = duck.Description,
                    Category = duck.Category,

                    ProducerID = duck.ProducerID
                };

                context.Ducks.Add(newEntity);
                context.SaveChanges();

                return newEntity.ID;
            }
        }

        public int AddProducer(IProducer producer)
        {
            using (var context = new DataContext())
            {
                var newEntity = new Producer
                {
                    Name = producer.Name,
                    Website = producer.Website
                };

                context.Producers.Add(newEntity);
                context.SaveChanges();

                return newEntity.ID;
            }
        }

        public int DeleteDuck(int id)
        {
            using (var context = new DataContext())
            {
                var duckToDelete = context.Ducks.Find(id);
                if (duckToDelete != null)
                {
                    context.Ducks.Remove(duckToDelete);
                    context.SaveChanges();
                    return 0;
                }
            }
            return 1;
        }

        public int DeleteProducer(int id)
        {
            using (var context = new DataContext())
            {
                var producerToDelete = context.Producers.Find(id);
                if (producerToDelete != null)
                {
                    context.Producers.Remove(producerToDelete);
                    context.SaveChanges();
                    return 0;
                }
            }
            return 1;
        }

        public IEnumerable<IDuck> GetDucks()
        {
            using (var context = new DataContext())
            {
                return context.Ducks.AsNoTracking().ToList();
            }
        }

        public IDuck GetNewDuck()
        {
            return new Duck();
        }

        public IProducer GetNewProducer()
        {
            return new Producer();
        }

        public IEnumerable<IProducer> GetProducers()
        {
            using (var context = new DataContext())
            {
                return context.Producers.AsNoTracking().ToList();
            }
        }

        public int UpdateDuck(IDuck duck)
        {
            using (var context = new DataContext())
            {
                var existingDuck = context.Ducks.Find(duck.ID);

                if (existingDuck != null)
                {
                    existingDuck.Name = duck.Name;
                    existingDuck.Price = duck.Price;
                    existingDuck.Description = duck.Description;
                    existingDuck.Category = duck.Category;
                    existingDuck.Photo = duck.Photo;
                    existingDuck.ProducerID = duck.ProducerID;

                    context.SaveChanges();
                    return 0;
                }
            }
            return 1;
        }

        public int UpdateProducer(IProducer producer)
        {
            using (var context = new DataContext())
            {
                var existingProducer = context.Producers.Find(producer.ID);

                if (existingProducer != null)
                {
                    existingProducer.Name = producer.Name;
                    existingProducer.Website = producer.Website;

                    context.SaveChanges();
                    return 0;
                }
            }
            return 1;
        }
    }
}
