using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Data;
using SchoolLibrary.Models.Search;

namespace SchoolLibrary.Controllers
{
    public class SearchController : Controller
    {
        private readonly SchoolLibraryContext _context;

        public SearchController(SchoolLibraryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return View(new SearchResultViewModel());

            q = q.Trim();

            var model = new SearchResultViewModel
            {
                Query = q,

                Books = await _context.Books
                    .Include(b => b.Author)
                    .Where(b =>
                        b.Title.Contains(q) ||
                        b.Author!.Name.Contains(q))
                    .Take(10)
                    .ToListAsync(),

                Authors = await _context.Authors
                    .Where(a => a.Name.Contains(q))
                    .Take(10)
                    .ToListAsync(),

                Students = await _context.Students
                    .Where(s =>
                        s.FullName.Contains(q) ||
                        s.Grade.ToString().Contains(q))
                    .Take(10)
                    .ToListAsync(),

                Loans = await _context.Loans
                    .Include(l => l.Student)
                    .Include(l => l.Book).ThenInclude(b => b.Author)
                    .Where(l =>
                        l.Student.FullName.Contains(q) ||
                        l.Book.Title.Contains(q) ||
                        l.Book.Author!.Name.Contains(q))
                    .OrderByDescending(l => l.LoanDate)
                    .Take(10)
                    .ToListAsync(),
            };

            return View(model);
        }
    }
}
