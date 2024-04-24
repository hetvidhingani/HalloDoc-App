using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Entities.DataModels;

[Table("doctor")]
public partial class Doctor
{
    [Key]
    [Column("doctorid")]
    public int Doctorid { get; set; }

    [Column("specialist")]
    [StringLength(50)]
    public string Specialist { get; set; } = null!;

    [InverseProperty("Doctor")]
    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
}
