using System;

namespace Backend.Helpers;

public class LikesParams:PaginationParams
{
    public int userId { get; set; }
    public required string predicate { get; set; }="like";
}
