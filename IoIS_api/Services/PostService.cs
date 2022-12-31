using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialApp.API.WebAPI.Dtos;
using SocialApp.API.WebAPI.Models;
using SocialApp.API.WebAPI.Models.Entities;
using SocialApp.API.WebAPI.Services.Interfaces;
using SocialApp.API.WebAPI.ViewModels;

namespace SocialApp.API.WebAPI.Services
{
    public class PostService : IPostService
    {
        private readonly SocialAppContext _context;
        private readonly IMapper _mapper;

        public PostService(SocialAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PostVM> CreateAsync(NewPostDto dto, User user)
        {
            var group = await _context.Groups.Where(g => g.Id == dto.GroupId).FirstOrDefaultAsync();

            if (dto.GroupId != null && group == null)
            {
                return null;
            }

            var post = _mapper.Map<Post>(dto);
            post.CreatedBy = user;
            post.CreatedAt = DateTime.Now;

            _context.Add(post);
            await _context.SaveChangesAsync();

            return _mapper.Map<PostVM>(post);
        }

        public async Task<PostVM> GetByIdAsync(int id)
        {
            return await _context.Posts
                .Where(p => p.Id == id)
                .Select(p => _mapper.Map<PostVM>(p))
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<PostVM>> GetPostsOfUserAsync(string username)
        {
            return await _context.Posts
                .Include(p => p.Group)
                .Where(p => p.CreatedBy.UserName == username)
                .Select(p => _mapper.Map<PostVM>(p))
                .ToListAsync();
        }
    }
}
