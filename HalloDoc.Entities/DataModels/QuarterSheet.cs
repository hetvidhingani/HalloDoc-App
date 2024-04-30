using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Entities.DataModels;

[Table("QuarterSheet")]
public partial class QuarterSheet
{
    [Key]
    public int TimeSheetId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    public int? Status { get; set; }

    public int? PhysicianId { get; set; }

    public int? PayRate { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsFinalized { get; set; }

    [ForeignKey("PhysicianId")]
    [InverseProperty("QuarterSheets")]
    public virtual Physician? Physician { get; set; }

    [InverseProperty("TimeSheet")]
    public virtual ICollection<TimeSheetDetail> TimeSheetDetails { get; set; } = new List<TimeSheetDetail>();
}
