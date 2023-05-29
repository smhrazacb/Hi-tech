namespace WebHookTest.Services;

public interface IGrantUrlTesterService
{
    Task<bool> TestGrantUrl(string urlHook, string url, string token);
}
