using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Strzelecki_Baranowski.DuckApp.BL;
using Strzelecki_Baranowski.DuckApp.CORE;
using Strzelecki_Baranowski.DuckApp.INTERFACES;
using Strzelecki_Baranowski.DuckApp.WebUI.Models;

namespace Strzelecki_Baranowski.DuckApp.WebUI.Controllers
{
    public class DucksController : Controller
    {
        private readonly BLC _blc;
        private readonly IWebHostEnvironment _webHostEnvironment; // <--- Serwis od ścieżek
        public DucksController(BLC blc, IWebHostEnvironment webHostEnvironment)
        {
            _blc = blc;
            _webHostEnvironment = webHostEnvironment;
        }

        //public IActionResult Index()
        //{
        //    IEnumerable<IDuck> ducksFromCore = _blc.GetAllDucks();
        //    var model = ducksFromCore.Select(d => new Duck
        //    {
        //        ID = d.ID,
        //        Name = d.Name,
        //        Price = (decimal)d.Price, // Rzutowanie jeśli w CORE jest double
        //        Category = (Category)d.Category, // Rzutowanie enuma
        //        Description = d.Description,
        //        Photo = d.Photo, // Uwaga na nazwy właściwości
        //        ProducerID = d.ProducerID,
        //        // Jeśli IDuck ma pole ProducerName, to super, jak nie to trzeba pobrać
        //    }).ToList();

