using System;

namespace Backend.Models;

public class Message
{
    public int Id{get;set;}
    public AppUser SourceUser { get; set; } = null!;
    public int SourceUserId { get; set; }
    public AppUser TargetUser { get; set; } = null!;
    public int TargetUserId { get; set; }
    public required string Content { get; set; }
    public DateTime? ReadDate {get;set;}
    public DateTime SentDate { get; set; }=DateTime.UtcNow;
    public bool SourceDeleted { get; set; }
    public bool TargetDeleted { get; set; }

}
