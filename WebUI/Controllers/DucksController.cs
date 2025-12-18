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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DucksController(BLC blc, IWebHostEnvironment webHostEnvironment)
        {
            _blc = blc;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(DuckFilterViewModel filter, bool reset = false)
        {
            if (reset)
            {
                HttpContext.Session.Remove("DuckFilterState");
                filter = new DuckFilterViewModel();
            }
            else if (Request.Query.Count == 0)
            {
                var storedFilter = HttpContext.Session.Get<DuckFilterViewModel>("DuckFilterState");
                if (storedFilter != null)
                {
                    filter = storedFilter;
                }
            }
            else
            {
                HttpContext.Session.Set("DuckFilterState", filter);
            }

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

            if (!string.IsNullOrEmpty(filter.Description))
            {
                query = filter.DescriptionMode switch
                {
                    FilterMode.Contains => query.Where(d => d.Description?.Contains(filter.Description, StringComparison.OrdinalIgnoreCase) ?? false),
                    FilterMode.NotContains => query.Where(d => !(d.Description?.Contains(filter.Description, StringComparison.OrdinalIgnoreCase) ?? false)),
                    FilterMode.Equal => query.Where(d => (d.Description ?? "").Equals(filter.Description, StringComparison.OrdinalIgnoreCase)),
                    FilterMode.NotEqual => query.Where(d => !(d.Description ?? "").Equals(filter.Description, StringComparison.OrdinalIgnoreCase)),
                    _ => query
                };
            }

            if (filter.Category.HasValue)
                query = query.Where(d => (Category)d.Category == filter.Category.Value);

            if (filter.ProducerID.HasValue)
                query = query.Where(d => d.ProducerID == filter.ProducerID.Value);

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
                Filter = filter,
                ProducersList = new SelectList(_blc.GetAllProducers(), "ID", "Name", filter.ProducerID)
            };

            return View(viewModel);
        }

        public IActionResult Details(int id)
        {
            var coreDuck = _blc.GetAllDucks().FirstOrDefault(d => d.ID == id);

            if (coreDuck == null)
            {
                return NotFound();
            }

            var producers = _blc.GetAllProducers();
            var producer = producers.FirstOrDefault(p => p.ID == coreDuck.ProducerID);

            var model = new Duck
            {
                ID = coreDuck.ID,
                Name = coreDuck.Name,
                Price = (decimal)coreDuck.Price,
                Description = coreDuck.Description,
                Category = (Category)coreDuck.Category,
                ProducerID = coreDuck.ProducerID,
                Producer = producer != null ? new Producer { Name = producer.Name, ID = producer.ID } : null,
                Photo = coreDuck.Photo
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var coreDuck = _blc.GetAllDucks().FirstOrDefault(d => d.ID == id);
            if (coreDuck == null) return NotFound();

            var model = new Duck
            {
                ID = coreDuck.ID,
                Name = coreDuck.Name,
                Price = (decimal)coreDuck.Price,
                Description = coreDuck.Description,
                Category = (Category)coreDuck.Category,
                ProducerID = coreDuck.ProducerID,
                Photo = coreDuck.Photo
            };

            var producers = _blc.GetAllProducers();
            ViewBag.ProducersList = new SelectList(producers, "ID", "Name", model.ProducerID);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Duck model)
        {
            if (id != model.ID) return NotFound();

            if (ModelState.IsValid)
            {
                var domainDuck = _blc.GetNewDuck();
                domainDuck.ID = model.ID;
                domainDuck.Name = model.Name;
                domainDuck.Price = (double)model.Price;
                domainDuck.Description = model.Description ?? String.Empty;
                domainDuck.Category = (Strzelecki_Baranowski.DuckApp.CORE.Category)model.Category;
                domainDuck.ProducerID = model.ProducerID;

                if (model.PhotoUpload != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "photos");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.PhotoUpload.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.PhotoUpload.CopyToAsync(fileStream);
                    }

                    domainDuck.Photo = filePath;
                }
                else
                {
                    domainDuck.Photo = model.Photo ?? "";
                }
                try
                {

                    _blc.UpdateDuck(domainDuck);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Business logic error: {ex.Message}");
                }
            }

            var producers = _blc.GetAllProducers();
            ViewBag.ProducersList = new SelectList(producers, "ID", "Name", model.ProducerID);
            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var producers = _blc.GetAllProducers();
            ViewBag.ProducersList = new SelectList(producers, "ID", "Name");

            return View(new Duck());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Duck model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var domainDuck = _blc.GetNewDuck();
                    domainDuck.Name = model.Name;
                    domainDuck.Price = (double)model.Price;
                    domainDuck.Description = model.Description ?? "";
                    domainDuck.Category = (Strzelecki_Baranowski.DuckApp.CORE.Category)model.Category;
                    domainDuck.ProducerID = model.ProducerID;

                    if (model.PhotoUpload != null)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "photos");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.PhotoUpload.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.PhotoUpload.CopyToAsync(fileStream);
                        }

                        domainDuck.Photo = filePath ?? "";
                    }
                    else
                    {
                        domainDuck.Photo = string.Empty;
                    }

                    _blc.AddNewDuck(domainDuck);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Failed to add duck: " + ex.Message);
                }
            }
            var producers = _blc.GetAllProducers();
            ViewBag.ProducersList = new SelectList(producers, "ID", "Name", model.ProducerID);

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var coreDuck = _blc.GetAllDucks().FirstOrDefault(d => d.ID == id);
            if (coreDuck == null) return NotFound();

            var model = new Duck
            {
                ID = coreDuck.ID,
                Name = coreDuck.Name,
                Price = (decimal)coreDuck.Price,
                Description = coreDuck.Description,
                Category = (Category)coreDuck.Category,
                ProducerID = coreDuck.ProducerID,
                Photo = coreDuck.Photo ?? string.Empty
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _blc.DeleteDuck(id);

            TempData["SuccessMessage"] = "Successfully deleted a duck";

            return RedirectToAction(nameof(Index));
        }
    }
}
