BEGIN TRANSACTION; 

DELETE FROM Ducks;
DELETE FROM Producers;

DELETE FROM sqlite_sequence WHERE name='Ducks';
DELETE FROM sqlite_sequence WHERE name='Producers';

--  Producers

INSERT INTO Producers (ID, Name, Website) VALUES (1, 'Tubbz', 'https://tubbz.com/en-eu');
INSERT INTO Producers (ID, Name, Website) VALUES (2, 'Rubbaducks', 'https://rubbaducks.net/');
INSERT INTO Producers (ID, Name, Website) VALUES (3, 'Locomocean', 'https://locomocean.com/');
INSERT INTO Producers (ID, Name, Website) VALUES (4, 'Schnabels', 'https://mbw.sh/en/marken/schnabels/');
INSERT INTO Producers (ID, Name, Website) VALUES (5, 'Yarto', 'https://www.yarto.com/');


-- Kaczki od Tubbz (ProducerID = 1)
INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (1, 'Minecraft - Zombie (Mini)', 1, 9.99, 'He crawls straight out of the pixelated world of Minecraft and into your bath paradise. With his angular design, green complexion and typical blocky look, this undead creature is probably the most charming bath guest you''ve ever had.', 'photos/1.jpg', 1);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (2, 'Power Rangers - Red Ranger', 1, 19.99, 'Just in case some aliens are thinking of taking over the duck pond, the Mighty Morphin Power Rangers are coming. Red Ranger is suited and booted in his iconic suit.', 'photos/2.jpg', 2);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (3, 'Jurassic Park - T-Rex (Mini)', 1, 9.99, 'The Jurassic Park mini T-Rex rubber ducky is an absolute must-have for all dino fans. With its fearsome yet somehow cute look, it brings an extra dose of Jurassic charm to your bathroom.', 'photos/3.jpg', 2);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (4, 'Lord of the Rings - Gimli (Mini)', 1, 9.99, 'Attention, the baths of Middle-earth have a new protector: Gimli the Dwarf! Our favourite dwarf from the Lord of the Rings saga has been transformed into a bath duck.', 'photos/4.jpg', 2);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (5, 'Sonic the Hedgehog (Boxed)', 1, 19.99, 'The Sonic the Hedgehog rubber duck is an absolute must-have for fans! With its classic look from 1991, this rubber duck brings nostalgia and fun to any collection.', 'photos/5.jpg', 1);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (6, 'Shrek', 1, 24.99, 'This rubber duck doesn''t come from the fairy tale forest - it comes straight from the swamp! Shrek, probably the most famous ogre in film history, has turned into a squeaky duck.', 'photos/6.jpg', 2);

-- Kaczki od Rubbaducks (ProducerID = 2)
INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (7, 'Duckphin - Delfin', 2, 13.49, 'Duckphin, the squeaking master of waves! The fusion of a rubber duck with the elegance of a dolphin.', 'photos/7.jpg', 3);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (8, 'Duckerball - Fussball', 2, 13.49, 'A duck made out of a football. What don''t you understand?!', 'photos/8.jpg', 4);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (9, 'Hero - Feuerwehrmann', 2, 13.49, 'Some ducks have jobs. This one is a firefighter.', 'photos/9.jpg', 5);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (10, 'Duckaroo - Kanguru', 2, 13.49, 'Proudly presenting the Duckaroo duck - a wonderful blend of rubber duck and kangaroo! Yes, you read it right, a kangaroo with a baby in its pouch.', 'photos/10.jpg', 3);

