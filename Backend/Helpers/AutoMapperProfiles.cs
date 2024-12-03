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
        .ForMember(d=>d.PhotoUrl,o=>o.MapFrom(s=>s.Photos.FirstOrDefault(x=>x.IsMain)!.Url!=null?
                                                        s.Photos.FirstOrDefault(x=>x.IsMain)!.Url:"https://res.cloudinary.com/dnuqsdk2a/image/upload/v1733093753/user_yeqwso.png"));
        CreateMap<Photo,PhotoDTO>();
        CreateMap<EditMemberDTO,AppUser>();
        CreateMap<RegisterDTO,AppUser>();

    }

}
