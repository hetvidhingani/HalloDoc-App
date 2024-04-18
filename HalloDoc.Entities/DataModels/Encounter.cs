using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Entities.DataModels;

[Table("encounter")]
public partial class Encounter
{
    [Key]
    [Column("encounterid")]
    public int Encounterid { get; set; }

    [Column("requestid")]
    public int Requestid { get; set; }

    [Column("adminid")]
    public int? Adminid { get; set; }

    [Column("physicianid")]
    public int? Physicianid { get; set; }

    [Column("modifiedby")]
    [StringLength(100)]
    public string? Modifiedby { get; set; }

    [Column("firstname")]
    [StringLength(50)]
    public string? Firstname { get; set; }

    [Column("lastname")]
    [StringLength(50)]
    public string? Lastname { get; set; }

    [Column("location")]
    [StringLength(100)]
    public string? Location { get; set; }

    [Column("dateofbirth")]
    public DateTime? Dateofbirth { get; set; }

    [Column("phonenumber")]
    [StringLength(20)]
    public string? Phonenumber { get; set; }

    [Column("medicalreport")]
    public string? Medicalreport { get; set; }

    [Column("date")]
    public DateTime? Date { get; set; }

    [Column("email")]
    [StringLength(50)]
    public string? Email { get; set; }

    [Column("historyofpresentillness")]
    [StringLength(50)]
    public string? Historyofpresentillness { get; set; }

    [Column("medicalhistory")]
    [StringLength(50)]
    public string? Medicalhistory { get; set; }

    [Column("medications")]
    [StringLength(50)]
    public string? Medications { get; set; }

    [Column("temp")]
    public int? Temp { get; set; }

    [Column("bloodpressuresystolic")]
    public int? Bloodpressuresystolic { get; set; }

    [Column("bloodpressurediastolic")]
    public int? Bloodpressurediastolic { get; set; }

    [Column("heent")]
    [StringLength(50)]
    public string? Heent { get; set; }

    [Column("chest")]
    [StringLength(50)]
    public string? Chest { get; set; }

    [Column("hr")]
    public int? Hr { get; set; }

    [Column("allergies")]
    [StringLength(50)]
    public string? Allergies { get; set; }

    [Column("cv")]
    [StringLength(50)]
    public string? Cv { get; set; }

    [Column("abd")]
    [StringLength(50)]
    public string? Abd { get; set; }

    [Column("extr")]
    [StringLength(50)]
    public string? Extr { get; set; }

    [Column("skin")]
    [StringLength(50)]
    public string? Skin { get; set; }

    [Column("neuro")]
    [StringLength(50)]
    public string? Neuro { get; set; }

    [Column("diagnosis")]
    [StringLength(50)]
    public string? Diagnosis { get; set; }

    [Column("medicationsdispensed")]
    [StringLength(50)]
    public string? Medicationsdispensed { get; set; }

    [Column("followup")]
    [StringLength(50)]
    public string? Followup { get; set; }

    [Column("other")]
    [StringLength(50)]
    public string? Other { get; set; }

    [Column("treatmentplan")]
    [StringLength(50)]
    public string? Treatmentplan { get; set; }

    [Column("procedures")]
    [StringLength(50)]
    public string? Procedures { get; set; }

    [Column("rr")]
    public int? Rr { get; set; }

    [Column("pain")]
    public int? Pain { get; set; }

    [Column("ischanged")]
    public bool? Ischanged { get; set; }

    [Column("isfinalized")]
    public bool? Isfinalized { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [ForeignKey("Adminid")]
    [InverseProperty("Encounters")]
    public virtual Admin? Admin { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Encounters")]
    public virtual Physician? Physician { get; set; }

    [ForeignKey("Requestid")]
    [InverseProperty("Encounters")]
    public virtual Request Request { get; set; } = null!;
}
