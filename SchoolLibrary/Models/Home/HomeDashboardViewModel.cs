using SchoolLibrary.Data.Entities;

namespace SchoolLibrary.Models.Home
{
    public class HomeDashboardViewModel
    {
        public int BooksCount { get; set; }
        public int AuthorsCount { get; set; }
        public int StudentsCount { get; set; }
        public int ActiveLoansCount { get; set; }

        public List<Loan> LastActiveLoans { get; set; } = new();
    }
}
