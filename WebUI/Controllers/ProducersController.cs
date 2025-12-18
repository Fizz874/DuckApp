using Microsoft.AspNetCore.Mvc;
using Strzelecki_Baranowski.DuckApp.WebUI.Models;
using Strzelecki_Baranowski.DuckApp.BL;
using System.Linq;
using Strzelecki_Baranowski.DuckApp.CORE;
using Strzelecki_Baranowski.DuckApp.INTERFACES;
using Strzelecki_Baranowski.DuckApp.WebUI;

namespace Strzelecki_Baranowski.DuckApp.Web.Controllers
{
    public class ProducersController : Controller
    {
        private readonly BLC _blc;

        public ProducersController(BLC blc)
        {
            _blc = blc;
        }

        public IActionResult Index(ProducerFilterViewModel filter, bool reset = false)
        {
            if (reset)
            {
                HttpContext.Session.Remove("ProducerFilterState");
                filter = new ProducerFilterViewModel();
            }
            else if (Request.Query.Count == 0)
            {
                var storedFilter = HttpContext.Session.Get<ProducerFilterViewModel>("ProducerFilterState");
                if (storedFilter != null) filter = storedFilter;
            }
            else
            {
                HttpContext.Session.Set("ProducerFilterState", filter);
            }

            IEnumerable<IProducer> query = _blc.GetAllProducers();

            if (filter.ID.HasValue)
            {
                int val = filter.ID.Value;
                query = filter.IDMode switch
                {
                    FilterMode.Equal => query.Where(p => p.ID == val),
                    FilterMode.NotEqual => query.Where(p => p.ID != val),
                    FilterMode.Greater => query.Where(p => p.ID > val),
                    FilterMode.Less => query.Where(p => p.ID < val),
                    FilterMode.GreaterOrEqual => query.Where(p => p.ID >= val),
                    FilterMode.LessOrEqual => query.Where(p => p.ID <= val),
                    _ => query
                };
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = filter.NameMode switch
                {
                    FilterMode.Contains => query.Where(p => p.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase)),
                    FilterMode.NotContains => query.Where(p => !p.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase)),
                    FilterMode.Equal => query.Where(p => p.Name.Equals(filter.Name, StringComparison.OrdinalIgnoreCase)),
                    FilterMode.NotEqual => query.Where(p => !p.Name.Equals(filter.Name, StringComparison.OrdinalIgnoreCase)),
                    _ => query
                };
            }

            if (!string.IsNullOrEmpty(filter.Website))
            {
                query = filter.WebsiteMode switch
                {
                    FilterMode.Contains => query.Where(p => (p.Website ?? string.Empty).Contains(filter.Website, StringComparison.OrdinalIgnoreCase)),
                    FilterMode.NotContains => query.Where(p => !(p.Website ?? string.Empty).Contains(filter.Website, StringComparison.OrdinalIgnoreCase)),
                    FilterMode.Equal => query.Where(p => (p.Website ?? string.Empty).Equals(filter.Website, StringComparison.OrdinalIgnoreCase)),
                    _ => query
                };
            }

            var producerModels = query.Select(p => new Producer
            {
                ID = p.ID,
                Name = p.Name,
                Website = p.Website
            }).ToList();

            var viewModel = new ProducerIndexViewModel
            {
                Producers = producerModels,
                Filter = filter
            };

            return View(viewModel);
        }

        public IActionResult Details(int id)
        {
            var coreProducer = _blc.GetAllProducers().FirstOrDefault(p => p.ID == id);

            if (coreProducer == null)
            {
                return NotFound();
            }

            var coreDucks = _blc.GetAllDucks().Where(d => d.ProducerID == id);

            var model = new Producer
            {
                ID = coreProducer.ID,
                Name = coreProducer.Name,
                Website = coreProducer.Website,

                Ducks = coreDucks.Select(d => new Duck
                {
                    ID = d.ID,
                    Name = d.Name,
                    Price = (decimal)d.Price,
                    Photo = d.Photo
                }).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var coreProducer = _blc.GetAllProducers().FirstOrDefault(p => p.ID == id);

            if (coreProducer == null) return NotFound();

            var model = new Producer
            {
                ID = coreProducer.ID,
                Name = coreProducer.Name,
                Website = coreProducer.Website ?? string.Empty 
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Producer model)
        {
            if (id != model.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var domainProducer = _blc.GetNewProducer();

                    domainProducer.ID = model.ID;
                    domainProducer.Name = model.Name;
                    domainProducer.Website = model.Website ?? string.Empty; 

                    _blc.UpdateProducer(domainProducer);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving: " + ex.Message);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new Producer());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Producer model)
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
                    ModelState.AddModelError("", "Error: " + ex.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var coreProducer = _blc.GetAllProducers().FirstOrDefault(p => p.ID == id);
            if (coreProducer == null) return NotFound();

            var producerDucks = _blc.GetAllDucks().Where(d => d.ProducerID == id).ToList();

            var model = new Producer
            {
                ID = coreProducer.ID,
                Name = coreProducer.Name,
                Website = coreProducer.Website,

                Ducks = producerDucks.Select(d => new Duck
                {
                    ID = d.ID,
                    Name = d.Name
                }).ToList()
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
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