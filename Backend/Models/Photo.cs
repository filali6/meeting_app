using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;
[Table("Photos")]
public class Photo
{
     public int Id { get; set; }
     public required string Url { get; set; }
     public bool IsMain { get; set; }=false;
     public string? PublicId {get;set;}
     public  string? AppUserId{get;set;}="";
     public AppUser AppUser {get;set;}=null!;
     public bool Approuved{get;set;}=false;

}
