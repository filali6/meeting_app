using System;

namespace Backend.DTOs;

public class GetUserLoginDTO
{
    public required string username { get; set; }
    public required string token { get; set; }
}
