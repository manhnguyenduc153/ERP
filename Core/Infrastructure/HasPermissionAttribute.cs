using ERP_API.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ERP_API.Authorization
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission)
        {
            Policy = $"Permission.{permission}";
        }
    }
}