-- Kaczki od Locomocean (ProducerID = 3)
INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (11, 'Glow in the Dark - Pond Trooper', 3, 22.99, 'Experience galactic adventures right in your bathtub with the Pond Trooper! This light-up bath duck from the Pond Wars series brings light into the darkness.', 'photos/11.jpg', 2);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (12, 'Glow in the Dark - Dragon', 3, 22.99, 'Light the magical fire in your bathtub - with Drago the Dragon! Thanks to water-activated LED lights, Drago starts to glow in dazzling colors.', 'photos/12.jpg', 3);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (13, 'Glow in the Dark - Duck Bond', 3, 22.99, 'The name is Bond... Duck Bond! In her white tuxedo, Duck Bond not only conquers the hearts of spies, but also your bathroom.', 'photos/13.jpg', 2);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (14, 'Glow in the Dark - M. Quackson', 3, 22.99, 'Experience the magic of the King of Pop! Thanks to water-activated LEDs, it magically lights up as soon as it touches the water.', 'photos/14.jpg', 6);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (15, 'Glow in the Dark - Duck Fadar', 3, 22.99, 'Feel the power in the water with Duck Fadar! This mysterious space duck brings galactic fun to your bathtub.', 'photos/15.jpg', 2);

-- Kaczki od Schnabels (ProducerID = 4)
INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (16, 'Rubber Duck Basketball', 4, 7.79, 'With its blue jersey, white sweatband, and basketball in hand, it''s got the home court in your bathtub. Ready to rock the next match!', 'photos/16.jpg', 5);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (17, 'City Duck Bavaria', 4, 7.79, 'Totally Oktoberfest, totally Munich! Not only does it come with lederhosen, but it also carries a fancy beer in its hand.', 'photos/17.jpg', 7);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (18, 'City Duck Paris', 4, 7.79, 'Love - art - it''s Paris. Dressed in a beret, striped shirt, and scarf, our duck dives into the Parisian art scene.', 'photos/18.jpg', 7);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (19, 'Bartender Rubber Duck', 4, 7.79, 'With its elegant outfit, black bow tie, and suspenders, as well as a cocktail shaker in one hand, this squeaky duck is ready to liven up the mood.', 'photos/19.jpg', 5);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (20, 'Rubber Duckie Forwarding Agent', 4, 7.79, 'The Quacky Truckie! Dressed in a snazzy blue shirt, our Spediteur Duck proudly carries a miniature truck in one hand and a container in the other.', 'photos/20.jpg', 5);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (21, 'Rubber Duck Brewer', 4, 7.79, 'Dive into the foamy world with our Beer Brewer Rubber Duck. Complete with a beer keg and beer mug in hand, it brings a touch of Oktoberfest to your home.', 'photos/21.jpg', 5);

-- Kaczki od Yarto (ProducerID = 5)
INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (22, 'Big Ben', 5, 11.49, 'Half duck half Big Ben.', 'photos/22.jpg', 7);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (23, 'Robin Hood Rubber Duck', 5, 11.49, 'With its little green forest outfit, it''s ready to conquer the tub and steal your heart. This squeaky duck is not just a rubber duck, it''s a hero!', 'photos/23.jpg', 2);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (24, 'Eiffel-Tower Duck''oration', 5, 12.99, 'Get ready for the most unique Christmas tree ornament! A cheerful yellow rubber duck, elegantly balancing the iconic Eiffel Tower on its beak.', 'photos/24.jpg', 7);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (25, 'Donald Trump Rubber Duck', 5, 11.49, 'This rubber duck makes duck bathing great again! With its blonde quiff, serious expression, and distinctive red tie.', 'photos/25.jpg', 6);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (26, 'Astronaut Rubber Duck', 5, 11.49, 'The perfect little spaceman, dressed in a meticulously designed white spacesuit. It will make NASA and space fans rejoice.', 'photos/26.jpg', 5);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (27, 'Einstein Rubber Duck', 5, 11.49, 'A symbol of brilliance, a nod to all physics enthusiasts, and proof that even a little duckling has enough intellect to comprehend physics.', 'photos/27.jpg', 6);

INSERT INTO Ducks (ID, Name, ProducerID, Price, Description, Photo, Category) 
VALUES (28, 'Old Fashioned Pilot Duck', 5, 11.49, 'Pretty self-explanatory. It''s a duck with a dream.', 'photos/28.jpg', 5);

COMMIT;