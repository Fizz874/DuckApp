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
            _producers = new List<Producer>()
            {
                new Producer() { ID = 1, Name = "Tubbz", Website = "https://tubbz.com/en-eu" },
                new Producer() { ID = 2, Name = "Rubbaducks", Website = "https://rubbaducks.net/" },
                new Producer() { ID = 3, Name = "Locomocean", Website = "https://locomocean.com/" },
                new Producer() { ID = 4, Name = "Schnabels", Website = "https://mbw.sh/en/marken/schnabels/" },
                new Producer() { ID = 5, Name = "Yarto", Website = "https://www.yarto.com/" }
            };


            _ducks = new List<Duck>()
            {
                // -- Tubbz (ProducerID = 1) --
                new Duck() {
                    ID = 1, Name = "Minecraf - Zombie (Mini)", ProducerID = 1, Price = 9.99, Photo = "photos/1.jpg", Category = Category.VideoGames,
                    Description = "He crawls straight out of the pixelated world of Minecraft and into your bath paradise. With his angular design, green complexion and typical blocky look, this undead creature is probably the most charming bath guest you've ever had."
                },
                new Duck() {
                    ID = 2, Name = "Power Rangers - Red Ranger", ProducerID = 1, Price = 19.99, Photo = "photos/2.jpg", Category = Category.Movies,
                    Description = "Just in case some aliens are thinking of taking over the duck pond, the Mighty Morphin Power Rangers are coming. Red Ranger is suited and booted in his iconic suit."
                },
                new Duck() {
                    ID = 3, Name = "Jurassic Park - T-Rex (Mini)", ProducerID = 1, Price = 9.99, Photo = "photos/3.jpg", Category = Category.Movies,
                    Description = "The Jurassic Park mini T-Rex rubber ducky is an absolute must-have for all dino fans. With its fearsome yet somehow cute look, it brings an extra dose of Jurassic charm to your bathroom."
                },
                new Duck() {
                    ID = 4, Name = "Lord of the Rings - Gimli (Mini)", ProducerID = 1, Price = 9.99, Photo = "photos/4.jpg", Category = Category.Movies,
                    Description = "Attention, the baths of Middle-earth have a new protector: Gimli the Dwarf! Our favourite dwarf from the Lord of the Rings saga has been transformed into a bath duck."
                },
                new Duck() {
                    ID = 5, Name = "Sonic the Hedgehog (Boxed)", ProducerID = 1, Price = 19.99, Photo = "photos/5.jpg", Category = Category.VideoGames,
                    Description = "The Sonic the Hedgehog rubber duck is an absolute must-have for fans! With its classic look from 1991, this rubber duck brings nostalgia and fun to any collection."
                },
                new Duck() {
                    ID = 6, Name = "Shrek", ProducerID = 1, Price = 24.99, Photo = "photos/6.jpg", Category = Category.Movies,
                    Description = "This rubber duck doesn't come from the fairy tale forest - it comes straight from the swamp! Shrek, probably the most famous ogre in film history, has turned into a squeaky duck."
                },

                // -- Rubbaducks (ProducerID = 2) --
                new Duck() {
                    ID = 7, Name = "Duckphin - Delfin", ProducerID = 2, Price = 13.49, Photo = "photos/7.jpg", Category = Category.Animals,
                    Description = "Duckphin, the squeaking master of waves! The fusion of a rubber duck with the elegance of a dolphin."
                },
                new Duck() {
                    ID = 8, Name = "Duckerball - Fussball", ProducerID = 2, Price = 13.49, Photo = "photos/8.jpg", Category = Category.Sports,
                    Description = "A duck made out of a football. What don't you understand?!"
                },
                new Duck() {
                    ID = 9, Name = "Hero - Feuerwehrmann", ProducerID = 2, Price = 13.49, Photo = "photos/9.jpg", Category = Category.Jobs,
                    Description = "Some ducks have jobs. This one is a firefighter."
                },
                new Duck() {
                    ID = 10, Name = "Duckaroo - Kanguru", ProducerID = 2, Price = 13.49, Photo = "photos/10.jpg", Category = Category.Animals,
                    Description = "Proudly presenting the Duckaroo duck - a wonderful blend of rubber duck and kangaroo! Yes, you read it right, a kangaroo with a baby in its pouch."
                },

                // -- Locomocean (ProducerID = 3) --
                new Duck() {
                    ID = 11, Name = "Glow in the Dark - Pond Trooper", ProducerID = 3, Price = 22.99, Photo = "photos/11.jpg", Category = Category.Movies,
                    Description = "Experience galactic adventures right in your bathtub with the Pond Trooper! This light-up bath duck from the Pond Wars series brings light into the darkness."
                },
                new Duck() {
                    ID = 12, Name = "Glow in the Dark - Dragon", ProducerID = 3, Price = 22.99, Photo = "photos/12.jpg", Category = Category.Animals,
                    Description = "Light the magical fire in your bathtub - with Drago the Dragon! Thanks to water-activated LED lights, Drago starts to glow in dazzling colors."
                },
                new Duck() {
                    ID = 13, Name = "Glow in the Dark - Duck Bond", ProducerID = 3, Price = 22.99, Photo = "photos/13.jpg", Category = Category.Movies,
                    Description = "The name is Bond... Duck Bond! In her white tuxedo, Duck Bond not only conquers the hearts of spies, but also your bathroom."
                },
                new Duck() {
                    ID = 14, Name = "Glow in the Dark - M. Quackson", ProducerID = 3, Price = 22.99, Photo = "photos/14.jpg", Category = Category.Celebrities,
                    Description = "Experience the magic of the King of Pop! Thanks to water-activated LEDs, it magically lights up as soon as it touches the water."
                },
                new Duck() {
                    ID = 15, Name = "Glow in the Dark - Duck Fadar", ProducerID = 3, Price = 22.99, Photo = "photos/15.jpg", Category = Category.Movies,
                    Description = "Feel the power in the water with Duck Fadar! This mysterious space duck brings galactic fun to your bathtub."
                },

                // -- Schnabels (ProducerID = 4) --
                new Duck() {
                    ID = 16, Name = "Rubber Duck Basketball", ProducerID = 4, Price = 7.79, Photo = "photos/16.jpg", Category = Category.Sports,
                    Description = "With its blue jersey, white sweatband, and basketball in hand, it's got the home court in your bathtub. Ready to rock the next match!"
                },
                new Duck() {
                    ID = 17, Name = "City Duck Bavaria", ProducerID = 4, Price = 7.79, Photo = "photos/17.jpg", Category = Category.Cities,
                    Description = "Totally Oktoberfest, totally Munich! Not only does it come with lederhosen, but it also carries a fancy beer in its hand."
                },
                new Duck() {
                    ID = 18, Name = "City Duck Paris", ProducerID = 4, Price = 7.79, Photo = "photos/18.jpg", Category = Category.Cities,
                    Description = "Love - art - it's Paris. Dressed in a beret, striped shirt, and scarf, our duck dives into the Parisian art scene."
                },
                new Duck() {
                    ID = 19, Name = "Bartender Rubber Duck", ProducerID = 4, Price = 7.79, Photo = "photos/19.jpg", Category = Category.Jobs,
                    Description = "With its elegant outfit, black bow tie, and suspenders, as well as a cocktail shaker in one hand, this squeaky duck is ready to liven up the mood."
                },
                new Duck() {
                    ID = 20, Name = "Rubber Duckie Forwarding Agent", ProducerID = 4, Price = 7.79, Photo = "photos/20.jpg", Category = Category.Jobs,
                    Description = "The Quacky Truckie! Dressed in a snazzy blue shirt, our Spediteur Duck proudly carries a miniature truck in one hand and a container in the other."
                },
                new Duck() {
                    ID = 21, Name = "Rubber Duck Brewer", ProducerID = 4, Price = 7.79, Photo = "photos/21.jpg", Category = Category.Jobs,
                    Description = "Dive into the foamy world with our Beer Brewer Rubber Duck. Complete with a beer keg and beer mug in hand, it brings a touch of Oktoberfest to your home."
                },

                // -- Yarto (ProducerID = 5) --
                new Duck() {
                    ID = 22, Name = "Big Ben", ProducerID = 5, Price = 11.49, Photo = "photos/22.jpg", Category = Category.Cities,
                    Description = "Half duck half Big Ben."
                },
                new Duck() {
                    ID = 23, Name = "Robin Hood Rubber Duck", ProducerID = 5, Price = 11.49, Photo = "photos/23.jpg", Category = Category.Movies,
                    Description = "With its little green forest outfit, it's ready to conquer the tub and steal your heart. This squeaky duck is not just a rubber duck, it's a hero!"
                },
                new Duck() {
                    ID = 24, Name = "Eiffel-Tower Duck'oration", ProducerID = 5, Price = 12.99, Photo = "photos/24.jpg", Category = Category.Cities,
                    Description = "Get ready for the most unique Christmas tree ornament! A cheerful yellow rubber duck, elegantly balancing the iconic Eiffel Tower on its beak."
                },
                new Duck() {
                    ID = 25, Name = "Donald Trump Rubber Duck", ProducerID = 5, Price = 11.49, Photo = "photos/25.jpg", Category = Category.Celebrities,
                    Description = "This rubber duck makes duck bathing great again! With its blonde quiff, serious expression, and distinctive red tie."
                },
                new Duck() {
                    ID = 26, Name = "Astronaut Rubber Duck", ProducerID = 5, Price = 11.49, Photo = "photos/26.jpg", Category = Category.Jobs,
                    Description = "The perfect little spaceman, dressed in a meticulously designed white spacesuit. It will make NASA and space fans rejoice."
                },
                new Duck() {
                    ID = 27, Name = "Einstein Rubber Duck", ProducerID = 5, Price = 11.49, Photo = "photos/27.jpg", Category = Category.Celebrities,
                    Description = "A symbol of brilliance, a nod to all physics enthusiasts, and proof that even a little duckling has enough intellect to comprehend physics."
                },
                new Duck() {
                    ID = 28, Name = "Old Fashioned Pilot Duck", ProducerID = 5, Price = 11.49, Photo = "photos/28.jpg", Category = Category.Jobs,
                    Description = "Pretty self-explanatory. It's a duck with a dream."
                }
            };
        }

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

        public int AddProducer(IProducer producer)
        {
            int newId = _producers.Any() ? _producers.Max(x => x.ID) + 1 : 1;

            Producer newProducerEntity = new Producer()
            {
                ID = newId,
                Name = producer.Name,
                Website = producer.Website,
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
            Producer? producerToRemove = _producers.LastOrDefault(x => x.ID == id);

            if (producerToRemove == null) return 1;

            _ducks.RemoveAll(d => d.ProducerID == id);

            _producers.Remove(producerToRemove);
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