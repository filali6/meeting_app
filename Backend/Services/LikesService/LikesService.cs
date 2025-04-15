using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Backend.Data;
using Backend.DTOs;
using Backend.Helpers;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.LikesService;

public class LikesService(DataContext context, IMapper mapper) : ILikeWriter, ILikeReader, ILikeBrowser
{
    private readonly DataContext _context = context;
    private readonly IMapper _mapper = mapper;

    public void AddLike(UserLike like)
    {
        _context.Likes.Add(like);
    }

    public void DeleteLike(UserLike like)
    {
        _context.Likes.Remove(like);
    }

    public async Task<IEnumerable<string>> GetCurrentUserLikeIds(string userId)
    {
        return await _context.Likes
            .Where(l => l.SourceUserId == userId)
            .Select(l => l.TargetUserId)
            .ToListAsync();
    }

    public async Task<UserLike?> GetUserLike(string sourceUser, string TargetUser)
    {
        return await _context.Likes.FirstOrDefaultAsync(l => l.SourceUserId == sourceUser && l.TargetUserId == TargetUser);
    }

    public async Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams)
    {
        var userId = likesParams.userId;
        IQueryable<MemberDto> query;
        var likes = _context.Likes.AsQueryable();
        switch (likesParams.predicate)
        {
            case "like":
                query = likes.Where(x => x.SourceUserId == likesParams.userId)
                    .Select(x => x.TargetUser)
                    .ProjectTo<MemberDto>(_mapper.ConfigurationProvider);
                break;
            case "likedBy":
                query = likes.Where(x => x.TargetUserId == likesParams.userId)
                    .Select(x => x.SourceUser)
                    .ProjectTo<MemberDto>(_mapper.ConfigurationProvider);
                break;
            default:
                var likedIds = await GetCurrentUserLikeIds(likesParams.userId!);
                query = likes.Where(x => x.TargetUserId == likesParams.userId && likedIds.Contains(x.SourceUserId))
                    .Select(x => x.SourceUser)
                    .ProjectTo<MemberDto>(_mapper.ConfigurationProvider);
                break;
        }
        return await PagedList<MemberDto>.CreatAsync(query,likesParams.pageNumber,likesParams.PageSize);
    }


}
