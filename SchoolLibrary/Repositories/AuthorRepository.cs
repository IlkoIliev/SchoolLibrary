using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Data;
using SchoolLibrary.Data.Entities;
using SchoolLibrary.Repositories.Interfaces;

namespace SchoolLibrary.Repositories
{
    public class AuthorRepository : IRepository<Author>
    {
        private readonly SchoolLibraryContext _context;

        public AuthorRepository(SchoolLibraryContext context)
        {
            _context = context;
        }

        public Task<List<Author>> GetAllAsync()
        => _context.Authors
            .OrderBy(a => a.Name)
            .ToListAsync();

        public Task<Author?> GetByIdAsync(int id)
            => _context.Authors.FirstOrDefaultAsync(a => a.Id == id);

        public async Task AddAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Author author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return;

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }

        public Task<bool> HasBooksAsync(int authorId)
        {
            return _context.Books.AsNoTracking()
                .AnyAsync(b => b.AuthorId == authorId);
        }

        public Task<bool> ExistsByNameAsync(string name)
        {
            name = name.Trim();
            return _context.Authors.AnyAsync(a => a.Name == name);
        }

        public Task<bool> ExistsByNameExceptIdAsync(string name, int id)
        {
            name = name.Trim();
            return _context.Authors.AnyAsync(a => a.Id != id && a.Name == name);
        }

        public Task<int> CountAsync()
            => _context.Authors.CountAsync();
    }
}
