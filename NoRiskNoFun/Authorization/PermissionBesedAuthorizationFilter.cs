using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NoRiskNoFun.Data;
using System.Security.Claims;

namespace NoRiskNoFun.Authorization
{
    public class PermissionBesedAuthorizationFilter(ApplicationDbContext dbContext) : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var attribute = (CheckPermissionAattribute)context.ActionDescriptor.EndpointMetadata.FirstOrDefault(x=> x is CheckPermissionAattribute);
            if (attribute != null)
            {
                var claimsIdentity = context.HttpContext.User.Identities as ClaimsIdentity;

                  if (claimsIdentity == null || !claimsIdentity.IsAuthenticated)
                 {
                    context.Result = new ForbidResult();            
                 }
                 else
                 {
                    var userId = int.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                    var hasPermission = dbContext.Set<UserPermissions>().Any(up => up.UserId == userId && up.PermissionId == attribute.Permission);
                    if (!hasPermission)
                    {
                        context.Result = new ForbidResult();
                    }

                }

                
            }
        }
    }
}
