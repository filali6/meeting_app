using System;
using Backend.Models;

namespace Backend.DTOs;

public class MessageDTO
{
    public int Id { get; set; }
    public required string SourcePhotoUrl { get; set; }
    public required string SourceUsername{ get; set; }
    public int SourceUserId { get; set; }
    public required string TargetPhotoUrl { get; set; }
    public required string TargetUsername{ get; set; }
    public int TargetUserId { get; set; }
    public required string Content { get; set; }
    public DateTime? ReadDate { get; set; }
    public DateTime SentDate { get; set; } 
}
