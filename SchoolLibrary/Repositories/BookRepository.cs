using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Data;
using SchoolLibrary.Data.Entities;
using SchoolLibrary.Repositories.Interfaces;

namespace SchoolLibrary.Repositories
{
    public class BookRepository : IRepository<Book>
    {
        private readonly SchoolLibraryContext _context;

        public BookRepository(SchoolLibraryContext context)
            => _context = context;

        public Task<List<Book>> GetAllAsync()
            => _context.Books.Include(b => b.Author).ToListAsync();

        public Task<Book?> GetByIdAsync(int id)
            => _context.Books.Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByTitleAsync(string title)
        {
            return await _context.Books
                .AnyAsync(b => b.Title == title);
        }

        public Task<int> CountAsync()
            => _context.Books.CountAsync();

        public async Task<(List<Book> items, int totalItems)> GetPagedAsync(int page, int pageSize)
        {
            var query = _context.Books
                .Include(b => b.Author)
                .AsQueryable();

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderBy(b => b.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalItems);
        }
    }
}
