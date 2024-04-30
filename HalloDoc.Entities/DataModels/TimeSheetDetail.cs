using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Entities.DataModels;

public partial class TimeSheetDetail
{
    [Key]
    public int TimeDetailsId { get; set; }

    public DateOnly Date { get; set; }

    public int? TotalHours { get; set; }

    public int? OnCallHours { get; set; }

    public int? NumbersOfShift { get; set; }

    public int? HouseCall { get; set; }

    public int? NightShiftWeekend { get; set; }

    public int? HouseCallNightWeekend { get; set; }

    public int? PhoneConsult { get; set; }

    public int? PhoneNightWeekend { get; set; }

    public int? Amount { get; set; }

    public int? Batchtest { get; set; }

    public int? TotalAmount { get; set; }

    public int? BonusAmount { get; set; }

    public int? TimeSheetId { get; set; }

    [Column("holiday", TypeName = "bit(1)")]
    public BitArray? Holiday { get; set; }

    [StringLength(100)]
    public string? Item { get; set; }

    [StringLength(100)]
    public string? Bill { get; set; }

    [Column("isReceipt", TypeName = "bit(1)")]
    public BitArray? IsReceipt { get; set; }

    [ForeignKey("TimeSheetId")]
    [InverseProperty("TimeSheetDetails")]
    public virtual QuarterSheet? TimeSheet { get; set; }
}
