using System.Security.Claims;
using FoodPool.provider.interfaces;

namespace FoodPool.provider;

public class HttpContextProvider : IHttpContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetCurrentUser()
    {
        var id = _httpContextAccessor.HttpContext?.User.FindFirstValue("userId");
        return !int.TryParse(id, out var userId) ? 0 : userId;
    }
}