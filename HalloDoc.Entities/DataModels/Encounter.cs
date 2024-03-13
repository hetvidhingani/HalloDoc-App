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
    public int? Requestid { get; set; }

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
    public DateOnly? Dateofbirth { get; set; }

    [Column("phonenumber")]
    [StringLength(20)]
    public string? Phonenumber { get; set; }

    [Column("medicalreport")]
    public string? Medicalreport { get; set; }

    [Column("date")]
    public DateOnly? Date { get; set; }

    [Column("email")]
    [StringLength(50)]
    public string? Email { get; set; }

    [Column("historyofpresentillness")]
    public string? Historyofpresentillness { get; set; }

    [Column("medicalhistory")]
    public string? Medicalhistory { get; set; }

    [Column("medications")]
    public string? Medications { get; set; }

    [Column("temp")]
    [Precision(5, 2)]
    public decimal? Temp { get; set; }

    [Column("bloodpressuresystolic")]
    public int? Bloodpressuresystolic { get; set; }

    [Column("bloodpressurediastolic")]
    public int? Bloodpressurediastolic { get; set; }

    [Column("heent")]
    public string? Heent { get; set; }

    [Column("chest")]
    public string? Chest { get; set; }

    [Column("hr")]
    public int? Hr { get; set; }

    [Column("allergies")]
    public string? Allergies { get; set; }

    [Column("cv")]
    public string? Cv { get; set; }

    [Column("abd")]
    public string? Abd { get; set; }

    [Column("extr")]
    public string? Extr { get; set; }

    [Column("skin")]
    public string? Skin { get; set; }

    [Column("neuro")]
    public string? Neuro { get; set; }

    [Column("diagnosis")]
    public string? Diagnosis { get; set; }

    [Column("medicationsdispensed")]
    public string? Medicationsdispensed { get; set; }

    [Column("followup")]
    public string? Followup { get; set; }

    [Column("other")]
    public string? Other { get; set; }

    [Column("treatmentplan")]
    public string? Treatmentplan { get; set; }

    [Column("procedures")]
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
}
