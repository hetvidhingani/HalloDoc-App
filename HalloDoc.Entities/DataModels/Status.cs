using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Entities.DataModels;

[Table("status")]
public partial class Status
{
    [Key]
    [Column("statusid")]
    public int Statusid { get; set; }

    [Column("statusname")]
    [StringLength(50)]
    public string Statusname { get; set; } = null!;

    [InverseProperty("StatusNavigation")]
    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    [InverseProperty("StatusNavigation")]
    public virtual ICollection<Physician> Physicians { get; set; } = new List<Physician>();
}
