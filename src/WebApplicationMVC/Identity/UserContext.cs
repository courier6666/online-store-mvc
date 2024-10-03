using Microsoft.AspNetCore.Http;
using Store.Domain.Entities.Interfaces;
using Store.WebApplicationMVC.Extensions;

namespace Store.WebApplicationMVC.Identity
{
    public sealed class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public UserContext(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public bool IsAuthenticated => _contextAccessor
            .HttpContext?
            .User
            .Identity?
            .IsAuthenticated ??
        throw new ApplicationException("User context is unavailable");

        public Guid? UserId => _contextAccessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new ApplicationException("User context is unavailable");
    }
}
