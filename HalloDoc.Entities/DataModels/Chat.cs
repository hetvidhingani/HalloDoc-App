using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Entities.DataModels;

[Table("Chat")]
public partial class Chat
{
    [Key]
    [Column("connectionId")]
    public int ConnectionId { get; set; }

    [Column("senderId", TypeName = "character varying")]
    public string SenderId { get; set; } = null!;

    [Column("recieverId", TypeName = "character varying")]
    public string? RecieverId { get; set; }

    [StringLength(500)]
    public string Message { get; set; } = null!;

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ChatDate { get; set; }

    [StringLength(100)]
    public string? ChatBoxClass { get; set; }
}
