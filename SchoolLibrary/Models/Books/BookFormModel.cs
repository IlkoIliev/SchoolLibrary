using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SchoolLibrary.Models.Books
{
    public class BookFormModel
    {
        public int Id { get; set; }   // 0 при Create, >0 при Edit

        [Required(ErrorMessage = "Заглавието е задължително.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Годината е задължителна.")]
        [Range(1450, 2026, ErrorMessage = "Годината трябва да е между 1450 и 2026.")]
        public int? Year { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Моля, изберете автор.")]
        public int AuthorId { get; set; }

        public List<SelectListItem> Authors { get; set; } = new();
    }
}
