using System;

namespace Backend.Exceptions;

public class ApiExceptions(int satusCode, string errorMessage, string? details)
{
    public int satusCode { get; set; }=satusCode;
    public string? errorMessage { get; set; } = errorMessage;
    public  string? details {get;set;} =details;

}
