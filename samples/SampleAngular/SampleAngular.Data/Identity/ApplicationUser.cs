using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SampleAngular.Data.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        {
        }

        public ApplicationUser(string userName) : base(userName)
        {
        }

        public ApplicationUser(int userId, string userEmail) : base(userEmail)
        {
            Id = userId;
            NormalizedUserName = userEmail.ToUpper();
            Email = userEmail;
            NormalizedEmail = userEmail.ToUpper();
        }
        public bool IsActive { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public virtual ICollection<ApplicationUserRole> Roles { get; set; }
    }
}

