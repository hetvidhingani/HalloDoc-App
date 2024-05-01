using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Entities.DataModels;

[Table("PayRate")]
public partial class PayRate
{
    [Key]
    public int PayRateId { get; set; }

    public int? CatagoryId { get; set; }

    [Column("rate")]
    public int? Rate { get; set; }

    [Column("physicianId")]
    public int? PhysicianId { get; set; }

    [ForeignKey("PhysicianId")]
    [InverseProperty("PayRates")]
    public virtual Physician? Physician { get; set; }
}
