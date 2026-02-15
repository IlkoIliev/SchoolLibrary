using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Models.Loans;
using SchoolLibrary.Services;

namespace SchoolLibrary.Controllers
{
    public class LoansController : Controller
    {
        private readonly LoanService _loanService;
        private readonly StudentService _studentService;
        private readonly BookService _bookService;

        public LoansController(LoanService loanService, StudentService studentService, BookService bookService)
        {
            _loanService = loanService;
            _studentService = studentService;
            _bookService = bookService;
        }

        // Списък активни заеми
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var result = await _loanService.GetActivePagedAsync(page, pageSize);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new LoanCreateFormModel
            {
                Students = await _studentService.GetSelectListAsync(),
                Books = await _bookService.GetSelectListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoanCreateFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Students = await _studentService.GetSelectListAsync(model.StudentId);
                model.Books = await _bookService.GetSelectListAsync(model.BookId);
                return View(model);
            }

            var (ok, message) = await _loanService.CreateAsync(model);

            if (!ok)
            {
                ModelState.AddModelError("", message);
                model.Students = await _studentService.GetSelectListAsync(model.StudentId);
                model.Books = await _bookService.GetSelectListAsync(model.BookId);
                return View(model);
            }

            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id)
        {
            var (ok, message) = await _loanService.ReturnAsync(id);
            TempData["Message"] = message;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> History(int page = 1, int pageSize = 10)
        {
            var result = await _loanService.GetHistoryPagedAsync(page, pageSize);
            return View(result);
        }
    }
}
