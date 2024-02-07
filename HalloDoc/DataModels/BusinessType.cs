using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.DataModels;

[Table("BusinessType")]
public partial class BusinessType
{
    [Key]
    public int BusinessTypeId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("BusinessType")]
    public virtual ICollection<Business> Businesses { get; set; } = new List<Business>();
}
