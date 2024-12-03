using System;

namespace Backend.Helpers;

public class UserParams
{
    private const int MaxPageSize = 20;
    public int pageNumber { get; set; } = 1;
    private int _pageSize = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}
