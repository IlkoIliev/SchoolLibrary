using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolLibrary.Data.Entities;
using SchoolLibrary.Models.Books;
using SchoolLibrary.Models.Paging;
using SchoolLibrary.Repositories;

namespace SchoolLibrary.Services
{
    public class BookService
    {
        private readonly BookRepository _repo;
        private readonly LoanService _loanService;

        public BookService(BookRepository repo, LoanService loanService)
        {
            _repo = repo;
            _loanService = loanService;
        }

        public Task<List<Book>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<Book?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        public async Task<(bool ok, string message)> AddAsync(BookFormModel model)
        {
            var title = model.Title.Trim();

            if (await _repo.ExistsByTitleAsync(title))
                return (false, "Вече съществува книга със същото заглавие.");

            var book = new Book
            {
                Title = model.Title.Trim(),
                Year = model.Year,
                AuthorId = model.AuthorId
            };

            await _repo.AddAsync(book);
            return (true, "Книгата е добавена успешно.");
        }

        public async Task<(bool ok, string message)> UpdateAsync(BookFormModel model)
        {
            if (model.Id <= 0)
                return (false, "Невалиден идентификатор.");

            if (string.IsNullOrWhiteSpace(model.Title))
                return (false, "Заглавието е задължително.");

            if (model.AuthorId <= 0)
                return (false, "Моля, изберете автор.");

            // Проверка дали книгата реално съществува
            var existing = await _repo.GetByIdAsync(model.Id);
            if (existing == null)
                return (false, "Книгата не е намерена.");

            if (await _repo.ExistsByTitleAsync(model.Title))
                return (false, "Вече съществува книга със същото заглавие.");

            // Обновяване само на нужните полета
            existing.Title = model.Title.Trim();
            existing.Year = model.Year;
            existing.AuthorId = model.AuthorId;

            await _repo.UpdateAsync(existing);

            return (true, "Книгата е обновена успешно.");
        }

        public async Task<(bool ok, string message)> DeleteAsync(int id)
        {
            if (id <= 0)
                return (false, "Невалиден идентификатор.");

            if (await _loanService.BookHasActiveLoansAsync(id))
                return (false, "Книгата не може да бъде изтрита, защото има активен заем.");

            await _repo.DeleteAsync(id);
            return (true, "Книгата е изтрита успешно.");
        }

        public async Task<List<SelectListItem>> GetSelectListAsync(int selectedBookId = 0)
        {
            var books = await _repo.GetAllAsync();

            var items = books.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = $"{b.Title} ({b.Author?.Name})",
                Selected = b.Id == selectedBookId
            }).ToList();

            items.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "-- Избери книга --",
                Selected = selectedBookId == 0
            });

            return items;
        }

        public Task<int> CountAsync()
            => _repo.CountAsync();

        public async Task<PagedResult<Book>> GetPagedAsync(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 50) pageSize = 50; // защита

            var (items, totalItems) = await _repo.GetPagedAsync(page, pageSize);

            return new PagedResult<Book>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }
    }
}
