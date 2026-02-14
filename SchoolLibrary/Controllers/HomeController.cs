using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Models;
using SchoolLibrary.Models.Home;
using SchoolLibrary.Services;
using System.Diagnostics;

namespace SchoolLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BookService _bookService;
        private readonly AuthorService _authorService;
        private readonly StudentService _studentService;
        private readonly LoanService _loanService;

        public HomeController(ILogger<HomeController> logger,
               BookService bookService,
               AuthorService authorService,
               StudentService studentService,
               LoanService loanService)
        {
            _logger = logger;
            _bookService = bookService;
            _authorService = authorService;
            _studentService = studentService;
            _loanService = loanService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeDashboardViewModel
            {
                BooksCount = await _bookService.CountAsync(),
                AuthorsCount = await _authorService.CountAsync(),
                StudentsCount = await _studentService.CountAsync(),
                ActiveLoansCount = await _loanService.CountActiveAsync(),
                LastActiveLoans = await _loanService.GetLastActiveAsync(5)
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
