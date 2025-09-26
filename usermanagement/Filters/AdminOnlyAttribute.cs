using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace usermanagement.Filters // <-- Change to match your project namespace
{
    public class AdminOnlyAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? false)
            {
                // User not logged in → redirect to login
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            var isAdminClaim = user.FindFirst("IsAdmin")?.Value;

            if (string.IsNullOrEmpty(isAdminClaim) || isAdminClaim.ToLower() != "true")
            {
                // Not an admin → 403 Forbidden
                context.Result = new ForbidResult();
            }
        }
    }
}
