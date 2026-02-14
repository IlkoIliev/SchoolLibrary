using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Data;
using SchoolLibrary.Data.Entities;
using SchoolLibrary.Repositories.Interfaces;

namespace SchoolLibrary.Repositories
{
    public class StudentRepository : IRepository<Student>
    {
        private readonly SchoolLibraryContext _context;

        public StudentRepository(SchoolLibraryContext context)
            => _context = context;

        public Task<List<Student>> GetAllAsync()
            => _context.Students
                .OrderBy(s => s.FullName)
                .ToListAsync();

        public Task<Student?> GetByIdAsync(int id)
            => _context.Students.FirstOrDefaultAsync(s => s.Id == id);

        public async Task AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }

        // Полезно за Loans: dropdown списък
        public Task<List<Student>> GetForSelectAsync()
            => _context.Students
                .OrderBy(s => s.FullName)
                .ToListAsync();

        public Task<int> CountAsync()
            => _context.Students.CountAsync();
    }
}
