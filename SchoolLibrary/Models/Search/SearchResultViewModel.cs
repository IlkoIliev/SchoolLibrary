using SchoolLibrary.Data.Entities;

namespace SchoolLibrary.Models.Search
{
    public class SearchResultViewModel
    {
        public string Query { get; set; } = string.Empty;

        public List<Book> Books { get; set; } = new();
        public List<Author> Authors { get; set; } = new();
        public List<Student> Students { get; set; } = new();
        public List<Loan> Loans { get; set; } = new();
    }
}
