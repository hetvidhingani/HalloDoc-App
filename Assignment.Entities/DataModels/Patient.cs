using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Entities.DataModels;

[Table("Patient")]
public partial class Patient
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    public int DoctorId { get; set; }

    public int Age { get; set; }

    [StringLength(50)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    public string PhoneNo { get; set; } = null!;

    [StringLength(50)]
    public string Disease { get; set; } = null!;

    [Column("Specialist ")]
    [StringLength(50)]
    public string Specialist { get; set; } = null!;

    public int Gender { get; set; }

    [ForeignKey("DoctorId")]
    [InverseProperty("Patients")]
    public virtual Doctor Doctor { get; set; } = null!;
}
