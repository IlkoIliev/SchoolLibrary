using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolLibrary.Data.Entities;
using SchoolLibrary.Models.Authors;
using SchoolLibrary.Repositories;

namespace SchoolLibrary.Services
{
    public class AuthorService
    {
        private readonly AuthorRepository _repo;

        public AuthorService(AuthorRepository repo)
        {
            _repo = repo;
        }

        public Task<List<Author>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<Author?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        public async Task<(bool ok, string message)> AddAsync(AuthorFormModel model)
        {
            var name = model.Name.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Името е задължително.");

            if (await _repo.ExistsByNameAsync(name))
                return (false, "Вече съществува автор с това име.");

            await _repo.AddAsync(new Author { Name = name });
            return (true, "Авторът е добавен успешно.");
        }

        public async Task<AuthorFormModel?> GetFormByIdAsync(int id)
        {
            var author = await _repo.GetByIdAsync(id);
            if (author == null) return null;

            return new AuthorFormModel
            {
                Id = author.Id,
                Name = author.Name
            };
        }

        public async Task<(bool ok, string message)> UpdateAsync(AuthorFormModel model)
        {
            if (model.Id <= 0)
                return (false, "Невалиден идентификатор.");

            var name = model.Name.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Името е задължително.");

            var author = await _repo.GetByIdAsync(model.Id);
            if (author == null)
                return (false, "Авторът не е намерен.");

            if (await _repo.ExistsByNameExceptIdAsync(name, model.Id))
                return (false, "Вече съществува автор с това име.");

            author.Name = name;
            await _repo.UpdateAsync(author);
            return (true, "Авторът е обновен успешно.");
        }

        public async Task<(bool ok, string message)> DeleteAsync(int id)
        {
            if (id <= 0)
                return (false, "Невалиден идентификатор.");

            if (await _repo.HasBooksAsync(id))
                return (false, "Авторът не може да бъде изтрит, защото има книги към него.");

            await _repo.DeleteAsync(id);
            return (true, "Авторът е изтрит успешно.");
        }

        public async Task<List<SelectListItem>> GetSelectListAsync(int selectedAuthorId = 0)
        {
            var authors = await GetAllAsync();

            var items = authors
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name,
                    Selected = (a.Id == selectedAuthorId)
                })
                .ToList();

            items.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "-- Избери автор --",
                Selected = (selectedAuthorId == 0)
            });

            return items;
        }

        public Task<int> CountAsync()
            => _repo.CountAsync();
    }
}
