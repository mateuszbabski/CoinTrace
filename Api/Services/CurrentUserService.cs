using Application.Interfaces;
using System.Security.Claims;

namespace Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public int UserId => int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}
