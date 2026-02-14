using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SchoolLibrary.Data.Entities;

public partial class Loan
{
    [Key]
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int BookId { get; set; }

    public DateOnly LoanDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    [ForeignKey("BookId")]
    [InverseProperty("Loans")]
    public virtual Book Book { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("Loans")]
    public virtual Student Student { get; set; } = null!;
}
