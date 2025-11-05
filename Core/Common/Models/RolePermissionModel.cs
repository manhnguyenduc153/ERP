using ERP_API.Enums;
using System.Collections.Generic;

namespace ERP_API.Models
{
    public class RolePermissionModel
    {
        public string RoleName { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
