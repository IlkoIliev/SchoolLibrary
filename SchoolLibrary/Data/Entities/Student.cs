using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SchoolLibrary.Data.Entities;

public partial class Student
{
    [Key]
    public int Id { get; set; }

    [StringLength(120)]
    public string FullName { get; set; } = null!;

    public int Grade { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
