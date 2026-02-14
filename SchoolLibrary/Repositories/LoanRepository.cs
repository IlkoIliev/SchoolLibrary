using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Data;
using SchoolLibrary.Data.Entities;
using SchoolLibrary.Repositories.Interfaces;

namespace SchoolLibrary.Repositories
{
    public class LoanRepository : IRepository<Loan>
    {
        private readonly SchoolLibraryContext _context;

        public LoanRepository(SchoolLibraryContext context)
            => _context = context;

        // 1) Активни заеми (невърнати книги)
        public Task<List<Loan>> GetActiveAsync()
            => _context.Loans
                .Include(l => l.Book).ThenInclude(b => b.Author)
                .Include(l => l.Student)
                .Where(l => l.ReturnDate == null)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();

        // 2) История (всички заеми)
        public Task<List<Loan>> GetAllAsync()
            => _context.Loans
                .Include(l => l.Book).ThenInclude(b => b.Author)
                .Include(l => l.Student)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();

        // 3) Проверка дали книга е заета в момента
        public Task<bool> IsBookCurrentlyLoanedAsync(int bookId)
            => _context.Loans.AnyAsync(l => l.BookId == bookId && l.ReturnDate == null);

        // 4) Добавяне на заем
        public async Task AddAsync(Loan loan)
        {
            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();
        }

        // 5) Вземаме заем по Id
        public Task<Loan?> GetByIdAsync(int id)
            => _context.Loans
                .Include(l => l.Book).ThenInclude(b => b.Author)
                .Include(l => l.Student)
                .FirstOrDefaultAsync(l => l.Id == id);

        // 6) Update (за ReturnDate)
        public async Task UpdateAsync(Loan loan)
        {
            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Loan>> GetHistoryAsync()
            => _context.Loans
                .Include(l => l.Student)
                .Include(l => l.Book).ThenInclude(b => b.Author)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();

        public Task<bool> HasActiveLoansForBookAsync(int bookId)
            => _context.Loans.AnyAsync(l => l.BookId == bookId && l.ReturnDate == null);

        public Task<bool> HasActiveLoansForStudentAsync(int studentId)
            => _context.Loans.AnyAsync(l => l.StudentId == studentId && l.ReturnDate == null);

        public Task<int> CountActiveAsync()
            => _context.Loans.CountAsync(l => l.ReturnDate == null);

        public Task<List<Loan>> GetLastActiveAsync(int take)
            => _context.Loans
                .Include(l => l.Student)
                .Include(l => l.Book).ThenInclude(b => b.Author)
                .Where(l => l.ReturnDate == null)
                .OrderByDescending(l => l.LoanDate)
                .Take(take)
                .ToListAsync();
    }
}
