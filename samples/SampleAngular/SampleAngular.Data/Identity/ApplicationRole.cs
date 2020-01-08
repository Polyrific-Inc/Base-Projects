using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SampleAngular.Data.Identity
{
    public class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }

        public ApplicationRole(int roleId, string roleName) : base(roleName)
        {
            Id = roleId;
            NormalizedName = roleName.ToUpper();
        }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}

