using System.Security.Claims;

namespace Application.Interfaces
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        ClaimsPrincipal User { get; }
    }
}