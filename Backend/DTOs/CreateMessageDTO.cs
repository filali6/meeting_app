using System;

namespace Backend.DTOs;

public class CreateMessageDTO
{
    public required string TargetUsername { get; set; }
    public required string Content { get; set; }
}
