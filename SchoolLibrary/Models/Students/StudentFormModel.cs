using System.ComponentModel.DataAnnotations;

namespace SchoolLibrary.Models.Students
{
    public class StudentFormModel
    {
        public int Id { get; set; } // 0 при Create, >0 при Edit

        [Required(ErrorMessage = "Името е задължително.")]
        [StringLength(120, ErrorMessage = "Името не може да е над 120 символа.")]
        public string FullName { get; set; } = string.Empty;

        [Range(1, 12, ErrorMessage = "Класът трябва да е между 1 и 12.")]
        public int Grade { get; set; }
    }
}
