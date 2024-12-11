using System;

namespace Backend.Helpers;

public class UserParams
{
    private const int MaxPageSize = 20;
    public int pageNumber { get; set; } = 1;
    private int _pageSize = 4;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    public string? Gender{get;set;}
    public string? CurrentUser{get;set;}
    public int MinAge { get; set; } = 18;
    public int MaxAge { get; set; } = 80;
}
