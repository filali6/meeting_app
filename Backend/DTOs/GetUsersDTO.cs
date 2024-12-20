using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class GetUsersDTO
{

    public required string username { get; set; }
    public required string id { get; set; }
}