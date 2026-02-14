using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Models.Authors;
using SchoolLibrary.Services;

namespace SchoolLibrary.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly AuthorService _service;

        public AuthorsController(AuthorService service)
            => _service = service;

        public async Task<IActionResult> Index()
            => View(await _service.GetAllAsync());

        public async Task<IActionResult> Details(int id)
        {
            var author = await _service.GetByIdAsync(id);
            return author == null ? NotFound() : View(author);
        }

        [HttpGet]
        public IActionResult Create()
            => View(new AuthorFormModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorFormModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (ok, message) = await _service.AddAsync(model);
            if (!ok)
            {
                ModelState.AddModelError("", message);
                return View(model);
            }

            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _service.GetFormByIdAsync(id);
            return model == null ? NotFound() : View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AuthorFormModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (ok, message) = await _service.UpdateAsync(model);
            if (!ok)
            {
                ModelState.AddModelError("", message);
                return View(model);
            }

            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _service.GetByIdAsync(id);
            return author == null ? NotFound() : View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (ok, message) = await _service.DeleteAsync(id);

            if (!ok)
            {
                // Връщаме същия Delete view с грешка
                ModelState.AddModelError("", message);
                var author = await _service.GetByIdAsync(id);
                return author == null ? NotFound() : View(author);
            }

            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));
        }
    }
}
