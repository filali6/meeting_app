using System;
using AutoMapper;
using Backend.DTOs;
using Backend.Extensions;
using Backend.Models;

namespace Backend.Helpers;

public class AutoMapperProfiles:Profile
{
    public AutoMapperProfiles(){
        CreateMap<AppUser,MemberDto>()
        .ForMember(d=>d.Age,o=>o.MapFrom(x=>x.DateBirth.CalculateAge()))
        .ForMember(d=>d.PhotoUrl,o=>o.MapFrom(s=>s.Photos.FirstOrDefault(x=>x.IsMain)!.Url));
        CreateMap<Photo,PhotoDTO>();
    }

}
