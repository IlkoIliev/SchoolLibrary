using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Models.Students;
using SchoolLibrary.Services;

namespace SchoolLibrary.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentService _service;

        public StudentsController(StudentService service)
            => _service = service;

        public async Task<IActionResult> Index()
            => View(await _service.GetAllAsync());

        public async Task<IActionResult> Details(int id)
        {
            var student = await _service.GetByIdAsync(id);
            return student == null ? NotFound() : View(student);
        }

        [HttpGet]
        public IActionResult Create()
            => View(new StudentFormModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentFormModel model)
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
            var student = await _service.GetByIdAsync(id);
            if (student == null) return NotFound();

            var model = new StudentFormModel
            {
                Id = student.Id,
                FullName = student.FullName,
                Grade = student.Grade
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StudentFormModel model)
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
            var student = await _service.GetByIdAsync(id);
            return student == null ? NotFound() : View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (ok, message) = await _service.DeleteAsync(id);
            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));
        }
    }
}
