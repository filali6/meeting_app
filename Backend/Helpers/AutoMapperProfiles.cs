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
            .ForMember(d => d.Age, o => o.MapFrom(u => u.GetAge()))
            .ForMember(d => d.PhotoUrl, o => o.MapFrom(u => u.GetMainPhotoUrl()));
        CreateMap<Photo,PhotoDTO>();
        CreateMap<EditMemberDTO,AppUser>();
        CreateMap<RegisterDTO,AppUser>();
        CreateMap<Message,MessageDTO>()
        .ForMember(d=>d.TargetPhotoUrl,o=>o.MapFrom(s=>s.TargetUser.Photos.FirstOrDefault(x=>x.IsMain)!.Url!=null
                                                    ?s.TargetUser.Photos.FirstOrDefault(x=>x.IsMain)!.Url
                                                    :"https://res.cloudinary.com/dnuqsdk2a/image/upload/v1733093753/user_yeqwso.png"))
        .ForMember(d=>d.SourcePhotoUrl,o=>o.MapFrom(s=>s.SourceUser.Photos.FirstOrDefault(x=>x.IsMain)!.Url!=null
                                                    ?s.SourceUser.Photos.FirstOrDefault(x=>x.IsMain)!.Url
                                                    :"https://res.cloudinary.com/dnuqsdk2a/image/upload/v1733093753/user_yeqwso.png"))
        .ForMember(d=>d.SourceUsername,o=>o.MapFrom(s=>s.SourceUser.UserName))
        .ForMember(d=>d.TargetUsername,o=>o.MapFrom(s=>s.TargetUser.UserName));

        CreateMap<DateTime,DateTime>().ConvertUsing(d=>DateTime.SpecifyKind(d,DateTimeKind.Utc));
        CreateMap<DateTime?,DateTime?>().ConvertUsing(d=>d.HasValue?DateTime.SpecifyKind(d.Value,DateTimeKind.Utc):null);
    }

}
