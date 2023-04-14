namespace Ordering.Infrastructure.Repositories.Services
{
    public interface IIdentityService
    {
        string GetUserIdentity();

        string GetUserName();
    }
}
