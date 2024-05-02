using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Entities.DataModels;

[Table("Category")]
public partial class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Column("Category")]
    [StringLength(100)]
    public string Category1 { get; set; } = null!;

    [InverseProperty("Catagory")]
    public virtual ICollection<PayRate> PayRates { get; set; } = new List<PayRate>();
}
