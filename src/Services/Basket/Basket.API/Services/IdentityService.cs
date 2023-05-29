using OpenIddict.Abstractions;

namespace Basket.API.Services
.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _IHttpContextAccessor;

        public IdentityService(IHttpContextAccessor context)
        {
            _IHttpContextAccessor = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string GetUserIdentity()
        {
            return _IHttpContextAccessor.HttpContext.User.FindFirst(OpenIddictConstants.Claims.Username).Value;
        }

        public string GetUserName()
        {
            return _IHttpContextAccessor.HttpContext.User.Identity.Name;
        }
    }
}
