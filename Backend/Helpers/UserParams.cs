using System;

namespace Backend.Helpers;

public class UserParams:PaginationParams
{
    public string? Gender{get;set;}
    public string? CurrentUser{get;set;}
    public int MinAge { get; set; } = 18;
    public int MaxAge { get; set; } = 80;
}
