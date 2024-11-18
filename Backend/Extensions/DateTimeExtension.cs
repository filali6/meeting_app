using System;

namespace Backend.Extensions;

public static class DateTimeExtension
{
    public static int CalculateAge(this DateOnly birth ){
        var today=DateOnly.FromDateTime(DateTime.Now);
        var age =today.Year-birth.Year;
        if (birth > today.AddYears(-age)) 
        age--;
        return age;
    }
}
