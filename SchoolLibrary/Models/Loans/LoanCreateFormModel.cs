using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SchoolLibrary.Models.Loans
{
    public class LoanCreateFormModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Моля, изберете ученик.")]
        public int StudentId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Моля, изберете книга.")]
        public int BookId { get; set; }

        public List<SelectListItem> Students { get; set; } = new();
        public List<SelectListItem> Books { get; set; } = new();
    }
}
