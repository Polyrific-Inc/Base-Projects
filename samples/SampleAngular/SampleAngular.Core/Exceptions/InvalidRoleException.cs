using System;
using System.Collections.Generic;
using System.Text;

namespace SampleAngular.Core.Exceptions
{
    public class InvalidRoleException : Exception
    {
        public InvalidRoleException(string roleName) : base($"Role \"{roleName}\" is not valid.")
        {
            RoleName = roleName;
        }

        public string RoleName { get; }
    }
}