        //    return View(model);
        //}
        public IActionResult Index(DuckFilterViewModel filter, bool reset = false)
        {
            // KROK A: Obsługa pamięci filtrów
            if (reset)
            {
                // 1. Użytkownik kliknął RESET - czyścimy sesję i filtry
                HttpContext.Session.Remove("DuckFilterState");
                filter = new DuckFilterViewModel();
            }
            else if (Request.Query.Count == 0)
            {
                // 2. Brak parametrów w URL (powrót z innej strony) - próbujemy odtworzyć z sesji
                var storedFilter = HttpContext.Session.Get<DuckFilterViewModel>("DuckFilterState");
                if (storedFilter != null)
                {
                    filter = storedFilter;
                }
            }
            else
            {
                // 3. Są parametry (użytkownik kliknął Apply) - zapisujemy nowy stan do sesji
                HttpContext.Session.Set("DuckFilterState", filter);
            }

            // KROK B: Standardowa logika filtrowania (Twoja z poprzedniego kroku)
            IEnumerable<IDuck> query = _blc.GetAllDucks();

            if (filter.ID.HasValue)
            {
                int val = filter.ID.Value;
                query = filter.IDMode switch
                {
                    FilterMode.Equal => query.Where(d => d.ID == val),
                    FilterMode.NotEqual => query.Where(d => d.ID != val),
                    FilterMode.Greater => query.Where(d => d.ID > val),
                    FilterMode.Less => query.Where(d => d.ID < val),
                    FilterMode.GreaterOrEqual => query.Where(d => d.ID >= val),
                    FilterMode.LessOrEqual => query.Where(d => d.ID <= val),
                    _ => query
                };
            }

            // 2. Filtr Price (Number)
            if (filter.Price.HasValue)
            {
                double val = (double)filter.Price.Value;
                query = filter.PriceMode switch
                {
                    FilterMode.Equal => query.Where(d => d.Price == val),
                    FilterMode.NotEqual => query.Where(d => d.Price != val),
                    FilterMode.Greater => query.Where(d => d.Price > val),
                    FilterMode.Less => query.Where(d => d.Price < val),
                    FilterMode.GreaterOrEqual => query.Where(d => d.Price >= val),
                    FilterMode.LessOrEqual => query.Where(d => d.Price <= val),
                    _ => query
                };
            }

            // 3. Filtr Name (Text)
            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = filter.NameMode switch
                {
                    FilterMode.Contains => query.Where(d => d.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase)),
                    FilterMode.NotContains => query.Where(d => !d.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase)),
                    FilterMode.Equal => query.Where(d => d.Name.Equals(filter.Name, StringComparison.OrdinalIgnoreCase)),
                    FilterMode.NotEqual => query.Where(d => !d.Name.Equals(filter.Name, StringComparison.OrdinalIgnoreCase)),
                    _ => query
                };
            }

            // 4. Filtr Description (Text)
            if (!string.IsNullOrEmpty(filter.Description))
            {
                // Uwaga: Description może być null w bazie, więc używamy ?.
                query = filter.DescriptionMode switch
                {
                    FilterMode.Contains => query.Where(d => d.Description?.Contains(filter.Description, StringComparison.OrdinalIgnoreCase) ?? false),
                    FilterMode.NotContains => query.Where(d => !(d.Description?.Contains(filter.Description, StringComparison.OrdinalIgnoreCase) ?? false)),
                    FilterMode.Equal => query.Where(d => (d.Description ?? "").Equals(filter.Description, StringComparison.OrdinalIgnoreCase)),
                    FilterMode.NotEqual => query.Where(d => !(d.Description ?? "").Equals(filter.Description, StringComparison.OrdinalIgnoreCase)),
                    _ => query
                };
            }

            // 5. Category & Producer
            if (filter.Category.HasValue)
                query = query.Where(d => (Category)d.Category == filter.Category.Value);

            if (filter.ProducerID.HasValue)
                query = query.Where(d => d.ProducerID == filter.ProducerID.Value);


            // KROK C: Budowanie modelu
            var duckModels = query.Select(d => new Duck
            {
                ID = d.ID,
                Name = d.Name,
                Price = (decimal)d.Price,
                Category = (Category)d.Category,
                Description = d.Description,
                Photo = d.Photo,
                ProducerID = d.ProducerID
            }).ToList();

            var viewModel = new DuckIndexViewModel
            {
                Ducks = duckModels,
                Filter = filter, // Przekazujemy (potencjalnie odtworzony) filtr do widoku
                ProducersList = new SelectList(_blc.GetAllProducers(), "ID", "Name", filter.ProducerID)
            };

            return View(viewModel);
        }
        //public IActionResult Index(DuckFilterViewModel filter)
        //{
        //    IEnumerable<IDuck> query = _blc.GetAllDucks();

        //    // 1. Filtr ID (Number)
        //    if (filter.ID.HasValue)
        //    {
        //        int val = filter.ID.Value;
        //        query = filter.IDMode switch
        //        {
        //            FilterMode.Equal => query.Where(d => d.ID == val),
        //            FilterMode.NotEqual => query.Where(d => d.ID != val),
        //            FilterMode.Greater => query.Where(d => d.ID > val),
        //            FilterMode.Less => query.Where(d => d.ID < val),
        //            FilterMode.GreaterOrEqual => query.Where(d => d.ID >= val),
        //            FilterMode.LessOrEqual => query.Where(d => d.ID <= val),
        //            _ => query
        //        };
        //    }

        //    // 2. Filtr Price (Number)
        //    if (filter.Price.HasValue)
        //    {
        //        double val = (double)filter.Price.Value;
        //        query = filter.PriceMode switch
        //        {
        //            FilterMode.Equal => query.Where(d => d.Price == val),
        //            FilterMode.NotEqual => query.Where(d => d.Price != val),
        //            FilterMode.Greater => query.Where(d => d.Price > val),
        //            FilterMode.Less => query.Where(d => d.Price < val),
        //            FilterMode.GreaterOrEqual => query.Where(d => d.Price >= val),
        //            FilterMode.LessOrEqual => query.Where(d => d.Price <= val),
        //            _ => query
        //        };
        //    }

        //    // 3. Filtr Name (Text)
        //    if (!string.IsNullOrEmpty(filter.Name))
        //    {
        //        query = filter.NameMode switch
        //        {
        //            FilterMode.Contains => query.Where(d => d.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase)),
        //            FilterMode.NotContains => query.Where(d => !d.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase)),
        //            FilterMode.Equal => query.Where(d => d.Name.Equals(filter.Name, StringComparison.OrdinalIgnoreCase)),
        //            FilterMode.NotEqual => query.Where(d => !d.Name.Equals(filter.Name, StringComparison.OrdinalIgnoreCase)),
        //            _ => query
        //        };
        //    }

        //    // 4. Filtr Description (Text)
        //    if (!string.IsNullOrEmpty(filter.Description))
        //    {
        //        // Uwaga: Description może być null w bazie, więc używamy ?.
        //        query = filter.DescriptionMode switch
        //        {
        //            FilterMode.Contains => query.Where(d => d.Description?.Contains(filter.Description, StringComparison.OrdinalIgnoreCase) ?? false),
        //            FilterMode.NotContains => query.Where(d => !(d.Description?.Contains(filter.Description, StringComparison.OrdinalIgnoreCase) ?? false)),
        //            FilterMode.Equal => query.Where(d => (d.Description ?? "").Equals(filter.Description, StringComparison.OrdinalIgnoreCase)),
        //            FilterMode.NotEqual => query.Where(d => !(d.Description ?? "").Equals(filter.Description, StringComparison.OrdinalIgnoreCase)),
        //            _ => query
        //        };
        //    }

        //    // 5. Category & Producer
        //    if (filter.Category.HasValue)
        //        query = query.Where(d => (Category)d.Category == filter.Category.Value);

        //    if (filter.ProducerID.HasValue)
        //        query = query.Where(d => d.ProducerID == filter.ProducerID.Value);

        //    // Mapowanie na Model MVC (zachowując logikę zdjęć)
        //    var duckModels = query.Select(d => new Duck
        //    {
        //        ID = d.ID,
        //        Name = d.Name,
        //        Price = (decimal)d.Price,
        //        Category = (Category)d.Category,
        //        Description = d.Description,
        //        Photo = d.Photo,
        //        ProducerID = d.ProducerID
        //    }).ToList();

        //    var viewModel = new DuckIndexViewModel
        //    {
        //        Ducks = duckModels,
        //        Filter = filter,
        //        ProducersList = new SelectList(_blc.GetAllProducers(), "ID", "Name", filter.ProducerID)
        //    };

        //    return View(viewModel);
        //}

        // GET: Ducks/Details/5
        public IActionResult Details(int id)
        {
            // 1. Pobierz wszystkie kaczki z BLC i znajdź tę jedną
            // (W idealnym świecie BLC powinno mieć metodę GetDuckById(id), ale użyjmy tego co masz)
            var coreDuck = _blc.GetAllDucks().FirstOrDefault(d => d.ID == id);

            if (coreDuck == null)
            {
                return NotFound(); // Zwraca błąd 404, jeśli nie ma takiego ID
            }

            // 2. Pobierz producenta, żeby wyświetlić jego nazwę
            var producers = _blc.GetAllProducers();
            var producer = producers.FirstOrDefault(p => p.ID == coreDuck.ProducerID);

            // 3. Mapowanie (IDuck -> Duck Model MVC)
            var model = new Duck
            {
                ID = coreDuck.ID,
                Name = coreDuck.Name,
                Price = (decimal)coreDuck.Price,
                Description = coreDuck.Description,
                Category = (Category)coreDuck.Category, // Rzutowanie enuma
                ProducerID = coreDuck.ProducerID,

                // Ważne: Przekazujemy też obiekt producenta, żeby widok mógł wyświetlić jego nazwę
                Producer = producer != null ? new Producer { Name = producer.Name, ID = producer.ID } : null,

                // 4. Obróbka zdjęcia (Wycinamy samą nazwę pliku, żeby pasowała do wwwroot)
                // Jeśli w bazie jest "C:\Users\...\foto.jpg", to bierzemy tylko "foto.jpg"
                Photo = coreDuck.Photo//System.IO.Path.GetFileName(coreDuck.Photo)
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // A. Pobierz kaczkę z BLC (tak jak w Details)
            var coreDuck = _blc.GetAllDucks().FirstOrDefault(d => d.ID == id);
            if (coreDuck == null) return NotFound();

            // B. Zmapuj na Model MVC
            var model = new Duck
            {
                ID = coreDuck.ID,
                Name = coreDuck.Name,
                Price = (decimal)coreDuck.Price,
                Description = coreDuck.Description,
                Category = (Category)coreDuck.Category,
                ProducerID = coreDuck.ProducerID,
                Photo = coreDuck.Photo // Zachowujemy ścieżkę, żeby nie zginęła!
            };

            // C. Przygotuj listy rozwijane (ComboBoxy)
            // SelectList(ŹródłoDanych, CoJestWartością(ID), CoJestTekstem(Name), CoJestWybrane)
            var producers = _blc.GetAllProducers();
            ViewBag.ProducersList = new SelectList(producers, "ID", "Name", model.ProducerID);

            return View(model);
        }

        // 2. KROK DRUGI: Zapisanie zmian (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Duck model) // Dodaj 'async Task' bo zapis pliku jest asynchroniczny
        {
            if (id != model.ID) return NotFound();

            if (ModelState.IsValid)
            {
                var domainDuck = _blc.GetNewDuck();

                // Przepisujemy standardowe dane
                domainDuck.ID = model.ID;
                domainDuck.Name = model.Name;
                domainDuck.Price = (double)model.Price;
                domainDuck.Description = model.Description ?? String.Empty;
                domainDuck.Category = (Strzelecki_Baranowski.DuckApp.CORE.Category)model.Category;
                domainDuck.ProducerID = model.ProducerID;

                // --- LOGIKA ZDJĘCIA ---

                if (model.PhotoUpload != null)
                {
                    // 1. Użytkownik wgrał nowe zdjęcie!

                    // Tworzymy folder docelowy: ".../wwwroot/photos"
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "photos");

                    // Tworzymy unikalną nazwę pliku (żeby kaczka1.jpg nie nadpisała innej kaczka1.jpg)
                    // Używamy GUID, np. "e32b4-5123-kaczka.jpg"
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.PhotoUpload.FileName;

                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Zapisujemy plik na dysku
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.PhotoUpload.CopyToAsync(fileStream);
                    }

                    // Zapisujemy PEŁNĄ ścieżkę do bazy (dla kompatybilności z Twoim BLC)
                    domainDuck.Photo = filePath;
                }
                else
                {
                    // 2. Użytkownik nie zmienił zdjęcia -> zachowujemy stare
                    domainDuck.Photo = model.Photo ?? "";
                }
                try
                {

                    _blc.UpdateDuck(domainDuck);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Błąd z logiki biznesowej: {ex.Message}");
                }
            }

            // Jeśli walidacja nie przeszła, ponownie ładujemy listę producentów
            var producers = _blc.GetAllProducers();
            ViewBag.ProducersList = new SelectList(producers, "ID", "Name", model.ProducerID);
            return View(model);
        }


        // GET: Ducks/Create
        [HttpGet]
        public IActionResult Add()
        {
            // 1. Musimy załadować listę producentów do dropdowna
            var producers = _blc.GetAllProducers();
            ViewBag.ProducersList = new SelectList(producers, "ID", "Name");

            // 2. Przekazujemy pusty model, żeby formularz wiedział jakie ma pola
            return View(new Duck());
        }

        // POST: Ducks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Duck model)
        {
            // Tu nie sprawdzamy ID, bo to nowa kaczka

            if (ModelState.IsValid)
            {
                try
                {
                    var domainDuck = _blc.GetNewDuck();

                    // Przepisujemy dane
                    domainDuck.Name = model.Name;
                    domainDuck.Price = (double)model.Price;
                    domainDuck.Description = model.Description ?? "";
                    domainDuck.Category = (Strzelecki_Baranowski.DuckApp.CORE.Category)model.Category;
                    domainDuck.ProducerID = model.ProducerID;

                    // --- LOGIKA ZDJĘCIA ---
                    if (model.PhotoUpload != null)
                    {
                        // Tworzymy folder i nazwę pliku
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "photos");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.PhotoUpload.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Zapisujemy na dysku
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.PhotoUpload.CopyToAsync(fileStream);
                        }

                        // Ścieżka do bazy
                        domainDuck.Photo = filePath ?? "";
                    }
                    else
                    {
                        // Jeśli nie wybrano zdjęcia, zapisujemy pusty string lub null
                        domainDuck.Photo = string.Empty;
                    }

                    // --- ZAPIS (AddNewDuck) ---
                    _blc.AddNewDuck(domainDuck);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Nie udało się dodać kaczki: " + ex.Message);
                }
            }

            // Jeśli walidacja nie przeszła, ładujemy listę producentów ponownie
            var producers = _blc.GetAllProducers();
            ViewBag.ProducersList = new SelectList(producers, "ID", "Name", model.ProducerID);

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            // 1. Pobieramy kaczkę, żeby pokazać użytkownikowi CO usuwa
            var coreDuck = _blc.GetAllDucks().FirstOrDefault(d => d.ID == id);
            if (coreDuck == null) return NotFound();

            // 2. Mapujemy na model (żeby wyświetlić zdjęcie i nazwę)
            var model = new Duck
            {
                ID = coreDuck.ID,
                Name = coreDuck.Name,
                Price = (decimal)coreDuck.Price,
                Description = coreDuck.Description,
                Category = (Category)coreDuck.Category,
                ProducerID = coreDuck.ProducerID,
                // Używamy bezpiecznej ścieżki (pamiętasz "pancerną" poprawkę?)
                Photo = coreDuck.Photo ?? string.Empty
            };

            return View(model);
        }

        // POST: Ducks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Wywołujemy usuwanie z bazy (BLC)
            // UWAGA: Usunąłem kod System.IO.File.Delete - zdjęcia są bezpieczne!
            _blc.DeleteDuck(id);

            // Dodajemy komunikat, który wyświetli się RAZ na liście (TempData)
            TempData["SuccessMessage"] = "Sukces! Kaczka została usunięta z bazy.";

            return RedirectToAction(nameof(Index));
        }

    }
}
