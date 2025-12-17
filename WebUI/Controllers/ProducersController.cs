using Microsoft.AspNetCore.Mvc;
using Strzelecki_Baranowski.DuckApp.WebUI.Models;
using Strzelecki_Baranowski.DuckApp.BL; 
using System.Linq;

namespace Strzelecki_Baranowski.DuckApp.Web.Controllers
{
    public class ProducersController : Controller
    {
        private readonly BLC _blc;

        public ProducersController(BLC blc)
        {
            _blc = blc;
        }

        // GET: Producers (Lista kafelków)
        public IActionResult Index()
        {
            var coreProducers = _blc.GetAllProducers();

            // Mapowanie: IProducer -> Producer (MVC)
            var model = coreProducers.Select(p => new Producer
            {
                ID = p.ID,
                Name = p.Name,
                Website = p.Website
            }).ToList();

            return View(model);
        }

        // GET: Producers/Details/5
        public IActionResult Details(int id)
        {
            // 1. Pobierz producenta
            var coreProducer = _blc.GetAllProducers().FirstOrDefault(p => p.ID == id);

            if (coreProducer == null)
            {
                return NotFound();
            }

            // 2. Pobierz kaczki tego producenta (Relacja jeden-do-wielu)
            // Filtrujemy wszystkie kaczki, gdzie ProducerID == id obecnego producenta
            var coreDucks = _blc.GetAllDucks().Where(d => d.ProducerID == id);

            // 3. Mapowanie producenta
            var model = new Producer
            {
                ID = coreProducer.ID,
                Name = coreProducer.Name,
                Website = coreProducer.Website,

                // 4. Mapowanie listy jego kaczek
                Ducks = coreDucks.Select(d => new Duck
                {
                    ID = d.ID,
                    Name = d.Name,
                    Price = (decimal)d.Price,
                    Photo = d.Photo//System.IO.Path.GetFileName(d.Photo) // Samo nazwa pliku
                }).ToList()
            };

            return View(model);
        }

        // GET: Producers/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // 1. Pobierz producenta z BLC
            var coreProducer = _blc.GetAllProducers().FirstOrDefault(p => p.ID == id);

            if (coreProducer == null) return NotFound();

            // 2. Zmapuj na Model MVC
            var model = new Producer
            {
                ID = coreProducer.ID,
                Name = coreProducer.Name,
                Website = coreProducer.Website ?? ""
            };

            return View(model);
        }

        // POST: Producers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Producer model)
        {
            if (id != model.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Stwórz obiekt dla BLC
                    var domainProducer = _blc.GetNewProducer();

                    // 2. Przepisz dane
                    domainProducer.ID = model.ID;
                    domainProducer.Name = model.Name;
                    domainProducer.Website = model.Website ?? "";

                    // 3. Zapisz
                    _blc.UpdateProducer(domainProducer);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Błąd zapisu: " + ex.Message);
                }
            }

            // Jeśli walidacja nie przeszła, wyświetl formularz ponownie
            return View(model);
        }

        // 1. Zmień nazwę metody GET
        [HttpGet]
        public IActionResult Add() // Było: Create
        {
            return View(new Producer());
        }

        // 2. Zmień nazwę metody POST i w formularzu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Producer model) // Było:
                                                 // te
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var domainProducer = _blc.GetNewProducer();
                    domainProducer.Name = model.Name;
                    domainProducer.Website = model.Website ?? string.Empty;

                    _blc.AddNewProducer(domainProducer);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Błąd: " + ex.Message);
                }
            }
            return View(model);
        }

        // GET: Producers/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var coreProducer = _blc.GetAllProducers().FirstOrDefault(p => p.ID == id);
            if (coreProducer == null) return NotFound();

            // 1. Pobieramy kaczki tego producenta, żeby tylko JE POLICZYĆ
            var producerDucks = _blc.GetAllDucks().Where(d => d.ProducerID == id).ToList();

            var model = new Producer
            {
                ID = coreProducer.ID,
                Name = coreProducer.Name,
                Website = coreProducer.Website,

                // Mapujemy kaczki, żeby widok wiedział, ile ich jest
                Ducks = producerDucks.Select(d => new Duck
                {
                    ID = d.ID,
                    Name = d.Name
                    // reszta pól niepotrzebna do samego licznika
                }).ToList()
            };

            return View(model);
        }

        // POST: Producers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                // 2. CZYSTY POST: Po prostu usuwamy producenta.
                // Skoro DAO robi kaskadę, to kaczki znikną same.
                _blc.DeleteProducer(id);

                TempData["SuccessMessage"] = "Producer and all associated ducks were deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting producer: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }   

    }
}