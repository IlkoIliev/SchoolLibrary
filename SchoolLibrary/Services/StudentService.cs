using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolLibrary.Data.Entities;
using SchoolLibrary.Models.Students;
using SchoolLibrary.Repositories;

namespace SchoolLibrary.Services
{
    public class StudentService
    {
        private readonly StudentRepository _repo;
        private readonly LoanService _loanService;

        public StudentService(StudentRepository repo, LoanService loanService)
        {    
            _repo = repo;
            _loanService = loanService;
        }

        public Task<List<Student>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<Student?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        public async Task<(bool ok, string message)> AddAsync(StudentFormModel model)
        {
            var name = model.FullName.Trim();

            if (string.IsNullOrWhiteSpace(name))
                return (false, "Името е задължително.");

            if (model.Grade < 1 || model.Grade > 12)
                return (false, "Класът трябва да е между 1 и 12.");

            var student = new Student
            {
                FullName = name,
                Grade = model.Grade
            };

            await _repo.AddAsync(student);
            return (true, "Ученикът е добавен успешно.");
        }

        public async Task<(bool ok, string message)> UpdateAsync(StudentFormModel model)
        {
            if (model.Id <= 0)
                return (false, "Невалиден идентификатор.");

            var name = model.FullName.Trim();

            if (string.IsNullOrWhiteSpace(name))
                return (false, "Името е задължително.");

            if (model.Grade < 1 || model.Grade > 12)
                return (false, "Класът трябва да е между 1 и 12.");

            var existing = await _repo.GetByIdAsync(model.Id);
            if (existing == null)
                return (false, "Ученикът не е намерен.");

            existing.FullName = name;
            existing.Grade = model.Grade;

            await _repo.UpdateAsync(existing);
            return (true, "Ученикът е обновен успешно.");
        }

        public async Task<(bool ok, string message)> DeleteAsync(int id)
        {
            if (id <= 0)
                return (false, "Невалиден идентификатор.");

            if (await _loanService.StudentHasActiveLoansAsync(id))
                return (false, "Ученикът не може да бъде изтрит, защото има активни заеми.");

            await _repo.DeleteAsync(id);
            return (true, "Ученикът е изтрит успешно.");
        }

        // ⭐ за Loans: dropdown Student
        public async Task<List<SelectListItem>> GetSelectListAsync(int selectedId = 0)
        {
            var students = await _repo.GetForSelectAsync();

            var items = students.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = $"{s.FullName} (клас {s.Grade})",
                Selected = s.Id == selectedId
            }).ToList();

            items.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "-- Избери ученик --",
                Selected = selectedId == 0
            });

            return items;
        }

        public Task<int> CountAsync()
            => _repo.CountAsync();
    }
}
