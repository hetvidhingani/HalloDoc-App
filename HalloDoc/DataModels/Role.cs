using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.DataModels;

[Table("Role")]
[Index("RoleName", Name = "Unique_RoleName", IsUnique = true)]
public partial class Role
{
    [Key]
    [Column("RoleID")]
    public long RoleId { get; set; }

    [StringLength(50)]
    public string RoleName { get; set; } = null!;
}
