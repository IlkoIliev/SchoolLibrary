using SchoolLibrary.Data.Entities;
using SchoolLibrary.Models.Loans;
using SchoolLibrary.Repositories;

namespace SchoolLibrary.Services
{
    public class LoanService
    {
        private readonly LoanRepository _repo;

        public LoanService(LoanRepository repo)
            => _repo = repo;

        public Task<List<Loan>> GetActiveAsync()
            => _repo.GetActiveAsync();

        public Task<List<Loan>> GetAllAsync()
            => _repo.GetAllAsync();

        public async Task<(bool ok, string message)> CreateAsync(LoanCreateFormModel model)
        {
            if (model.StudentId <= 0)
                return (false, "Моля, изберете ученик.");

            if (model.BookId <= 0)
                return (false, "Моля, изберете книга.");

            if (await _repo.IsBookCurrentlyLoanedAsync(model.BookId))
                return (false, "Книгата вече е заета и не може да бъде дадена отново.");

            var loan = new Loan
            {
                StudentId = model.StudentId,
                BookId = model.BookId,
                LoanDate = DateOnly.FromDateTime(DateTime.Now),
                ReturnDate = null
            };

            await _repo.AddAsync(loan);
            return (true, "Заемът е създаден успешно.");
        }

        public async Task<(bool ok, string message)> ReturnAsync(int loanId)
        {
            if (loanId <= 0)
                return (false, "Невалиден идентификатор.");

            var loan = await _repo.GetByIdAsync(loanId);
            if (loan == null)
                return (false, "Заемът не е намерен.");

            if (loan.ReturnDate != null)
                return (false, "Този заем вече е приключен (книгата е върната).");

            loan.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
            await _repo.UpdateAsync(loan);

            return (true, "Книгата е върната успешно.");
        }

        public Task<List<Loan>> GetHistoryAsync()
            => _repo.GetHistoryAsync();

        public Task<bool> BookHasActiveLoansAsync(int bookId)
            => _repo.HasActiveLoansForBookAsync(bookId);

        public Task<bool> StudentHasActiveLoansAsync(int studentId)
            => _repo.HasActiveLoansForStudentAsync(studentId);

        public Task<int> CountActiveAsync()
            => _repo.CountActiveAsync();

        public Task<List<Loan>> GetLastActiveAsync(int take)
            => _repo.GetLastActiveAsync(take);
    }
}
