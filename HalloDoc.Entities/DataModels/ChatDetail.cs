using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Entities.DataModels;

[Table("ChatDetail")]
public partial class ChatDetail
{
    [Key]
    public int ChatId { get; set; }

    [StringLength(100)]
    public string SenderId { get; set; } = null!;

    [StringLength(100)]
    public string RecieverId { get; set; } = null!;

    [StringLength(500)]
    public string? Message { get; set; }
}
