using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Data;
using SchoolLibrary.Data.Entities;
using SchoolLibrary.Models.Books;
using SchoolLibrary.Repositories;
using SchoolLibrary.Services;

namespace SchoolLibrary.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookService _service;
        private readonly AuthorService _authorService;

        public BooksController(BookService service, AuthorService authorService)
        {
            _service = service;
            _authorService = authorService;
        }

        public async Task<IActionResult> Index()
            => View(await _service.GetAllAsync());

        public async Task<IActionResult> Details(int id)
        {
            var book = await _service.GetByIdAsync(id);
            return book == null ? NotFound() : View(book);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new BookFormModel
            {
                Authors = await _authorService.GetSelectListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Authors = await _authorService.GetSelectListAsync(model.AuthorId);
                return View(model);
            }

            var (ok, message) = await _service.AddAsync(model);
            if (!ok)
            {
                ModelState.AddModelError("", message);
                model.Authors = await _authorService.GetSelectListAsync(model.AuthorId);
                return View(model);
            }

            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _service.GetByIdAsync(id);
            if (book == null) return NotFound();

            var model = new BookFormModel
            {
                Id = book.Id,
                Title = book.Title,
                Year = book.Year,
                AuthorId = book.AuthorId,
                Authors = await _authorService.GetSelectListAsync(book.AuthorId)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Authors = await _authorService.GetSelectListAsync(model.AuthorId);
                return View(model);
            }

            var (ok, message) = await _service.UpdateAsync(model);
            if (!ok)
            {
                ModelState.AddModelError("", message);
                model.Authors = await _authorService.GetSelectListAsync(model.AuthorId);
                return View(model);
            }

            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _service.GetByIdAsync(id);
            return book == null ? NotFound() : View(book);
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
