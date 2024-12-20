using System;

namespace Backend.Helpers;

public class LikesParams:PaginationParams
{
    public string? userId { get; set; }
    public required string predicate { get; set; }="like";
}
