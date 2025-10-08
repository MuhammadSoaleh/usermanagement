using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace usermanagement.Filters
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

            if (string.IsNullOrEmpty(isAdminClaim))
            {
                // Claim missing → Unauthorized
                context.Result = new ForbidResult();
                return;
            }

            if (isAdminClaim.ToLower() == "true")
            {
                // Admin → Allow
                return;
            }
            else if (isAdminClaim.ToLower() == "false")
            {
                // Employee → You can log or do something here
                // Example: redirect to employee dashboard or deny
                context.Result = new RedirectToActionResult("Index", "Employee", null);
                return;
            }

            // Unknown value → Forbidden
            context.Result = new ForbidResult();
        }
    }
}
