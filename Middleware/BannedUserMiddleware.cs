using Services;

public class BannedUserMiddleware
{
    private readonly RequestDelegate _next;

    public BannedUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserService userService)
    {
        // Skip if unauthenticated or identity missing
        var isAuthenticated = context?.User?.Identity?.IsAuthenticated ?? false;
        if (!isAuthenticated)
        {
            await _next(context!);
            return;
        }

        var userIdClaim = context!.User.FindFirst("id")?.Value;
        if (userIdClaim != null)
        {
            var user = await userService.GetUserByIdAsync(int.Parse(userIdClaim));
            if (user != null && user.banned)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Your account has been banned.");
                return;
            }
        }

        await _next(context);
    }
}