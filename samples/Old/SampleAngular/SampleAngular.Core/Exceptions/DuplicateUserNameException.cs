using System;
using System.Collections.Generic;
using System.Text;

namespace SampleAngular.Core.Exceptions
{
    public class DuplicateUserNameException : Exception
    {
        public string UserName { get; set; }

        public DuplicateUserNameException(string userName)
            : base($"User with UserName \"{userName}\" already exists.")
        {
            UserName = userName;
        }
    }
}
