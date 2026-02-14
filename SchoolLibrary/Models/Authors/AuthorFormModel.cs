using System.ComponentModel.DataAnnotations;

namespace SchoolLibrary.Models.Authors
{
    public class AuthorFormModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името е задължително.")]
        [StringLength(120, ErrorMessage = "Името може да е до 120 символа.")]
        public string Name { get; set; } = string.Empty;
    }
}
