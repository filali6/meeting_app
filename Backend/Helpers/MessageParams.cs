using System;
using CloudinaryDotNet.Actions;

namespace Backend.Helpers;

public class MessageParams : PaginationParams
{
    public string Container { get; set; } = "Unread";
    public string? Username { get; set; }
}
