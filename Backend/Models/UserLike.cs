using System;

namespace Backend.Models;

public class UserLike
{
    public AppUser SourceUser { get; set; }=null!;
    public required string SourceUserId { get; set; }
    public AppUser TargetUser { get; set; }=null!;
    public required string TargetUserId { get; set; }
}
